using Microsoft.AspNetCore.Mvc;

namespace BackendApi;

[ApiController]
[Route("[controller]")]
public class LessonController : ControllerBase
{

    private readonly ILogger<LessonController> _logger;

    public LessonController(ILogger<LessonController> logger)
    {
        _logger = logger;
    }
}