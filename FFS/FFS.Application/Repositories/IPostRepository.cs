using FFS.Application.DTOs.Admin;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories
{
	public interface IPostRepository : IRepository<Post, int>
	{
		PagedList<Post> GetListPosts(PostParameters postParameters);
		Task<List<Post>> GetTop3NewestPosts();
		Task<Post> GetPostByPostId(int postId);
		Task<Post> CreatePost(Post post);
		
		Task<Post> UpdatePost(Post updatedPost);
		Task DeletePost(int postId);
		Task<string> GetUserIdByPostId(int postId);
		int CountAllPost();
		List<PostStatistic> PostStatistics();
	}
}
