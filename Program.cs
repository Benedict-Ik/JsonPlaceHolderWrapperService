using JsonPlaceHolderWrapperService.Helpers;
using JsonPlaceHolderWrapperService.Interfaces;
using JsonPlaceHolderWrapperService.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Enabling Logging
builder.Logging.ClearProviders(); // Optional: Remove default providers
builder.Logging.AddConsole();     // Logs to console
builder.Logging.AddDebug();       // Logs to Debug output
builder.Logging.AddEventLog();    // Logs to Windows Event Log (Windows only)

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JSON Placeholder Wrapper Service", Version = "v1" });


    // Add Security Scheme
    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Enter your username and password for authentication.",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                }
            },
            new string[] {} // No predefined scopes, allows all authenticated users
        }
    });
});

var baseUrl = configuration["BaseUrl"];

builder.Services.AddHttpClient("JsonPlaceholder", client =>
{
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});


// Registering custom services in DI Container
builder.Services.AddScoped<IJsonPlaceholderService, JsonPlaceholderService>();

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options =>
    {
        options.ForwardChallenge = "BasicAuthenticationFailure";
        options.ForwardForbid = "BasicAuthenticationFailure";
    });


builder.Configuration.AddUserSecrets<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application is starting...");

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();