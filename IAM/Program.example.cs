using Infrastructure;
using Infrastructure.Pipeline;
using Interface.Pipeline;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Get configuration
var configuration = builder.Configuration;

// Validate required settings
var jwtSecretKey = configuration["Jwt:SecretKey"] 
    ?? throw new InvalidOperationException("JWT secret key not configured in appsettings.json");

var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured in appsettings.json");

var jwtExpiration = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");

// Add IAM Services
builder.Services.AddIamServices(connectionString, jwtSecretKey, jwtExpiration);

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply IAM migrations
app.Services.ApplyIamMigrations();

// Configure middleware
app.UseMiddleware<AuthorizationMiddleware>();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
