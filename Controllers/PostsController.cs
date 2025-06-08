using JsonPlaceHolderWrapperService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
}
