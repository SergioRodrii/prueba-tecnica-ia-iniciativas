using System.Text.Json;
using BackendDotnet.Clients;
using BackendDotnet.Data;
using BackendDotnet.Repositories;
using BackendDotnet.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("InitiativesDb");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Falta configurar ConnectionStrings__InitiativesDb.");
}

var fastApiBaseUrl = builder.Configuration["FASTAPI_BASE_URL"] ?? builder.Configuration["FastApi:BaseUrl"];
if (!Uri.TryCreate(fastApiBaseUrl, UriKind.Absolute, out var fastApiUri))
{
    throw new InvalidOperationException("Falta configurar FASTAPI_BASE_URL o FastApi:BaseUrl.");
}

var fastApiTimeoutSeconds = builder.Configuration.GetValue<int?>("FASTAPI_TIMEOUT_SECONDS")
    ?? builder.Configuration.GetValue<int?>("FastApi:TimeoutSeconds")
    ?? throw new InvalidOperationException("Falta configurar FASTAPI_TIMEOUT_SECONDS o FastApi:TimeoutSeconds.");
if (fastApiTimeoutSeconds <= 0)
{
    throw new InvalidOperationException("El timeout de FastAPI debe ser mayor que cero.");
}

var corsAllowedOrigins = builder.Configuration["CORS_ALLOWED_ORIGINS"] ?? builder.Configuration["Cors:AllowedOrigins"] ?? "*";
var origins = corsAllowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (origins.Length == 0 || (origins.Length == 1 && origins[0] == "*"))
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
        }
    });
});

builder.Services.AddDbContext<InitiativesDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<IInitiativeRepository, InitiativeRepository>();
builder.Services.AddScoped<IInitiativeService, InitiativeService>();
builder.Services.AddHttpClient<IAnalysisClient, AnalysisClient>(client =>
{
    client.BaseAddress = fastApiUri;
    client.Timeout = TimeSpan.FromSeconds(fastApiTimeoutSeconds);
});
builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower);

var app = builder.Build();

app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<InitiativesDbContext>();
    await database.Database.EnsureCreatedAsync();
}

app.MapControllers();

app.Run();
