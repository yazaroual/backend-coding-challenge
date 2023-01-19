using BackendApi.Dto;
using BackendApi.Enums;
using BackendApi.ErrorHandling;
using BackendApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Business;

/// <summary>
/// Business manager for the lessons
/// </summary>
public class LessonBusiness : ILessonBusiness
{
    private readonly ILogger<LessonBusiness> _logger;
    private readonly IAchievementBusiness _achievementBusiness;
    private readonly BackendApiContext _context;

    public LessonBusiness(
        BackendApiContext context,
        ILogger<LessonBusiness> logger,
        IAchievementBusiness achievementBusiness)
    {
        _logger = logger;
        _context = context;
        _achievementBusiness = achievementBusiness;
    }

    /// <summary>
    /// Save the progress of the user for a specific lesson.
    /// </summary>
    /// <param name="request">Contains the lesson ID, start and finish dates</param>
    public async Task SaveProgressAsync(SaveProgressDto request)
    {
        //Check parameters
        User user = await _context.User.FindAsync(request.UserId);

        if (user == null)
        {
            throw new BusinessException(ErrorCode.Forbidden, "Unable to find user");
        }

        Lesson lesson = await _context.Lesson.FindAsync(request.LessonId);

        if (lesson == null)
        {
            throw new BusinessException(ErrorCode.LessonNotFound, "Lesson not found");
        }

        if (request.StartedAt == default || request.CompletedAt == default)
        {
            throw new BusinessException(ErrorCode.LessonTimeMissing, "Start and completed datetime are requiered");
        }

        //Save progress of a specific lesson
        CompletedLesson completedLesson = new()
        {
            CompletedAt = request.CompletedAt,
            LessonId = lesson.Id,
            StartedAt = request.StartedAt,
            UserId = user.Id
        };

        await _context.CompletedLesson.AddAsync(completedLesson);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Lesson - SaveProgress - User {request.UserId} - {lesson.Id} saved as completed");

        //Now that the lesson is marked as completed, let's check if it was the final lesson of a chapter or course
        //If it's the case we update CompletedChapter and the associated achievements (Targeting chapters)
        //If the user completed a chapter, we also want to check if it was the last Chapter of a course
        //If it's the case we update CompletedCourse entity and the associated achievements (Targeting courses)

        _logger.LogInformation($"Lesson - SaveProgress - User {request.UserId} - Updating achievement ...");

        await _achievementBusiness.IncreaseAchievementProgressFor(user.Id, ObjectiveTarget.Lesson);

        bool anyChapterProgress = await CheckChaptersProgressAsync(user.Id, lesson.ChapterId);

        if (anyChapterProgress)
        {
            await _achievementBusiness.IncreaseAchievementProgressFor(user.Id, ObjectiveTarget.Chapter);

            var course = await _context.Course
                .Include(c => c.Chapters)
                .Where(c => c.Chapters.Any(ch => ch.Id == lesson.ChapterId))
                .FirstOrDefaultAsync();

            var anyCourseProgress = await CheckCourseProgressAsync(user.Id, course);

            if (anyCourseProgress)
            {
                await _achievementBusiness.IncreaseAchievementProgressFor(user.Id, ObjectiveTarget.Course);
            }

        }

        _logger.LogInformation($"Lesson - SaveProgress - User {request.UserId} - achievements were updated");
    }

    /// <summary>
    /// Check if a chapter was completed
    /// </summary>
    /// <param name="userId">User that may have completed a chapter</param>
    /// <param name="chapterId">Id of the chapter to verify</param>
    /// <returns>True if a chapter was completed</returns>
    private async Task<bool> CheckChaptersProgressAsync(int userId, int chapterId)
    {
        //Get all the completed lessons for this user and this chapter
        //If they are equal to the number of lessons this chapter have
        //Update the CompletedChapters entity and return true

        int completedLessonCounter = await _context.CompletedLesson
            .Include(c => c.Lesson).ThenInclude(l => l.Chapter)
            .Where(c => c.UserId == userId)
            .Where(c => c.Lesson.ChapterId == chapterId)
            .CountAsync();

        int chapterNumberOfLessons = await _context.Chapter
            .Where(c => c.Id == chapterId)
            .Select(c => c.LessonsNumber)
            .FirstOrDefaultAsync();

        //As the users can complete multiple times the same lessons
        if (completedLessonCounter >= chapterNumberOfLessons)
        {
            await _context.CompletedChapter.AddAsync(new CompletedChapter()
            {
                ChapterId = chapterId,
                UserId = userId,
                CompletedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Check if a course was completed
    /// </summary>
    /// <param name="userId">User that may have completed a course</param>
    /// <param name="course">Course to verify</param>
    /// <returns>True if the course was completed</returns>
    private async Task<bool> CheckCourseProgressAsync(int userId, Course course)
    {
        //Get all the completed chapters for this user and course
        //If the count is equal to the number of chapters in this course
        //Update the completed chapters and return true

        int completedChaptersCounter = await _context.CompletedChapter
            .Include(c => c.Chapter).ThenInclude(ch => ch.Course)
            .Where(c => c.UserId == userId)
            .Where(c => c.Chapter.CourseId == course.Id)
            .CountAsync();

        int chapterNumberOfChapters = course.ChaptersNumber;

        //As the users can complete multiple times the same chapters
        if (completedChaptersCounter >= chapterNumberOfChapters)
        {
            await _context.CompletedCourse.AddAsync(new CompletedCourse()
            {
                CourseId = course.Id,
                UserId = userId,
                CompletedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}