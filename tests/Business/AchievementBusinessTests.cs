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
            ChaptersNumber = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Chapters = new List<Chapter>(){
                new Chapter(){
                    Id = 1,
                    DisplayOrder = 1,
                    Name = "Keywords",
                    LessonsNumber = 1,
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
            },
            new Achievement()
            {
                Id = 4,
                Name = "Complete 25 lessons",
                ObjectiveGoal = 25,
                ObjectiveTarget = ObjectiveTarget.Lesson,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Achievement()
            {
                Id = 5,
                Name = "Complete 5 chapters",
                ObjectiveGoal = 5,
                ObjectiveTarget = ObjectiveTarget.Chapter,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Achievement()
            {
                Id = 6,
                Name = "Complete 5 Course",
                ObjectiveGoal = 5,
                ObjectiveTarget = ObjectiveTarget.Course,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        _context.UserAchievement.AddRange(
            //Completed 5 Lesson
            new UserAchievement()
            {
                AchievementId = 1,
                UserId = 1,
                CompletedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Progress = 5
            },
            //Did not yet complete 25 Lessons
            new UserAchievement()
            {
                AchievementId = 4,
                UserId = 1,
                CompletedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Progress = 5
            },
            //Completed 1 chapter
            new UserAchievement()
            {
                AchievementId = 2,
                UserId = 1,
                CreatedAt = DateTime.UtcNow.AddHours(-1),
                UpdatedAt = DateTime.UtcNow,
                Progress = 0
            },
            //Did not yet complete 5 chapters
            new UserAchievement()
            {
                AchievementId = 5,
                UserId = 1,
                CompletedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Progress = 1
            },
            //Did not yet complete 5 courses
            new UserAchievement()
            {
                AchievementId = 6,
                UserId = 1,
                CompletedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Progress = 1
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

    [Fact]
    public async Task IncreaseAchievementProgressFor_Lesson()
    {
        int userThatCompletedALesson = 1;
        int counterBeforeUpdate = await _context.UserAchievement
            .Include(a => a.Achievement)
            .Where(a => a.UserId == userThatCompletedALesson)
            .Where(a => a.Achievement.ObjectiveTarget == ObjectiveTarget.Lesson)
            .Where(a => a.Achievement.ObjectiveGoal > a.Progress)
            .Select(a => a.Progress)
            .FirstOrDefaultAsync();

        await _business.IncreaseAchievementProgressFor(userThatCompletedALesson, ObjectiveTarget.Lesson);

        //Verify that the counter was updated
        int counterAfterUpdate = await _context.UserAchievement
            .Include(a => a.Achievement)
            .Where(a => a.UserId == userThatCompletedALesson)
            .Where(a => a.Achievement.ObjectiveTarget == ObjectiveTarget.Lesson)
            .Where(a => a.Achievement.ObjectiveGoal > a.Progress)
            .Select(a => a.Progress)
            .FirstOrDefaultAsync();

        Assert.True(counterAfterUpdate > counterBeforeUpdate);
    }

    [Fact]
    public async Task IncreaseAchievementProgressFor_Chapter()
    {
        int userThatCompletedALesson = 1;
        int counterBeforeUpdate = await _context.UserAchievement
            .Include(a => a.Achievement)
            .Where(a => a.UserId == userThatCompletedALesson)
            .Where(a => a.Achievement.ObjectiveTarget == ObjectiveTarget.Chapter)
            .Where(a => a.Achievement.ObjectiveGoal > a.Progress)
            .Select(a => a.Progress)
            .FirstOrDefaultAsync();

        await _business.IncreaseAchievementProgressFor(userThatCompletedALesson, ObjectiveTarget.Chapter);

        //Verify that the counter was updated
        int counterAfterUpdate = await _context.UserAchievement
            .Include(a => a.Achievement)
            .Where(a => a.UserId == userThatCompletedALesson)
            .Where(a => a.Achievement.ObjectiveTarget == ObjectiveTarget.Chapter)
            .Where(a => a.Achievement.ObjectiveGoal > a.Progress)
            .Select(a => a.Progress)
            .FirstOrDefaultAsync();

        Assert.True(counterAfterUpdate > counterBeforeUpdate);
    }

    [Fact]
    public async Task IncreaseAchievementProgressFor_Course()
    {
        int userThatCompletedALesson = 1;
        int counterBeforeUpdate = await _context.UserAchievement
            .Include(a => a.Achievement)
            .Where(a => a.UserId == userThatCompletedALesson)
            .Where(a => a.Achievement.ObjectiveTarget == ObjectiveTarget.Course)
            .Where(a => a.Achievement.ObjectiveGoal > a.Progress)
            .Select(a => a.Progress)
            .FirstOrDefaultAsync();

        await _business.IncreaseAchievementProgressFor(userThatCompletedALesson, ObjectiveTarget.Course);

        //Verify that the counter was updated
        int counterAfterUpdate = await _context.UserAchievement
            .Include(a => a.Achievement)
            .Where(a => a.UserId == userThatCompletedALesson)
            .Where(a => a.Achievement.ObjectiveTarget == ObjectiveTarget.Course)
            .Where(a => a.Achievement.ObjectiveGoal > a.Progress)
            .Select(a => a.Progress)
            .FirstOrDefaultAsync();

        Assert.True(counterAfterUpdate > counterBeforeUpdate);
    }


}