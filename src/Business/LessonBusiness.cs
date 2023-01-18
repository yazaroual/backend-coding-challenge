using BackendApi.Dto;

namespace BackendApi.Business;

/// <summary>
/// Business manager for the lessons
/// </summary>
public class LessonBusiness : ILessonBusiness
{
    /// <summary>
    /// Save the progress of the user for a specific lesson.
    /// </summary>
    /// <param name="request">Contains the lesson ID, start and finish dates</param>
    public async Task SaveProgressAsync(SaveProgressDto request)
    {
    }
}