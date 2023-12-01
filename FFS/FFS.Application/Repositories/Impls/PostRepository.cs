using FFS.Application.Data;
using FFS.Application.DTOs.Admin;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Entities.Enum;
using FFS.Application.Helper;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
	public class PostRepository : EntityRepository<Post, int>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            
        }
        public PagedList<Post> GetListPosts(PostParameters postParameters)
        {
            
            try
            {
                IQueryable<Post> query = FindAll();

                // Apply filtering by title (if provided)
                if (!string.IsNullOrEmpty(postParameters.PostTitle))
                {
                    query = query.ToList().Where(p => CommonService.RemoveDiacritics(p.Title.ToLower()).Contains(CommonService.RemoveDiacritics(postParameters.PostTitle.Trim().ToLower()))).AsQueryable();
                }
				query = query.Where(p => p.Status == StatusPost.Accept);
				// Apply ordering (newest or oldest)
				if (string.Equals(postParameters.OrderBy, "newest", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(p => p.CreatedAt);
                }
                else if (string.Equals(postParameters.OrderBy, "oldest", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(p => p.CreatedAt);
                }
                var pagedList = PagedList<Post>.ToPagedList(
                query.Include(f => f.User).ThenInclude(c => c.Comments).Include(r => r.ReactPosts),
                postParameters.PageNumber,
                postParameters.PageSize);

                return pagedList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Post>> GetTop3NewestPosts()
        {
            return await _context.Posts.Include(x => x.User).Include(x => x.Comments).ThenInclude(x => x.User).Include(x => x.ReactPosts).ThenInclude(x => x.User)
			.OrderByDescending(post => post.Comments.Count)
			.Take(3)
			.ToListAsync();

		}

        public async Task<Post> CreatePost(Post post)
        {
            try
            {
                post.CreatedAt = DateTime.Now;
                post.Status = StatusPost.Pending;
                await Add(post);
                return post;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Post> UpdatePost(Post updatedPost)
        {
            try
            {
                var originalPost = await FindSingle(p => p.Id == updatedPost.Id);
            
                _context.Entry(originalPost).State = EntityState.Detached;

                updatedPost.CreatedAt = originalPost.CreatedAt;

                _context.Entry(updatedPost).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return updatedPost;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Post> GetPostByPostId(int postId)
        {
            try
            {
                var post = await _context.Posts.Include(x=>x.User).Include(x=>x.Comments).ThenInclude(x=>x.User).Include(x=>x.ReactPosts).ThenInclude(x=>x.User).FirstOrDefaultAsync(x=>x.Id == postId);
                return post;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

		


		public async Task DeletePost(int postId)
        {
            try
            {
                var existingPost = await FindById(postId);

                if (existingPost == null)
                {
                    throw new Exception("Bài viết không tồn tại !");
                }
				var comments = _context.Comments.Where(c => c.PostId == postId);
				_context.Comments.RemoveRange(comments);
				var reacts = _context.ReactPosts.Where(c => c.PostId == postId);
				_context.ReactPosts.RemoveRange(reacts);

				await Remove(existingPost);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

		public async Task<string> GetUserIdByPostId(int postId)
		{
			try
			{
				var userId = await _context.Posts
					.Where(post => post.Id == postId)
					.Select(post => post.UserId)
					.FirstOrDefaultAsync();

				return userId;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public int CountAllPost()
		{
			return _context.Posts.Count();	
		}

		public List<PostStatistic> PostStatistics()
		{
			try
			{
				var postStatistic = _context.Posts.GroupBy(user => user.Status).Select(group => new PostStatistic
				{
					PostStatus = group.Key,
					NumberOfPosts = group.Count()
				})
	.ToList();
				return postStatistic;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
