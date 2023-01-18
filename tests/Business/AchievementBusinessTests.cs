using BackendApi.Business;
using BackendApi.Dto;
using BackendApi.Enums;
using BackendApi.ErrorHandling;
using BackendApi.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BackendTests;

[Collection("AchievementBusiness")]
public class AchievementBusinessTests
{

    private const string InMemoryConnectionString = "DataSource=:memory:";
    private readonly SqliteConnection _connection;
    private readonly BackendApiContext _context;

    //Mocks
    private readonly Mock<ILogger<AchievementBusiness>> _loggerStub;

    //Business class for tests
    private readonly AchievementBusiness _business;


    public AchievementBusinessTests()
    {
        _connection = new SqliteConnection(InMemoryConnectionString);
        _connection.Open();
        var options = new DbContextOptionsBuilder<BackendApiContext>()
                .UseSqlite(_connection)
                .Options;

        _context = new BackendApiContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();


        // Mocks
        _loggerStub = new();

        _business = new(_context, _loggerStub.Object);

        SeedTestDatabase();

    }

    protected void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    private void SeedTestDatabase()
    {
        _context.User.Add(new User()
        {
            Id = 1,
            FirstName = "User",
            LastName = "One",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        _context.Course.Add(new Course()
        {
            Id = 1,
            Name = "C#",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Chapters = new List<Chapter>(){
                new Chapter(){
                    Id = 1,
                    DisplayOrder = 1,
                    Name = "Keywords",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Lessons = new List<Lesson>(){
                        new Lesson(){
                            Id = 1,
                            Content = "Let's learn more about types! :)",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            DisplayOrder = 1,
                            Name = "Introduction"
                        }
                    }
                }
            }
        });

        _context.Achievement.AddRange(
            new Achievement()
            {
                Id = 1,
                Name = "Complete 5 lessons",
                ObjectiveGoal = 5,
                ObjectiveTarget = ObjectiveTarget.Lesson,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Achievement()
            {
                Id = 2,
                Name = "Complete 1 chapter",
                ObjectiveGoal = 1,
                ObjectiveTarget = ObjectiveTarget.Chapter,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Achievement()
            {
                Id = 3,
                Name = "Complete the C# course",
                ObjectiveGoal = 1,
                ObjectiveTarget = ObjectiveTarget.Course,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        _context.UserAchievement.AddRange(
            new UserAchievement()
            {
                AchievementId = 1,
                UserId = 1,
                CompletedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Progress = 5
            },
            new UserAchievement()
            {
                AchievementId = 2,
                UserId = 1,
                CreatedAt = DateTime.UtcNow.AddHours(-1),
                UpdatedAt = DateTime.UtcNow,
                Progress = 0
            }
        );

        _context.SaveChanges();
    }

    [Fact]
    public async Task ListAchievements_ReturnsOneCompleted()
    {
        int userIdWithOneAchievement = 1;

        List<AchievementDto> results = await _business.ListAchievementsAsync(userIdWithOneAchievement);

        Assert.NotEmpty(results);
        Assert.True(results.First().Completed);
    }

    [Fact]
    public async Task ListAchievements_ReturnsNotCompleted()
    {
        int userIdWithOneAchievement = 1;

        List<AchievementDto> results = await _business.ListAchievementsAsync(userIdWithOneAchievement);

        Assert.NotEmpty(results);
        Assert.False(results.Last().Completed);
    }


    [Fact]
    public async Task ListAchievements_ReturnsEmpty()
    {
        int userIdWithNoAchivements = 2;

        List<AchievementDto> results = await _business.ListAchievementsAsync(userIdWithNoAchivements);

        Assert.Empty(results);
    }
}