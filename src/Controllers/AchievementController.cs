using BackendApi.Business;
using BackendApi.ErrorHandling;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi;

[ApiController]
[Route("[controller]")]
public class AchievementController : ControllerBase
{

    private readonly ILogger<AchievementController> _logger;
    private readonly IAchievementBusiness _achievementBusiness;

    public AchievementController(ILogger<AchievementController> logger, IAchievementBusiness achievementBusiness)
    {
        _logger = logger;
        _achievementBusiness = achievementBusiness;
    }

    /// <summary>
    /// List of achievements of a user
    /// </summary>
    /// <response code="200">List of achievements</response>
    [HttpGet]
    [ProducesResponseType(204)]
    public async Task<IActionResult> List(int userId)
    {
        _logger.LogInformation($"Achievements - List - User {userId} - Getting achievements ...");
        var result = await _achievementBusiness.ListAchievementsAsync(userId);
        _logger.LogInformation($"Achievements - List - User {userId} - Returning {result.Count} achievements.");
        return Ok(result);
    }
}