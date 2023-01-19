using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models;

public class CompletedCourse
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public Course Course { get; set; }
    public int CourseId { get; set; }
    public DateTime CompletedAt { get; set; }
}