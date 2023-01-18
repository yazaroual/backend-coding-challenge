using BackendApi.Dto;
using BackendApi.Enums;
using BackendApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Business;

/// <summary>
/// Business manager for the achievements
/// </summary>
public class AchievementBusiness : IAchievementBusiness
{
    private readonly ILogger<AchievementBusiness> _Logger;
    private readonly BackendApiContext _context;

    public AchievementBusiness(
        BackendApiContext context,
        ILogger<AchievementBusiness> logger)
    {
        _Logger = logger;
        _context = context;
    }

    /// <summary>
    /// Calculate the current achievements of a user.
    /// To be called when a lesson is completed. It will update all the achievements counters of the user.
    /// </summary>
    /// <param name="userId">User that just completed a lesson</param>
    /// <param name="completedLesson">The completed lesson</param>
    public async Task CalculateAchievementsAsync(int userId, Lesson completedLesson)
    {
        //A lesson was completed, it means we must automatically update the progress
        //of all the achievements linked to lessons.
        //Once it's done, we need to check if the user closed all the chapters of a lesson.
        //  To do so, we take the number of lesson in a chapter AND we count the number of lesson completed by the user
        //  For this chapter. If they are egal. => Update progress for all achievements linked to chapter
        //Do the same again for chapters and Courses
        //After that all the achievements will be completed

        //TODO : Add a number of chapters in the course entity and numbers of lessons in the chapter entity
        //Maybe we need a UserChapter table ? To count the number of chapters completed ?
    }

    /// <summary>
    /// Increase the progress for all the achivements of a user linked to a specific target (Course, lesson or chapter)
    /// </summary>
    /// <param name="userId">User that we need to update their progress to</param>
    /// <param name="target">Type of achivements we want to increase (Course, lesson or chapter)</param>
    /// <returns>The updated list of user achievements</returns>
    private async Task<List<UserAchievement>> IncreaseAchievementProgressFor(int userId, ObjectiveTarget target)
    {
        var achievements = await _context.UserAchievement
            .Include(a => a.Achievement)
            .Where(a => a.UserId == userId)
            .Where(a => a.Achievement.ObjectiveTarget == target)
            .ToListAsync();

        foreach (var ach in achievements)
        {
            ach.Progress += 1;
            if (ach.Progress == ach.Achievement.ObjectiveGoal)
            {
                ach.CompletedAt = DateTime.UtcNow;
            }
            ach.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return achievements;
    }


    /// <summary>
    /// List all the achievements of a specific user
    /// </summary>
    /// <param name="userId">User Id for whom we want the achievements</param>
    /// <returns>A list of AchievementDto</returns>
    public async Task<List<AchievementDto>> ListAchievementsAsync(int userId)
    {
        return await _context.UserAchievement
            .Where(u => u.UserId == userId)
            .OrderByDescending(d => d.CreatedAt)
            .Select(a => new AchievementDto()
            {
                Id = a.Id,
                Completed = a.CompletedAt != default,
                Progress = a.Progress
            })
            .ToListAsync();
    }

}