using JsonPlaceHolderWrapperService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JsonPlaceHolderWrapperService.Controllers
{
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
            _logger.LogInformation("Trying to fetch all posts.");
            try
            {
                var posts = await _service.GetPostsAsync();
                _logger.LogInformation($"Successfully retrieved {posts.Count} posts.");
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error occured while fetching posts.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            _logger.LogInformation($"Fetching post with ID: {id.ToString()}");
            try
            {
                var post = await _service.GetPostByIdAsync(id);
                if (post == null)
                {
                    _logger.LogInformation($"An error occured while fetching post with ID: {id.ToString()}.");
                    return NotFound(new { message = $"Post with ID {id.ToString()} not found." });
                }
                _logger.LogInformation($"Successfully retrieved post with ID: {id.ToString()}.");
                return Ok(post);
            }

            catch (Exception ex)
            {
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
                _logger.LogInformation("Successfully fetched comments for post with ID {PostId}.", id);
                return Ok(comments);
            }

            catch (Exception ex)
            {
                _logger.LogInformation("An error occured when trying to retrieve comments for post with ID {PostId}.", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
