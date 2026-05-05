using Klippr_Backend.Promotions.Application.Services;
using Klippr_Backend.Promotions.Domain.Repositories;
using Klippr_Backend.Promotions.Domain.Services;
using Klippr_Backend.Promotions.Infrastructure.EventPublishing;
using Klippr_Backend.Promotions.Infrastructure.Persistence;
using Klippr_Backend.Promotions.Interface.Facade;
using Klippr_Backend.Shared.Infrastructure.EventPublishing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<PromotionDbContext>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IPromotionCommandService, PromotionCommandService>();
builder.Services.AddScoped<IPromotionQueryService, PromotionQueryService>();
builder.Services.AddScoped<PromotionsContextFacade>();
builder.Services.AddScoped<PromotionEventPublisher>();
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
