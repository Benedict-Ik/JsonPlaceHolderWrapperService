# Json Placeholder Wrapper Service 
In this branch, we deleted the default `WeatherForecastController` class and its model, and proceeded to create `Posts` and `Users` controller classes.

## Added new Controllers
### PostsController
```C#
[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IJsonPlaceholderService _service;

    public PostsController(IJsonPlaceholderService jsonPlaceholderService)
    {
        this._service = jsonPlaceholderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _service.GetPostsAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var post = await _service.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    [HttpGet("{id}/comments")]
    public async Task<IActionResult> GetCommentsForPost(int id)
    {
        var comments = await _service.GetCommentsByPostIdAsync(id);
        return Ok(comments);
    }
}
```

### UsersController
```C#
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IJsonPlaceholderService _service;

    public UsersController(IJsonPlaceholderService jsonPlaceholderService)
    {
        this._service = jsonPlaceholderService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _service.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}
```


## Fixed Typo in Program.cs
Resolved typo by modifying the below line:
```C#
var baseUrl = configuration["BaseUrl"];
```