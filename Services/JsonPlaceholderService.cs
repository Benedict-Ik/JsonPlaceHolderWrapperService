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
            // Fetch data via external API
            var response = await _httpClient.GetAsync("posts");
            response.EnsureSuccessStatusCode();
            var responseInJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<PostDto>>(responseInJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
            return result;
        }


        public async Task<PostDto?> GetPostByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"posts/{id}");
            response.EnsureSuccessStatusCode();
            var responseInJson = await response.Content.ReadAsStringAsync()!;
            var result = JsonSerializer.Deserialize<PostDto?>(responseInJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
            return result;
        }

        public async Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId)
        {
            var response = await _httpClient.GetAsync($"posts/{postId}/comments");
            response.EnsureSuccessStatusCode();
            var responseInJson = await response.Content.ReadAsStringAsync()!;
            var result = JsonSerializer.Deserialize<List<CommentDto?>>(responseInJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
            return result;
        }
     

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"users/{id}");
            response.EnsureSuccessStatusCode();
            var responseInJson = await response.Content.ReadAsStringAsync()!;
            var result = JsonSerializer.Deserialize<UserDto>(responseInJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return result;
        }
    }
}
