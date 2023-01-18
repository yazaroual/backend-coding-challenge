using BackendApi.Dto;

namespace BackendApi.Business;

/// <summary>
/// Interface of the manager for the lessons
/// </summary>
public interface ILessonBusiness
{
    /// <summary>
    /// Save the progress of the user for a specific lesson.
    /// </summary>
    /// <param name="request">Contains the lesson ID, start and finish dates</param>
    Task SaveProgressAsync(SaveProgressDto request);
}
