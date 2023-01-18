using BackendApi.Business;
using BackendApi.Dto;
using BackendApi.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class LessonController : ControllerBase
{

    private readonly ILogger<LessonController> _logger;
    private readonly ILessonBusiness _lessonBusiness;

    public LessonController(ILogger<LessonController> logger, ILessonBusiness lessonBusiness)
    {
        _logger = logger;
        _lessonBusiness = lessonBusiness;
    }

    /// <summary>
    /// Complete a lesson for a user
    /// </summary>
    /// <response code="200">Update success</response>
    /// <response code="400">Update request is not valid. Errors can be "message=code" :
    /// `User was not found = 2`
    /// `LessonNotFound = 4`
    /// `LessonTimeMissing = 5`
    /// </response>
    [HttpPost]
    [Route("save-progress")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(BusinessError), 400)]
    public async Task<IActionResult> SaveProgressAsync(SaveProgressDto request)
    {
        _logger.LogInformation($"Lesson - SaveProgress - User {request.UserId} - Updating user progress ...");
        await _lessonBusiness.SaveProgressAsync(request);
        return NoContent();
    }
}