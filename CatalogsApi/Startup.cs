using CatalogsApi.Context;
using CatalogsApi.Filters;
using CatalogsApi.Middlewares;
using CatalogsApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CatalogsApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
            .AddNewtonsoftJson();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();


        services.AddResponseCaching();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        //services.AddAuthorization();


        services.AddTransient<ActionFilter>();
        // services.AddHostedService<FileLoggerService>();
        services.AddAutoMapper(typeof(Startup));
    }

    public void Configure(WebApplication app, IWebHostEnvironment env,
        ILogger<Startup> serviceLogger)
    {
        // Interrupts the request pipeline
        //app.Run(async context => await context.Response.WriteAsync("Hello World"));

        // Log the response body
        //app.Use(async (context, next) =>
        //{
        //    using var memoryStream = new MemoryStream();

        //    var originalBodyStream = context.Response.Body;
        //    context.Response.Body = memoryStream;

        //    await next.Invoke();

        //    memoryStream.Seek(0, SeekOrigin.Begin);
        //    var response = new StreamReader(memoryStream).ReadToEnd();
        //    memoryStream.Seek(0, SeekOrigin.Begin);

        //    await memoryStream.CopyToAsync(originalBodyStream);
        //    context.Response.Body = originalBodyStream;

        //    serviceLogger.LogInformation(response);
        //});

        //app.UseMiddleware<LogHttpResponseMiddleware>();
        // app.UseLogHttpResponse();

        if (env.IsDevelopment())
        {
        }
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseResponseCaching();

        //app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}