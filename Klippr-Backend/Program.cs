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
using Klippr_Backend.Shared.Infrastructure.EventPublishing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());
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
    EnsureContextTablesCreated(promotionDbContext, "Promotions");
    var redemptionDbContext = scope.ServiceProvider.GetRequiredService<RedemptionDbContext>();
    EnsureContextTablesCreated(redemptionDbContext, "redemptions");
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

static void EnsureContextTablesCreated(DbContext dbContext, string tableName)
{
    var databaseCreator = dbContext.GetService<IRelationalDatabaseCreator>();

    if (!databaseCreator.Exists())
        databaseCreator.Create();

    if (TableExists(dbContext, tableName))
        return;

    databaseCreator.CreateTables();
}

static bool TableExists(DbContext dbContext, string tableName)
{
    dbContext.Database.OpenConnection();

    try
    {
        using var command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = "SELECT 1 FROM sqlite_master WHERE type = 'table' AND name = $tableName LIMIT 1";

        var tableNameParameter = command.CreateParameter();
        tableNameParameter.ParameterName = "$tableName";
        tableNameParameter.Value = tableName;
        command.Parameters.Add(tableNameParameter);

        return command.ExecuteScalar() is not null;
    }
    finally
    {
        dbContext.Database.CloseConnection();
    }
}
