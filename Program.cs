using JsonPlaceHolderWrapperService.Helpers;
using JsonPlaceHolderWrapperService.Interfaces;
using JsonPlaceHolderWrapperService.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

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


//builder.Services.AddHttpClient("JsonPlaceholder", client =>
//{
//    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
//    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
//});

var baseUrl = configuration["BaseUrl"];

builder.Services.AddHttpClient("JsonPlaceholder", client =>
{
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});


// Registering custom services in DI Container
builder.Services.AddScoped<IJsonPlaceholderService, JsonPlaceholderService>();

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Configuration.AddUserSecrets<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();