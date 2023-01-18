namespace BackendApi.Enums;

public enum ErrorCode
{
    Unknown = 0,
    Forbidden = 1,
    NotFound = 2,
    FailedDependency = 3,

    //Lessons
    LessonNotFound = 4,
    LessonTimeMissing = 5,
}