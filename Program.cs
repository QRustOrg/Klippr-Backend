using System.Data;
using System.Data.Common;
using Klippr_Backend.Analytics.Application.Services.CommandServices;
using Klippr_Backend.Analytics.Application.Services.QueryServices;
using Klippr_Backend.Analytics.Domain.Repositories;
using Klippr_Backend.Analytics.Domain.Services;
using Klippr_Backend.Analytics.Infrastructure.Persistence;
using Klippr_Backend.Analytics.Interface.Facade;
using Klippr_Backend.Favorites.Application.Services;
using Klippr_Backend.Favorites.Domain.Repositories;
using Klippr_Backend.Favorites.Domain.Services;
using Klippr_Backend.Favorites.Infrastructure.Persistence;
using Klippr_Backend.Favorites.Interface.Facade;
using Klippr_Backend.IAM.Infrastructure;
using Klippr_Backend.Profile.Infrastructure;
using Klippr_Backend.Profile.Infrastructure.Persistence;
using Klippr_Backend.Promotions.Application.Services;
using Klippr_Backend.Promotions.Domain.Repositories;
using Klippr_Backend.Promotions.Domain.Services;
using Klippr_Backend.Promotions.Infrastructure.EventPublishing;
using Klippr_Backend.Promotions.Infrastructure.Persistence;
using Klippr_Backend.Redemption.Application.Services;
using Klippr_Backend.Redemption.Domain.Repositories;
using Klippr_Backend.Redemption.Domain.Services;
using Klippr_Backend.Redemption.Infrastructure.EventPublishing;
using Klippr_Backend.Redemption.Infrastructure.Persistence;
using Klippr_Backend.Redemption.Infrastructure.Persistence.Repositories;
using Klippr_Backend.Shared.Domain.Repositories;
using Klippr_Backend.Shared.Infrastructure.EventPublishing;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                              ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is required.");
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"]
                   ?? throw new InvalidOperationException("Configuration value 'Jwt:SecretKey' is required.");
var jwtExpirationMinutes = int.TryParse(builder.Configuration["Jwt:ExpirationMinutes"], out var configuredJwtExpirationMinutes)
    ? configuredJwtExpirationMinutes
    : 60;

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());

builder.Services.AddDbContext<AnalyticsDbContext>(options =>
    options.UseSqlite(defaultConnectionString));
builder.Services.AddDbContext<PromotionDbContext>(options =>
    options.UseSqlite(defaultConnectionString));
builder.Services.AddDbContext<RedemptionDbContext>(options =>
    options.UseSqlite(defaultConnectionString));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(defaultConnectionString));

builder.Services.AddIamServices(defaultConnectionString, jwtSecretKey, jwtExpirationMinutes);
builder.Services.AddProfileServices(defaultConnectionString);

builder.Services.AddScoped<IAbuseReportRepository, AbuseReportRepository>();
builder.Services.AddScoped<ICampaignMetricsRepository, CampaignMetricsRepository>();
builder.Services.AddScoped<IAnalyticsQueryService, AnalyticsQueryService>();
builder.Services.AddScoped<IAnalyticsCommandService, AnalyticsCommandService>();
builder.Services.AddScoped<AnalyticsContextFacade>();

builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IPromotionCommandService, PromotionCommandService>();
builder.Services.AddScoped<IPromotionQueryService, PromotionQueryService>();
builder.Services.AddScoped<PromotionEventPublisher>();

builder.Services.AddScoped<IRedemptionRepository, RedemptionRepository>();
builder.Services.AddScoped<IRedemptionCommandService, RedemptionCommandService>();
builder.Services.AddScoped<IRedemptionQueryService, RedemptionQueryService>();
builder.Services.AddScoped<RedemptionEventPublisher>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IFavoriteCommandService, FavoriteCommandService>();
builder.Services.AddScoped<IFavoriteQueryService, FavoriteQueryService>();
builder.Services.AddScoped<IFavoritesContextFacade, FavoritesContextFacade>();

builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Klippr Backend API v1");
    options.RoutePrefix = "swagger";
});

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

using (var scope = app.Services.CreateScope())
{
    EnsureDevelopmentSchema<AnalyticsDbContext>(scope.ServiceProvider);
    EnsureDevelopmentSchema<PromotionDbContext>(scope.ServiceProvider);
    EnsureDevelopmentSchema<RedemptionDbContext>(scope.ServiceProvider);
    EnsureDevelopmentSchema<AppDbContext>(scope.ServiceProvider);
    EnsureDevelopmentSchema<ProfileDbContext>(scope.ServiceProvider);
}
app.Services.ApplyIamMigrations();

app.MapControllers();

app.Run();

static void EnsureDevelopmentSchema<TContext>(IServiceProvider serviceProvider) where TContext : DbContext
{
    var dbContext = serviceProvider.GetRequiredService<TContext>();

    if (dbContext.Database.EnsureCreated() || !dbContext.Database.IsSqlite())
        return;

    var connection = dbContext.Database.GetDbConnection();
    var shouldCloseConnection = connection.State != ConnectionState.Open;

    if (shouldCloseConnection)
        connection.Open();

    try
    {
        foreach (var statement in dbContext.Database
                     .GenerateCreateScript()
                     .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var objectName = GetCreatedSqliteObjectName(statement);

            if (objectName is null || SqliteObjectExists(connection, objectName))
                continue;

            using var command = connection.CreateCommand();
            command.CommandText = statement;
            command.ExecuteNonQuery();
        }
    }
    finally
    {
        if (shouldCloseConnection)
            connection.Close();
    }
}

static string? GetCreatedSqliteObjectName(string statement)
{
    var tokens = statement.Split(' ', 6, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    if (tokens.Length >= 3 &&
        tokens[0].Equals("CREATE", StringComparison.OrdinalIgnoreCase) &&
        tokens[1].Equals("TABLE", StringComparison.OrdinalIgnoreCase))
        return TrimSqliteIdentifier(tokens[2]);

    if (tokens.Length >= 3 &&
        tokens[0].Equals("CREATE", StringComparison.OrdinalIgnoreCase) &&
        tokens[1].Equals("INDEX", StringComparison.OrdinalIgnoreCase))
        return TrimSqliteIdentifier(tokens[2]);

    if (tokens.Length >= 4 &&
        tokens[0].Equals("CREATE", StringComparison.OrdinalIgnoreCase) &&
        tokens[1].Equals("UNIQUE", StringComparison.OrdinalIgnoreCase) &&
        tokens[2].Equals("INDEX", StringComparison.OrdinalIgnoreCase))
        return TrimSqliteIdentifier(tokens[3]);

    return null;
}

static string TrimSqliteIdentifier(string identifier)
{
    return identifier.Trim().Trim('"', '`', '[', ']');
}

static bool SqliteObjectExists(DbConnection connection, string objectName)
{
    using var command = connection.CreateCommand();
    command.CommandText = "SELECT 1 FROM sqlite_master WHERE name = $name LIMIT 1";

    var parameter = command.CreateParameter();
    parameter.ParameterName = "$name";
    parameter.Value = objectName;
    command.Parameters.Add(parameter);

    return command.ExecuteScalar() is not null;
}
