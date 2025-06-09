using JsonPlaceHolderWrapperService.Interfaces;
using JsonPlaceHolderWrapperService.Models;
using System.Text.Json;

namespace JsonPlaceHolderWrapperService.Services
{
    public class JsonPlaceholderService : IJsonPlaceholderService
    {
        private readonly HttpClient _httpClient;

        public JsonPlaceholderService(IHttpClientFactory httpClientFactory)
        {
            this._httpClient = httpClientFactory.CreateClient("JsonPlaceholder");
        }


        public async Task<List<PostDto>> GetPostsAsync()
        {
            try
            {
                // Fetch data via external API
                var response = await _httpClient.GetAsync("posts");
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"External API returned status code {response.StatusCode}");
                }
                var responseInJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<PostDto>>(responseInJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to fetch posts from external API.", ex);
            }

            catch (JsonException ex)
            {
                throw new Exception("Failed to parse posts JSON response.", ex);
            }

        }


        public async Task<PostDto?> GetPostByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"posts/{id}");
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"External API returned status code {response.StatusCode}");
                }
                var responseInJson = await response.Content.ReadAsStringAsync()!;
                var result = JsonSerializer.Deserialize<PostDto?>(responseInJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to fetch posts from external API.", ex);
            }

            catch (JsonException ex)
            {
                throw new Exception("Failed to parse posts JSON response.", ex);
            }
        }

        public async Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"posts/{postId}/comments");
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"External API returned status code {response.StatusCode}");
                }
                var responseInJson = await response.Content.ReadAsStringAsync()!;
                var result = JsonSerializer.Deserialize<List<CommentDto?>>(responseInJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
                return result;
            }

            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to fetch posts from external API.", ex);
            }

            catch (JsonException ex)
            {
                throw new Exception("Failed to parse posts JSON response.", ex);
            }
        }


        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"users/{id}");
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"External API returned status code {response.StatusCode}");
                }
                var responseInJson = await response.Content.ReadAsStringAsync()!;
                var result = JsonSerializer.Deserialize<UserDto>(responseInJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }

            catch (HttpRequestException ex)
            {
                throw new Exception("Failed to fetch posts from external API.", ex);
            }

            catch (JsonException ex)
            {
                throw new Exception("Failed to parse posts JSON response.", ex);
            }
        }
    }
}
