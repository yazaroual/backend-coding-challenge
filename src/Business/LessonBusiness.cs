using BackendApi.Dto;
using BackendApi.Enums;
using BackendApi.ErrorHandling;
using BackendApi.Models;

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
        UserLesson completedLesson = new()
        {
            CompletedAt = request.CompletedAt,
            LessonId = lesson.Id,
            StartedAt = request.StartedAt,
            UserId = user.Id
        };

        await _context.UserLesson.AddAsync(completedLesson);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Lesson - SaveProgress - User {request.UserId} - {lesson.Id} saved as completed");

        //Calculate the achivement progress
        await _achievementBusiness.CalculateAchievementsAsync(user.Id, lesson.Id);

        _logger.LogInformation($"Lesson - SaveProgress - User {request.UserId} - achievements were recalculated");
    }
}