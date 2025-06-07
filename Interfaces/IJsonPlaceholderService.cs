using JsonPlaceHolderWrapperService.Models;

namespace JsonPlaceHolderWrapperService.Interfaces
{
    public interface IJsonPlaceholderService
    {
        Task<List<PostDto>> GetPostsAsync();
        Task<PostDto?> GetPostByIdAsync(int id);
        Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId);
        Task<UserDto?> GetUserByIdAsync(int id);
    }
}
