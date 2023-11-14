using AutoMapper;
using FFS.Application.DTOs.Comment;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet("{idStore}")]
        public IActionResult GetAllCommentByStore(int idStore)
        {
            try
            {
                List<Comment> comments = _commentRepository.FindAll(x => x.StoreId == idStore).ToList();
                return Ok(comments);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{idShipper}")]
        public IActionResult GetAllCommentByShipperId(string? idShipper)
        {
            try
            {
                List<Comment> comments = _commentRepository.FindAll(x => x.ShipperId == idShipper,  u=>u.User).ToList();
                
                return Ok(_mapper.Map<List<CommentDTO>>(comments));
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


    }
}
