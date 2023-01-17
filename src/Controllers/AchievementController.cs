using Microsoft.AspNetCore.Mvc;

namespace BackendApi;

[ApiController]
[Route("[controller]")]
public class AchievementController : ControllerBase
{

    private readonly ILogger<AchievementController> _logger;

    public AchievementController(ILogger<AchievementController> logger)
    {
        _logger = logger;
    }
}