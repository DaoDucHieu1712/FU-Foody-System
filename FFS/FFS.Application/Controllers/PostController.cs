using AutoMapper;
using FFS.Application.Data;
using FFS.Application.DTOs.Post;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Hubs;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Migrations;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IReactPostRepository _reactPostRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ApplicationDbContext _db;

        public PostController(IPostRepository postRepository, ApplicationDbContext db, IReactPostRepository reactPostRepository, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _postRepository = postRepository;
            _reactPostRepository = reactPostRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _db = db;
        }

        [HttpGet]
        public IActionResult GetListPosts([FromQuery] PostParameters postParameters)
        {
            try
            {
                var posts =  _postRepository.GetListPosts(postParameters);
                var metadata = new
                {
                    posts.TotalCount,
                    posts.PageSize,
                    posts.CurrentPage,
                    posts.TotalPages,
                    posts.HasNext,
                    posts.HasPrevious
                };
                var entityPost = _mapper.Map<List<PostDTO>>(posts);
				foreach (var postDTO in entityPost)
                {
                    postDTO.LikeNumber = postDTO.ReactPosts.Where(x=>x.IsLike==true).Count();
                }

                return Ok(new {entityPost, metadata});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{postId}")]
        public async Task<ActionResult<Post>> GetPostByPostId(int postId)
        {
            try
            {
                var post = await _postRepository.GetPostByPostId(postId);

                if (post == null)
                {
                    return NotFound();
                }
                var postDTO = _mapper.Map<PostDTO>(post);
              
                 postDTO.LikeNumber = postDTO.ReactPosts.Where(x => x.IsLike == true).Count();
                postDTO.CommentNumber = postDTO.Comments.Count();
                return Ok(postDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

        [HttpGet()]
        public async Task<ActionResult<List<Post>>> GetTop3NewestPosts()
        {
            try
            {
                var top3Posts = await _postRepository.GetTop3NewestPosts();
                var entityPost = _mapper.Map<List<PostDTO>>(top3Posts);
				foreach (var postDTO in entityPost)
				{
					postDTO.LikeNumber = postDTO.ReactPosts.Where(x => x.IsLike == true).Count();
					postDTO.CommentNumber = postDTO.Comments.Count();
				}
				return Ok(entityPost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
		[Authorize(Roles = $"User")]
		public async Task<ActionResult<Post>> CreatePost([FromBody] CreatePostDTO post)
        {
            try
            {
                var createdPost = await _postRepository.CreatePost(_mapper.Map<Post>(post));

                // Notify clients
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Bài viết {post.Title} đang chờ được phê duyệt");

                // Create and save notification
                var notification = new Notification
                {
                    UserId = post.UserId,
                    Title = "Bài viết mới",
                    Content = $"Bài viết của bạn có tiêu đề: {post.Title} đã được tạo thành công."
                };

                _db.Notifications.Add(notification);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Post>> UpdatePost(int id, [FromBody] UpdatePostDTO updatedPost)
        {
            try
            {
                if (id != updatedPost.Id)
                {
                    return BadRequest("Bài viết không tồn tại !");
                }

                var result = await _postRepository.UpdatePost(_mapper.Map<Post>(updatedPost));

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {

                await _postRepository.DeletePost(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ReactPost([FromBody] CreateReactPostDTO reactPostDTO)
        {
            try
            {
                var reactPost = await _reactPostRepository.FindSingle(x=>x.PostId == reactPostDTO.PostId && x.UserId == reactPostDTO.UserId);
                if (reactPost == null)
                {
                    _reactPostRepository?.Add(new ReactPost
                    {
                        PostId = reactPostDTO.PostId,
                        UserId = reactPostDTO.UserId,
                        IsLike = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDelete = false
                    });
                }
                else {
                    reactPost.IsLike = !reactPost.IsLike;

                    _reactPostRepository?.Update(reactPost);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
