using Klippr_Backend.Shared.Domain.Repositories;
using Klippr_Backend.Shared.Infrastructure.Interfaces.ASP.Configuration;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using Klippr_Backend.Favorites.Application.Services;
using Klippr_Backend.Favorites.Domain.Repositories;
using Klippr_Backend.Favorites.Domain.Services;
using Klippr_Backend.Favorites.Infrastructure.Persistence;
using Klippr_Backend.Favorites.Interface.Facade;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options =>
    options.Conventions.Add(new KebabCaseRouteNamingConvention()));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAllPolicy", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 5, 0)))
               .LogTo(Console.WriteLine, LogLevel.Information)
               .EnableSensitiveDataLogging()
               .EnableDetailedErrors();
    else
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(9, 5, 0)))
               .LogTo(Console.WriteLine, LogLevel.Error);
});

builder.Services.AddOpenApi();

// ── Dependency Injection ───────────────────────────────────────────────────
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Favorites
builder.Services.AddScoped<IFavoriteRepository,    FavoriteRepository>();
builder.Services.AddScoped<IFavoriteCommandService, FavoriteCommandService>();
builder.Services.AddScoped<IFavoriteQueryService,   FavoriteQueryService>();
builder.Services.AddScoped<IFavoritesContextFacade, FavoritesContextFacade>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAllPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();