
using BackendApi.Enums;
using BackendApi.ErrorHandling;

namespace BackendApi.Middleware;

public class BusinessExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public BusinessExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<BusinessExceptionMiddleware> logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (BusinessException e)
        {
            //Catch business exceptions as warnings
            int statusCode = GetStatusCode(e);

            logger.LogWarning($"{statusCode} - {e.Error.Code} : {e.Message}\n{e.StackTrace}");

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(e.Error);
        }
        catch (Exception e)
        {
            //Catch any non handled exception as an error
            var error = new BusinessError(ErrorCode.Unknown, "Internal Server Error");

            logger.LogError($"500 - {e.Message}\n{e.StackTrace}");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(error);
        }
    }

    /// <summary>
    /// Translates some codes to their HTTP Status
    /// </summary>
    private static int GetStatusCode(BusinessException e)
    {
        return e.Error.Code switch
        {
            ErrorCode.Unknown => StatusCodes.Status500InternalServerError,
            ErrorCode.Forbidden => StatusCodes.Status403Forbidden,
            ErrorCode.NotFound => StatusCodes.Status404NotFound,
            ErrorCode.FailedDependency => StatusCodes.Status424FailedDependency,
            _ => StatusCodes.Status400BadRequest,
        };
    }
}
