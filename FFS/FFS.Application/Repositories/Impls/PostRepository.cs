using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Repositories.Impls
{
    public class PostRepository: EntityRepository<Post, int>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            
        }
        public PagedList<Post> GetListPosts(PostParameters postParameters)
        {
            //var query = FindAll(i => i.StoreId == inventoryParameters.StoreId);

            //// Filter by food name if specified
            //if (!string.IsNullOrEmpty(inventoryParameters.FoodName))
            //{
            //    var foodNameLower = inventoryParameters.FoodName.ToLower();

            //    query = query
            //        .Where(i => i.Food.FoodName.ToLower().Contains(foodNameLower));
            //}

            //// Apply pagination
            //var pagedList = PagedList<Inventory>.ToPagedList(
            //    query.Include(f => f.Food).ThenInclude(c => c.Category).ThenInclude(s => s.Store),
            //    inventoryParameters.PageNumber,
            //    inventoryParameters.PageSize
            //);

            //return pagedList;
            try
            {
                IQueryable<Post> query = FindAll();

                // Apply filtering by title (if provided)
                if (!string.IsNullOrEmpty(postParameters.PostTitle))
                {
                    query = query.Where(p => p.Title.Contains(postParameters.PostTitle));
                }

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

        public async Task<Post> CreatePost(Post post)
        {
            try
            {
                post.CreatedAt = DateTime.Now;
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
                updatedPost.UpdatedAt = DateTime.Now;
                await Update(updatedPost);
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
                return await FindSingle(p => p.Id == postId, x => x.User, x => x.Comments, x=> x.ReactPosts);
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
                await Remove(existingPost);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
