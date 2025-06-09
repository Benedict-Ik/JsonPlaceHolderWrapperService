using JsonPlaceHolderWrapperService.Interfaces;
using JsonPlaceHolderWrapperService.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JsonPlaceHolderWrapperService.Services
{
    public class JsonPlaceholderService : IJsonPlaceholderService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<JsonPlaceholderService> _logger;

        public JsonPlaceholderService(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            this._httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
            this._logger = loggerFactory.CreateLogger<JsonPlaceholderService>();
        }


        public async Task<List<PostDto>> GetPostsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all posts from external API...");

                // Fetch data via external API
                var response = await _httpClient.GetAsync("posts");
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    var message = $"External API returned status code {response.StatusCode} when fetching posts.";
                    _logger.LogWarning(message);
                    throw new HttpRequestException($"External API returned status code {response.StatusCode}");
                }
                var responseInJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<PostDto>>(responseInJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
                _logger.LogInformation("Successfully fetched and deserialized posts.");
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch posts from external API.");
                throw new Exception("Failed to fetch posts from external API.", ex);
            }

            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse posts JSON response.");
                throw new Exception("Failed to parse posts JSON response.", ex);
            }

        }


        public async Task<PostDto?> GetPostByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching post with ID {PostId} from external API...", id);
                var response = await _httpClient.GetAsync($"posts/{id}");
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    var message = $"External API returned status code {response.StatusCode} for post ID {id}.";
                    _logger.LogWarning(message);
                    throw new HttpRequestException($"External API returned status code {response.StatusCode}");
                }
                var responseInJson = await response.Content.ReadAsStringAsync()!;
                var result = JsonSerializer.Deserialize<PostDto?>(responseInJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
                _logger.LogInformation("Successfully fetched and deserialized post with ID {PostId}.", id);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch post with ID {PostId} from external API.", id);
                throw new Exception("Failed to fetch posts from external API.", ex);
            }

            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse post JSON response for ID {PostId}.", id);
                throw new Exception("Failed to parse posts JSON response.", ex);
            }
        }

        public async Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId)
        {
            try
            {
                _logger.LogInformation("Fetching comments for post ID {PostId} from external API...", postId);
                var response = await _httpClient.GetAsync($"posts/{postId}/comments");
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    var message = $"External API returned status code {response.StatusCode} for comments of post ID {postId}.";
                    _logger.LogWarning(message);
                    throw new HttpRequestException($"External API returned status code {response.StatusCode}");
                }
                var responseInJson = await response.Content.ReadAsStringAsync()!;
                var result = JsonSerializer.Deserialize<List<CommentDto?>>(responseInJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
                _logger.LogInformation("Successfully fetched and deserialized comments for post ID {PostId}.", postId);
                return result;
            }

            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch comments for post ID {PostId} from external API.", postId);
                throw new Exception("Failed to fetch posts from external API.", ex);
            }

            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse comments JSON response for post ID {PostId}.", postId);
                throw new Exception("Failed to parse posts JSON response.", ex);
            }
        }


        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user with ID {UserId} from external API...", id);
                var response = await _httpClient.GetAsync($"users/{id}");
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    var message = $"External API returned status code {response.StatusCode} for user ID {id}.";
                    _logger.LogWarning(message);
                    throw new HttpRequestException($"External API returned status code {response.StatusCode}");
                }
                var responseInJson = await response.Content.ReadAsStringAsync()!;
                var result = JsonSerializer.Deserialize<UserDto>(responseInJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                _logger.LogInformation("Successfully fetched and deserialized user with ID {UserId}.", id);
                return result;
            }

            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Failed to fetch user with ID {UserId} from external API.", id);
                throw new Exception("Failed to fetch posts from external API.", ex);
            }

            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse user JSON response for ID {UserId}.", id);
                throw new Exception("Failed to parse posts JSON response.", ex);
            }
        }
    }
}
