using Microsoft.EntityFrameworkCore;

namespace BackendApi.Models;

public class BackendApiContext : DbContext
{
    public DbSet<Achievement> Achievement { get; set; }
    public DbSet<Chapter> Chapter { get; set; }
    public DbSet<Course> Course { get; set; }
    public DbSet<Lesson> Lesson { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<UserAchievement> UserAchievement { get; set; }
    public DbSet<UserLesson> UserLesson { get; set; }

    public BackendApiContext(DbContextOptions<BackendApiContext> options)
       : base(options)
    {
    }
}