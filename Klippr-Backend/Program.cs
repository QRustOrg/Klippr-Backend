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
using Klippr_Backend.Redemption.Interface.Facade;
using Klippr_Backend.Shared.Infrastructure.EventPublishing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PromotionDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IPromotionCommandService, PromotionCommandService>();
builder.Services.AddScoped<IPromotionQueryService, PromotionQueryService>();
builder.Services.AddScoped<PromotionEventPublisher>();
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddDbContext<RedemptionDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRedemptionRepository, RedemptionRepository>();
builder.Services.AddScoped<IRedemptionCommandService, RedemptionCommandService>();
builder.Services.AddScoped<IRedemptionQueryService, RedemptionQueryService>();
builder.Services.AddScoped<RedemptionContextFacade>();
builder.Services.AddScoped<RedemptionEventPublisher>();

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
    var promotionDbContext = scope.ServiceProvider.GetRequiredService<PromotionDbContext>();
    promotionDbContext.Database.EnsureCreated();
    var redemptionDbContext = scope.ServiceProvider.GetRequiredService<RedemptionDbContext>();
    redemptionDbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
