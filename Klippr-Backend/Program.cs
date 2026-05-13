using Klippr_Backend.Analytics.Application.Services;
using Klippr_Backend.Analytics.Application.Services.CommandServices;
using Klippr_Backend.Analytics.Application.Services.QueryServices;
using Klippr_Backend.Analytics.Domain.Repositories;
using Klippr_Backend.Analytics.Domain.Services;
using Klippr_Backend.Analytics.Infrastructure.Persistence;
using Klippr_Backend.Analytics.Interface.Facade;
using Klippr_Backend.IAM.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                              ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is required.");
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"]
                   ?? throw new InvalidOperationException("Configuration value 'Jwt:SecretKey' is required.");
var jwtExpirationMinutes = int.TryParse(builder.Configuration["Jwt:ExpirationMinutes"], out var configuredJwtExpirationMinutes)
    ? configuredJwtExpirationMinutes
    : 60;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AnalyticsDbContext>(options =>
    options.UseSqlite(defaultConnectionString));
builder.Services.AddIamServices(defaultConnectionString, jwtSecretKey, jwtExpirationMinutes);
builder.Services.AddScoped<IAbuseReportRepository, AbuseReportRepository>();
builder.Services.AddScoped<ICampaignMetricsRepository, CampaignMetricsRepository>();
builder.Services.AddScoped<IAnalyticsQueryService, AnalyticsQueryService>();
builder.Services.AddScoped<IAnalyticsCommandService, AnalyticsCommandService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Klippr Backend API v1");
        options.RoutePrefix = "swagger";
    });

    using var scope = app.Services.CreateScope();
    var analyticsDbContext = scope.ServiceProvider.GetRequiredService<AnalyticsDbContext>();
    analyticsDbContext.Database.EnsureCreated();
    app.Services.ApplyIamMigrations();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
