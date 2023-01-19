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
    /// Increase the progress for all the achivements of a user linked to a specific target (Course, lesson or chapter)
    /// </summary>
    /// <param name="userId">User that we need to update their progress to</param>
    /// <param name="target">Type of achivements we want to increase (Course, lesson or chapter)</param>
    /// <returns>The updated list of user achievements</returns>
    public async Task<List<UserAchievement>> IncreaseAchievementProgressFor(int userId, ObjectiveTarget target)
    {
        _Logger.LogInformation($"Achievements - Progress - User {userId} - Updating progress for {target}");

        var achievements = await _context.UserAchievement
            .Include(a => a.Achievement)
            .Where(a => a.UserId == userId)
            .Where(a => a.Achievement.ObjectiveTarget == target)
            //We only want to handle not yet completed achievements
            .Where(a => a.Achievement.ObjectiveGoal > a.Progress)
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