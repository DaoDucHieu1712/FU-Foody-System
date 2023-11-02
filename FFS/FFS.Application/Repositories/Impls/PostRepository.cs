using FFS.Application.Data;
using FFS.Application.Entities;

namespace FFS.Application.Repositories.Impls
{
    public class PostRepository: EntityRepository<Post, int>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            
        }
        public async Task<List<Post>> GetListPosts()
        {
            try
            {
                return await GetList(p => !p.IsDelete, x => x.User, x => x.Comments);
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
                return await FindSingle(p => p.Id == postId, x => x.User, x => x.Comments);
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
