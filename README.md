# Json Placeholder Wrapper Service 

## Defining logging in appsettings
```JSON
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    },
    "Console": {
      "LogLevel": {
        "Default": "Debug",
        "System": "Error"
      }
    },
    "File": {
      "Path": "logs/app.log",
      "LogLevel": {
        "Default": "Information"
      }
    }
  }
}
```

## Enabling Logging in Program.cs
```C#
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders(); // Optional: Remove default providers
builder.Logging.AddConsole();     // Logs to console
builder.Logging.AddDebug();       // Logs to Debug output
builder.Logging.AddEventLog();    // Logs to Windows Event Log (Windows only)

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application is starting...");

app.Run();
```


## Using ILogger in a controller class
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