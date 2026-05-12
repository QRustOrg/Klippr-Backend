using Klippr_Backend.Shared.Domain.Repositories;
using Klippr_Backend.Shared.Infrastructure.Interfaces.ASP.Configuration;
using Klippr_Backend.Shared.Infrastructure.Mediator.Cortex.Configuration;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Configuration;
using Klippr_Backend.Shared.Infrastructure.Persistence.EFC.Repositories;
using Klippr_Backend.Community.Application.Internal.CommandServices;
using Klippr_Backend.Community.Application.Internal.QueryServices;
using Klippr_Backend.Community.Domain.Repositories;
using Klippr_Backend.Community.Domain.Services;
using Klippr_Backend.Community.Infrastructure.Persistence.EFC.Repositories;
using Klippr_Backend.Setting.Application.Internal.CommandServices;
using Klippr_Backend.Setting.Application.Internal.QueryServices;
using Klippr_Backend.Setting.Domain.Repositories;
using Klippr_Backend.Setting.Domain.Services;
using Klippr_Backend.Setting.Infrastructure.Persistence.EFC.Repositories;
using Klippr_Backend.Favorites.Application.Services;
using Klippr_Backend.Favorites.Domain.Repositories;
using Klippr_Backend.Favorites.Domain.Services;
using Klippr_Backend.Favorites.Infrastructure.Persistence;
using Klippr_Backend.Favorites.Interface.Facade;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
        options.UseMySQL(connectionString)
               .LogTo(Console.WriteLine, LogLevel.Information)
               .EnableSensitiveDataLogging()
               .EnableDetailedErrors();
    else
        options.UseMySQL(connectionString)
               .LogTo(Console.WriteLine, LogLevel.Error);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Klippr API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, Name = "Authorization",
        Type = SecuritySchemeType.Http, BearerFormat = "JWT", Scheme = "bearer"
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
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Community
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewCommandService, ReviewCommandService>();
builder.Services.AddScoped<IReviewQueryServices, ReviewQueryService>();

// Setting
builder.Services.AddScoped<IPreferenceRepository, PreferenceRepository>();
builder.Services.AddScoped<IPreferenceCommandService, PreferenceCommandService>();
builder.Services.AddScoped<IPreferenceQueryServices, PreferenceQueryService>();

// Favorites
builder.Services.AddScoped<IFavoriteRepository,     FavoriteRepository>();
builder.Services.AddScoped<IFavoriteCommandService,  FavoriteCommandService>();
builder.Services.AddScoped<IFavoriteQueryService,    FavoriteQueryService>();
builder.Services.AddScoped<IFavoritesContextFacade,  FavoritesContextFacade>();

// ── Mediator ───────────────────────────────────────────────────────────────
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddCortexMediator(
    configuration: builder.Configuration,
    handlerAssemblyMarkerTypes: [typeof(Program)],
    configure: options =>
        options.AddOpenCommandPipelineBehavior(typeof(LoggingCommandBehavior<>)));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }

app.UseCors("AllowAllPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
