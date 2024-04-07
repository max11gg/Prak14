using ApiTrello.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using System.Configuration;
using System.Text;

using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

var key = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(32));

logger.Info("Application started");
try
{
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    builder.Host.UseNLog();

    Console.WriteLine($"Generated JWT Key: {key}");

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSingleton<JwtTokenService>();

    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ApiTrello.Models.TrelloContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("con")));

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                // ”становите другие параметры валидации здесь
            };
        });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });
    var app = builder.Build();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("AllowAll");

    //app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    //metrics
    app.MapMetrics();
    app.UseMetricServer();
    app.UsePrometheusHttpMetrics(); 

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Found problem!");
}
finally
{
    logger.Info("Application stopping");
    logger.Warn("Application is shutting down.");
    LogManager.Shutdown();
}