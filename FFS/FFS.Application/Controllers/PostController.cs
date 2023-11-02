using AutoMapper;
using FFS.Application.DTOs.Post;
using FFS.Application.Entities;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostController(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetListPosts()
        {
            try
            {
                var posts = await _postRepository.GetListPosts();
                return Ok(_mapper.Map<List<PostDTO>>(posts));
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
                return Ok(_mapper.Map<PostDTO>(post)); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromBody] CreatePostDTO post)
        {
            try
            {
                var createdPost = await _postRepository.CreatePost(_mapper.Map<Post>(post));
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
    }
}
