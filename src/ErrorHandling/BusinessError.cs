using BackendApi.Enums;

namespace BackendApi.ErrorHandling;

public class BusinessError
{
    public ErrorCode Code { get; set; }
    public string Message { get; set; }
    public string InnerException { get; set; }
    public DateTime OccuredAt { get; set; }

    public BusinessError(ErrorCode code = ErrorCode.Unknown, string message = null, Exception inner = null)
    {
        Code = code;
        Message = message ?? code.ToString();
        InnerException = inner?.ToString();
        OccuredAt = DateTime.UtcNow;
    }

}
