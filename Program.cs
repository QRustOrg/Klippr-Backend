using System.Text;
using System.Threading.RateLimiting;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                              ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is required.");
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"]
                   ?? throw new InvalidOperationException("Configuration value 'Jwt:SecretKey' is required.");
var jwtExpirationMinutes = int.TryParse(builder.Configuration["Jwt:ExpirationMinutes"], out var configuredJwtExpirationMinutes)
    ? configuredJwtExpirationMinutes
    : 60;
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "klippr-iam";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "klippr-api";

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Klippr Backend API", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Klippr Backend API", Version = "v2" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization. Pega solo el token (sin 'Bearer ')."
    });
    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", doc), new List<string>() }
    });
});

builder.Services.AddDbContext<AnalyticsDbContext>(options =>
    options.UseMySQL(defaultConnectionString));
builder.Services.AddDbContext<PromotionDbContext>(options =>
    options.UseMySQL(defaultConnectionString));
builder.Services.AddDbContext<RedemptionDbContext>(options =>
    options.UseMySQL(defaultConnectionString));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(defaultConnectionString));

builder.Services.AddIamServices(defaultConnectionString, jwtSecretKey, jwtExpirationMinutes, jwtIssuer, jwtAudience);
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    services.GetRequiredService<AnalyticsDbContext>().Database.Migrate();
    services.GetRequiredService<PromotionDbContext>().Database.Migrate();
    services.GetRequiredService<RedemptionDbContext>().Database.Migrate();
    services.GetRequiredService<AppDbContext>().Database.Migrate();
    services.GetRequiredService<ProfileDbContext>().Database.Migrate();
}
app.Services.ApplyIamMigrations();
await Klippr_Backend.IAM.Infrastructure.IamSeeder.SeedAdminAsync(app.Services, builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Klippr Backend API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Klippr Backend API v2");
        options.RoutePrefix = "swagger";
    });
    app.MapOpenApi();
}

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
