using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models;

public class UserAchievement
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public Achievement Achievement { get; set; }
    public int AchievementId { get; set; }
    public int Progress { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}