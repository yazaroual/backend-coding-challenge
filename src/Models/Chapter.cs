using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models;

public class Chapter
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public int LessonsNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Lesson> Lessons { get; set; }
}