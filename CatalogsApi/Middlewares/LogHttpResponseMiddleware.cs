namespace CatalogsApi.Middlewares
{
    public class LogHttpResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogHttpResponseMiddleware> _logger;

        public LogHttpResponseMiddleware(RequestDelegate next, ILogger<LogHttpResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using var memoryStream = new MemoryStream();

            var originalBodyStream = context.Response.Body;
            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            var response = new StreamReader(memoryStream).ReadToEnd();
            memoryStream.Seek(0, SeekOrigin.Begin);

            await memoryStream.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;

            _logger.LogInformation(response);
        }
    }

    public static class LogHttpResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogHttpResponse(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LogHttpResponseMiddleware>();
        }
    }
}
