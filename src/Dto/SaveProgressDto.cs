namespace BackendApi.Dto;

/// <summary>
/// DTO to indiquate which lesson was completed and at what time it was started and finished.
/// </summary>
public class SaveProgressDto
{
    public int LessonId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime CompletedAt { get; set; }
}