using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models;

public class Lesson
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
    public string Content { get; set; }
    public int ChapterId { get; set; }
    public Chapter Chapter { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}