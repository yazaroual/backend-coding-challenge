using BackendApi.Dto;
using BackendApi.Models;

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
    /// <param name="completedLessonId">Id of the completed lesson</param>
    public async Task CalculateAchievementsAsync(int userId, int completedLessonId)
    {

    }

    /// <summary>
    /// List all the achievements of a specific user
    /// </summary>
    /// <param name="userId">User Id for whom we want the achievements</param>
    /// <returns>A list of AchievementDto</returns>
    public async Task<List<AchievementDto>> ListAchievementsAsync(int userId)
    {
        return new List<AchievementDto>();
    }

}