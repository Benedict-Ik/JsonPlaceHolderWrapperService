# Json Placeholder Wrapper Service 

## Installed the below packages
Added the below NuGet packages:
```sh
dotnet add package Serilog.AspNetCore (8.0.3)
dotnet add package Serilog.Settings.Configuration (8.0.4)
dotnet add package Serilog.Sinks.Console (6.0.0) 
dotnet add package Serilog.Sinks.File (7.0.0)
```


## Updated Program.cs to Use Serilog
Added the below code:
```C#
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Setup Serilog
Log.Logger = new LoggerConfiguration() // The variable "Log" comes from the Serilog static class and is accessible when the Serilog directive 'using Serilog' is used
    .MinimumLevel.Debug() // Or .Information() in production
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Tell ASP.NET Core to use Serilog for logging
builder.Host.UseSerilog();

// Continue with builder.Services...


var app = builder.Build();

// Log startup information
Log.Information("Starting up the app...");


try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly!");
}
finally
{
    Log.CloseAndFlush();
}
```

## Maintained the previous log implementation in controller/service classes
```C#
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "BasicAuthentication")]
public class PostsController : ControllerBase
{
    private readonly IJsonPlaceholderService _service;
    private readonly ILogger<PostsController> _logger;

    public PostsController(IJsonPlaceholderService jsonPlaceholderService, ILogger<PostsController> logger)
    {
        this._service = jsonPlaceholderService;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        _logger.LogInformation("Request to get all posts started.");
        try
        {
            var posts = await _service.GetPostsAsync();
            _logger.LogInformation("Successfully retrieved {PostCount} posts.", posts.Count);
            return Ok(posts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all posts.");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        _logger.LogInformation("Request to get post by ID {PostId} started.", id);
        try
        {
            var post = await _service.GetPostByIdAsync(id);
            if (post == null)
            {
                _logger.LogWarning("Post with ID {PostId} not found.", id);
                return NotFound(new { message = $"Post with ID {id} not found." });
            }

            _logger.LogInformation("Successfully retrieved post with ID {PostId}.", id);
            return Ok(post);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting post with ID {PostId}.", id);
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}/comments")]
    public async Task<IActionResult> GetCommentsForPost(int id)
    {
        _logger.LogInformation("Request to get comments for post ID {PostId} started.", id);
        try
        {
            var comments = await _service.GetCommentsByPostIdAsync(id);
            _logger.LogInformation("Successfully retrieved {CommentCount} comments for post ID {PostId}.", comments.Count, id);
            return Ok(comments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting comments for post ID {PostId}.", id);
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
```