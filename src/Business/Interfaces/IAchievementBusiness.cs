using BackendApi.Dto;
using BackendApi.Models;

namespace BackendApi.Business;

/// <summary>
/// Interface for achievements Business manager
/// </summary>
public interface IAchievementBusiness
{
    /// <summary>
    /// Calculate the current achievements of a user.
    /// To be called when a lesson is completed. It will update all the achievements counters of the user.
    /// </summary>
    /// <param name="userId">User that just completed a lesson</param>
    /// <param name="completedLesson">The completed lesson</param>
    Task CalculateAchievementsAsync(int userId, Lesson completedLesson);

    /// <summary>
    /// List all the achievements of a specific user
    /// </summary>
    /// <param name="userId">User Id for whom we want the achievements</param>
    /// <returns>A list of AchievementDto</returns>
    Task<List<AchievementDto>> ListAchievementsAsync(int userId);
}
