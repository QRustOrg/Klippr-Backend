using Klippr_Backend.Favorites.Application.Services;
using Klippr_Backend.Favorites.Domain.Repositories;
using Klippr_Backend.Favorites.Domain.Services;
using Klippr_Backend.Favorites.Infrastructure.Persistence;
using Klippr_Backend.Favorites.Interface.Configuration;
using Klippr_Backend.Favorites.Interface.Facade;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── MVC ────────────────────────────────────────────────────────────────────
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options =>
    options.Conventions.Add(new KebabCaseRouteNamingConvention())); // own class, not Shared

// ── CORS ───────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
    options.AddPolicy("AllowAllPolicy", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// ── Database ───────────────────────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register FavoritesDbContext — own context, NOT the Shared AppDbContext
builder.Services.AddDbContext<FavoritesDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseMySQL(connectionString)
               .LogTo(Console.WriteLine, LogLevel.Information)
               .EnableSensitiveDataLogging()
               .EnableDetailedErrors();
    else
        options.UseMySQL(connectionString)
               .LogTo(Console.WriteLine, LogLevel.Error);
});

// ── Swagger ────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title   = "Klippr API — Favorites",
        Version = "v1"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In          = ParameterLocation.Header,
        Name        = "Authorization",
        Type        = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme      = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            },
            Array.Empty<string>()
        }
    });
    options.EnableAnnotations();
});

// ── Dependency Injection ───────────────────────────────────────────────────
// Everything is scoped to the Favorites BC — zero references to Shared services

builder.Services.AddScoped<IFavoriteRepository,     FavoriteRepository>();
builder.Services.AddScoped<IFavoriteUnitOfWork,     FavoriteUnitOfWork>();
builder.Services.AddScoped<IFavoriteCommandService, FavoriteCommandService>();
builder.Services.AddScoped<IFavoriteQueryService,   FavoriteQueryService>();
builder.Services.AddScoped<IFavoritesContextFacade, FavoritesContextFacade>();

// ── Pipeline ───────────────────────────────────────────────────────────────
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FavoritesDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
