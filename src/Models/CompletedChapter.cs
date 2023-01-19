using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models;

public class CompletedChapter
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public Chapter Chapter { get; set; }
    public int ChapterId { get; set; }
    public DateTime CompletedAt { get; set; }
}