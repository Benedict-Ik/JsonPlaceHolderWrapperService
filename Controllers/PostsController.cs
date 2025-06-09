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

        public PostsController(IJsonPlaceholderService jsonPlaceholderService)
        {
            this._service = jsonPlaceholderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var posts = await _service.GetPostsAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var post = await _service.GetPostByIdAsync(id);
                if (post == null)
                {
                    return NotFound(new { message = $"Post with ID {id} not found." });
                }
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
            try
            {
                var comments = await _service.GetCommentsByPostIdAsync(id);
                return Ok(comments);
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
