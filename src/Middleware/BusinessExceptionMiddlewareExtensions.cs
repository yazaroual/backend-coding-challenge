
namespace BackendApi.Middleware;
public static class BusinessExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseBusinessExceptionHandler(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BusinessExceptionMiddleware>();
    }
}
