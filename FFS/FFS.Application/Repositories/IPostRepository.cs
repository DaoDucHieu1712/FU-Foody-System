using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories
{
    public interface IPostRepository : IRepository<Post, int>
    {
        Task<List<Post>> GetListPosts();
        Task<Post> GetPostByPostId(int postId);
        Task<Post> CreatePost(Post post);
        Task<Post> UpdatePost(Post updatedPost);
        Task DeletePost(int postId);
    }
}
