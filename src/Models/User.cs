using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models;

public class User
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<UserAchievement> Achievements { get; set; }
    public List<CompletedLesson> CompletedLessons { get; set; }
    public List<CompletedChapter> CompletedChapters { get; set; }
    public List<CompletedCourse> CompletedCourses { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}