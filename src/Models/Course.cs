using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models;

public class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Chapter> Chapters { get; set; }
}