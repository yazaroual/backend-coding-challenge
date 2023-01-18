using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models;

public class UserLesson
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public Lesson Lesson { get; set; }
    public int LessonId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime CompletedAt { get; set; }
}