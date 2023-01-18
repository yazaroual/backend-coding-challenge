namespace BackendApi.Dto;

/// <summary>
/// DTO to indiquate which lesson was completed and at what time it was started and finished.
/// </summary>
public class SaveProgressDto
{
    /// <summary>
    /// User that completed the lesson
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Completed lesson
    /// </summary>
    public int LessonId { get; set; }

    /// <summary>
    /// Time at which lesson was started
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// Time at which lesson was completed
    /// </summary>
    public DateTime CompletedAt { get; set; }
}