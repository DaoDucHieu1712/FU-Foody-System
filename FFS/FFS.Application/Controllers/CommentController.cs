using AutoMapper;
using FFS.Application.DTOs.Comment;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CommentController : ControllerBase
	{
		private readonly ICommentRepository _commentRepository;
		private readonly IStoreRepository _storeRepository;

		private readonly IMapper _mapper;

		public CommentController(ICommentRepository commentRepository, IMapper mapper, IStoreRepository storeRepository)
		{
			_commentRepository = commentRepository;
			_mapper = mapper;
			_storeRepository = storeRepository;
		}

		[HttpGet("{idStore}")]
		public async Task<IActionResult> GetAllCommentByStore(int idStore)
		{
			try
			{
				Store st = await _storeRepository.FindById(idStore, null);
				if (st != null)
				{
					List<Comment> comments = _commentRepository.FindAll(x => x.StoreId == idStore).ToList();
					return Ok(comments);
				}
				throw new Exception();
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status400BadRequest, "Cửa hàng không tồn tại");
			}
		}

		[HttpGet("{idShipper}")]
		public IActionResult GetAllCommentByShipperId(string? idShipper)
		{
			try
			{
				if(idShipper == null)
				{
					return BadRequest("Shipper không tồn tại!");
				}
				List<Comment> comments = _commentRepository.FindAll(x => x.ShipperId == idShipper, u => u.User).ToList();

				return Ok(_mapper.Map<List<CommentDTO>>(comments));
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


	}
}
