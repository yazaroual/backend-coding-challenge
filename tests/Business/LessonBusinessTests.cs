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

[Collection("LessonBusiness")]
public class LessonBusinessTests
{

    private const string InMemoryConnectionString = "DataSource=:memory:";
    private readonly SqliteConnection _connection;
    private readonly BackendApiContext _context;

    //Mocks
    private readonly Mock<ILogger<LessonBusiness>> _loggerStub;
    private readonly Mock<IAchievementBusiness> _achievementStub;

    //Business class for tests
    private readonly LessonBusiness _business;


    public LessonBusinessTests()
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
        _achievementStub = new();

        _business = new(_context, _loggerStub.Object, _achievementStub.Object);

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

        _context.SaveChanges();
    }

    [Fact]
    public async Task SaveProgress_Success()
    {
        SaveProgressDto completedLesson = new()
        {
            UserId = 1,
            LessonId = 1,
            StartedAt = DateTime.UtcNow.AddMinutes(-5),
            CompletedAt = DateTime.UtcNow
        };

        await _business.SaveProgressAsync(completedLesson);

        //Find lesson and check data
        var savedLesson = _context.CompletedLesson
            .Where(u => u.UserId == 1 && u.LessonId == 1)
            .FirstOrDefault();

        Assert.NotNull(savedLesson);
        Assert.Equal(completedLesson.StartedAt, savedLesson.StartedAt);
        Assert.Equal(completedLesson.CompletedAt, savedLesson.CompletedAt);

        //Verify that we updated achievements for lessons
        _achievementStub.Verify(a =>
            a.IncreaseAchievementProgressFor(
                It.Is<int>(u => u == completedLesson.UserId),
                It.Is<ObjectiveTarget>(t => t == ObjectiveTarget.Lesson)),
                Times.Once);
    }

    [Fact]
    public async Task SaveProgress_UserNotFound()
    {
        SaveProgressDto completedLesson = new()
        {
            //User 2 does not exist
            UserId = 2,
            LessonId = 1,
            StartedAt = DateTime.UtcNow.AddMinutes(-5),
            CompletedAt = DateTime.UtcNow
        };

        //Verify that if the user is not found an error is thrown
        var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _business.SaveProgressAsync(completedLesson));

        //With the right error code
        Assert.Equal(ErrorCode.Forbidden, exception.Error.Code);
    }

    [Fact]
    public async Task SaveProgress_LessonNotFound()
    {
        SaveProgressDto completedLesson = new()
        {
            UserId = 1,
            //Lesson 999 does not exist
            LessonId = 999,
            StartedAt = DateTime.UtcNow.AddMinutes(-5),
            CompletedAt = DateTime.UtcNow
        };

        //Verify that if the user is not found an error is thrown
        var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _business.SaveProgressAsync(completedLesson));

        //With the right error code
        Assert.Equal(ErrorCode.LessonNotFound, exception.Error.Code);
    }

    [Fact]
    public async Task SaveProgress_LessonNoTime()
    {
        SaveProgressDto completedLesson = new()
        {
            UserId = 1,
            LessonId = 1,
            StartedAt = new DateTime(),
            CompletedAt = new DateTime()
        };

        //Verify that if the user is not found an error is thrown
        var exception = await Assert.ThrowsAsync<BusinessException>(async () => await _business.SaveProgressAsync(completedLesson));

        //With the right error code
        Assert.Equal(ErrorCode.LessonTimeMissing, exception.Error.Code);
    }

    [Fact]
    public async Task SaveProgress_CompletedChapter()
    {
        SaveProgressDto completedLesson = new()
        {
            UserId = 1,
            LessonId = 1,
            StartedAt = DateTime.UtcNow.AddMinutes(-5),
            CompletedAt = DateTime.UtcNow
        };

        await _business.SaveProgressAsync(completedLesson);

        //Find completed chapters and check data
        bool haveCompletedChapters = _context.CompletedChapter
            .Where(u => u.UserId == 1)
            .Any();

        Assert.True(haveCompletedChapters);

        //Verify that we updated achievements for chapters
        _achievementStub.Verify(a =>
            a.IncreaseAchievementProgressFor(
                It.Is<int>(u => u == completedLesson.UserId),
                It.Is<ObjectiveTarget>(t => t == ObjectiveTarget.Chapter)),
                Times.Once);
    }


    [Fact]
    public async Task SaveProgress_CompletedCourse()
    {
        SaveProgressDto completedLesson = new()
        {
            UserId = 1,
            LessonId = 1,
            StartedAt = DateTime.UtcNow.AddMinutes(-5),
            CompletedAt = DateTime.UtcNow
        };

        await _business.SaveProgressAsync(completedLesson);

        //Find completed chapters and check data
        bool haveCompletedCourse = _context.CompletedCourse
            .Where(u => u.UserId == 1)
            .Any();

        Assert.True(haveCompletedCourse);

        //Verify that we updated achievements for chapters
        _achievementStub.Verify(a =>
            a.IncreaseAchievementProgressFor(
                It.Is<int>(u => u == completedLesson.UserId),
                It.Is<ObjectiveTarget>(t => t == ObjectiveTarget.Course)),
                Times.Once);
    }




}