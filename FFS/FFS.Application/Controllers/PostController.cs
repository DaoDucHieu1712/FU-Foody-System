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
using Microsoft.AspNetCore.Identity;
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
		private readonly IAuthRepository _authRepository;
		private readonly IHubContext<NotificationHub> _hubContext;
		private readonly INotificationRepository _notifyRepository;
		private readonly ApplicationDbContext _db;
		private readonly ICommentRepository _commentRepository;
		private ILoggerManager _logger;
		public PostController(
			IPostRepository postRepository
			, ApplicationDbContext db
			, IAuthRepository authRepository
			, INotificationRepository notifyRepository
			, IReactPostRepository reactPostRepository
			, IMapper mapper
			, IHubContext<NotificationHub> hubContext
			, ICommentRepository commentRepository,
			  ILoggerManager logger)
		{
			_postRepository = postRepository;
			_reactPostRepository = reactPostRepository;
			_mapper = mapper;
			_hubContext = hubContext;
			_notifyRepository = notifyRepository;
			_authRepository = authRepository;
			_db = db;
			_commentRepository = commentRepository;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult GetListPosts([FromQuery] PostParameters postParameters)
		{
			try
			{
				_logger.LogInfo($"Attempting to get list of posts...");
				var posts = _postRepository.GetListPosts(postParameters);
				_logger.LogInfo($"Successfully retrieved list of posts.");
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
					postDTO.LikeNumber = postDTO.ReactPosts.Where(x => x.IsLike == true).Count();
					postDTO.CommentNumber = postDTO.Comments.Count();
				}

				return Ok(new { entityPost, metadata });
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting list of posts: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}
		[HttpGet("{postId}")]
		public async Task<ActionResult<Post>> GetPostByPostId(int postId)
		{
			try
			{
				_logger.LogInfo($"Attempting to get post with ID {postId}...");
				var post = await _postRepository.GetPostByPostId(postId);

				if (post == null)
				{
					_logger.LogInfo($"Post with ID {postId} not found.");
					return NotFound();
				}
				var postDTO = _mapper.Map<PostDTO>(post);

				postDTO.LikeNumber = postDTO.ReactPosts.Where(x => x.IsLike == true).Count();
				postDTO.CommentNumber = postDTO.Comments.Count();
				_logger.LogInfo($"Successfully retrieved post with ID {postId}.");
				return Ok(postDTO);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting post with ID {postId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{postId}")]
		public async Task<ActionResult<Post>> GetUserByPost(int postId)
		{
			try
			{
				_logger.LogInfo($"Attempting to get user by post with ID {postId}...");
				var post = await _postRepository.GetUserIdByPostId(postId);

				if (post == null)
				{
					_logger.LogInfo($"Post with ID {postId} not found.");
					return NotFound();
				}
				_logger.LogInfo($"Successfully retrieved user by post with ID {postId}.");
				return Ok(post);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting user by post with ID {postId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet()]
		public async Task<ActionResult<List<Post>>> GetTop3NewestPosts()
		{
			try
			{
				_logger.LogInfo("Attempting to get the top 3 newest posts...");
				var top3Posts = await _postRepository.GetTop3NewestPosts();
				var entityPost = _mapper.Map<List<PostDTO>>(top3Posts);
				foreach (var postDTO in entityPost)
				{
					postDTO.LikeNumber = postDTO.ReactPosts.Where(x => x.IsLike == true).Count();
					postDTO.CommentNumber = postDTO.Comments.Count();
				}
				_logger.LogInfo("Successfully retrieved the top 3 newest posts.");
				return Ok(entityPost);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting the top 3 newest posts: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Post>> CreatePost([FromBody] CreatePostDTO post)
		{
			try
			{
				_logger.LogInfo($"Attempting to create a new post for user with ID {post.UserId}...");
				var createdPost = await _postRepository.CreatePost(_mapper.Map<Post>(post));

				var notification = new Notification
				{
					CreatedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
					IsDelete = false,
					UserId = post.UserId,
					Title = "Bài viết mới",
					Content = $"Bài viết {post.Title} đang chờ được phê duyệt!"
				};

				await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
				await _notifyRepository.Add(notification);
				_logger.LogInfo($"Successfully created a new post for user with ID {post.UserId}.");
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while creating a new post: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpPut("{id}")]
		public async Task<ActionResult<Post>> UpdatePost(int id, [FromBody] UpdatePostDTO updatedPost)
		{
			try
			{
				_logger.LogInfo($"Attempting to update post with ID {id}...");
				if (id != updatedPost.Id)
				{
					_logger.LogError($"Invalid request: Provided post ID {id} does not match the ID in the request body.");
					return BadRequest("Bài viết không tồn tại !");
				}

				var result = await _postRepository.UpdatePost(_mapper.Map<Post>(updatedPost));

				if (result == null)
				{
					_logger.LogError($"Post with ID {id} not found.");
					return NotFound();
				}
				_logger.LogInfo($"Successfully updated post with ID {id}.");
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while updating post with ID {id}: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePost(int id)
		{
			try
			{
				_logger.LogInfo($"Attempting to delete post with ID {id}...");
				await _postRepository.DeletePost(id);
				_logger.LogInfo($"Successfully deleted post with ID {id}.");
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while deleting post with ID {id}: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}


		[Authorize]
		[HttpPost]
		public async Task<IActionResult> ReactPost([FromBody] CreateReactPostDTO reactPostDTO)
		{
			try
			{
				_logger.LogInfo($"Attempting to react to post with ID {reactPostDTO.PostId} by user ID {reactPostDTO.UserId}...");
				var reactingUser = await _authRepository.GetUser(reactPostDTO.UserId);

				if (reactingUser == null)
				{
					_logger.LogError($"Reacting user with ID {reactPostDTO.UserId} not found.");
					return BadRequest("Reacting user not found");
				}
				var postAuthor = await _postRepository.GetUserIdByPostId((int)reactPostDTO.PostId);
				var reactPost = await _reactPostRepository.FindSingle(x => x.PostId == reactPostDTO.PostId && x.UserId == reactPostDTO.UserId);
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



					if (postAuthor != null && reactPostDTO.UserId != postAuthor)
					{

						var notification = new Notification
						{
							CreatedAt = DateTime.Now,
							UpdatedAt = DateTime.Now,
							IsDelete = false,
							UserId = postAuthor,
							Title = "Hoạt động mới",
							Content = $"{reactingUser.firstName} {reactingUser.lastName} đã thích bài viết của bạn."
						};

						await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
						await _notifyRepository.Add(notification);
						_logger.LogInfo($"Notification sent to post author with ID {postAuthor} about the reaction.");
					}


				}
				else
				{
					reactPost.IsLike = !reactPost.IsLike;

					_reactPostRepository?.Update(reactPost);
					_logger.LogInfo($"React post updated for post ID {reactPostDTO.PostId} and user ID {reactPostDTO.UserId}.");
				}

				_logger.LogInfo($"Successfully reacted to post with ID {reactPostDTO.PostId} by user ID {reactPostDTO.UserId}.");

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while reacting to post with ID {reactPostDTO.PostId} by user ID {reactPostDTO.UserId}: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> CommentPost(Comment comment)
		{
			try
			{
				_logger.LogInfo($"Attempting to add a new comment for post with ID {comment.PostId}...");
				await _commentRepository.Add(comment);
				_logger.LogInfo($"Comment added successfully for post with ID {comment.PostId}.");
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while adding a comment for post with ID {comment.PostId}: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}
	}
}
