using BackendApi.Dto;
using BackendApi.Enums;
using BackendApi.Models;

namespace BackendApi.Business;

/// <summary>
/// Interface for achievements Business manager
/// </summary>
public interface IAchievementBusiness
{
    /// <summary>
    /// Increase the progress for all the achivements of a user linked to a specific target (Course, lesson or chapter)
    /// </summary>
    /// <param name="userId">User that we need to update their progress to</param>
    /// <param name="target">Type of achivements we want to increase (Course, lesson or chapter)</param>
    /// <returns>The updated list of user achievements</returns>
    Task<List<UserAchievement>> IncreaseAchievementProgressFor(int userId, ObjectiveTarget target);

    /// <summary>
    /// List all the achievements of a specific user
    /// </summary>
    /// <param name="userId">User Id for whom we want the achievements</param>
    /// <returns>A list of AchievementDto</returns>
    Task<List<AchievementDto>> ListAchievementsAsync(int userId);

}
