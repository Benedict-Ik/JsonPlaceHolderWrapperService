using JsonPlaceHolderWrapperService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JsonPlaceHolderWrapperService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class UsersController : ControllerBase
    {
        private readonly IJsonPlaceholderService _service;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IJsonPlaceholderService jsonPlaceholderService, ILogger<UsersController> logger)
        {
            this._service = jsonPlaceholderService;
            this._logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            _logger.LogInformation("Trying to fetch user with ID: {UserId}.", id);
            try
            {
                var user = await _service.GetUserByIdAsync(id);
                if (user == null)
                {
                    _logger.LogInformation("User with ID: {UserId} was not found.", id);
                    return NotFound(new { message = $"User with ID {id} not found." });
                }
                return Ok(user);
            }

            catch (Exception ex)
            {
                _logger.LogInformation("An error occured when attempting to fetch user with ID: {UserId}.", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
