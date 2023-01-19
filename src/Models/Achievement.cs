using System.ComponentModel.DataAnnotations.Schema;
using BackendApi.Enums;

namespace BackendApi.Models;

public class Achievement
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    /// <summary>
    /// Number of tasks of a certain target to get the achievement.
    /// </summary>
    public int ObjectiveGoal { get; set; }

    /// <summary>
    /// Type of target to which the achievement apply.
    /// Ex : Courses, Lessons or Chapters
    /// </summary>
    public ObjectiveTarget ObjectiveTarget { get; set; }

    /// <summary>
    /// If the achievement is related to a course, a course Id will be specified
    /// </summary>
    public int? CourseId { get; set; }

    /// <summary>
    /// If the achievement is related to a course, a course will be specified
    /// </summary>
    public Course Course { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}