using System.Reflection;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using UsersManager.Application;
using UsersManager.Application.Common.Mappings;
using UsersManager.Persistence;
using UsersManager.WebApi;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("init sqlite logs db");
var logsDbPath = Assembly.GetEntryAssembly()?.Location;
logsDbPath = Path.GetDirectoryName(logsDbPath);
logsDbPath = Path.Combine(logsDbPath ?? throw new InvalidOperationException(), "Logs.db");
SqLiteLogging.InitDb($"Data Source={logsDbPath};Version=3;");

logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);

    // logging
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddCustomAuthenticationAndAuthorization(builder.Configuration);

    builder.Services.AddApplication();
    builder.Services.AddPersistence(builder.Configuration);

    builder.Services.AddAutoMapper(config =>
    {
        config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        config.AddProfile(new AssemblyMappingProfile(typeof(AssemblyMappingProfile).Assembly));
    });

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "Users manager api",
                Version = "v1",
            });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer",
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    var app = builder.Build();

// app.UseHttpsRedirection();

    app.UseMiddleware<ExceptionLoggingMiddleware>();
    app.UseMiddleware<HttpExceptionMiddleware>();

    app.UseAuthentication();
    app.UseAuthorization();

    if (app.Environment.IsDevelopment())
    {
        app.MapGet("", context =>
        {
            context.Response.Redirect("/swagger");
            return Task.CompletedTask;
        });

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
