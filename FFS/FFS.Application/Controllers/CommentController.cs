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
		private ILoggerManager _logger;

		public CommentController(ICommentRepository commentRepository, IMapper mapper, IStoreRepository storeRepository, ILoggerManager logger)
		{
			_commentRepository = commentRepository;
			_mapper = mapper;
			_storeRepository = storeRepository;
			_logger = logger;
		}

		[HttpGet("{idStore}")]
		public async Task<IActionResult> GetAllCommentByStore(int idStore)
		{
			try
			{
				_logger.LogInfo($"Retrieving comments for store with ID: {idStore}");
				Store st = await _storeRepository.FindById(idStore, null);
				if (st != null)
				{
					List<Comment> comments = _commentRepository.FindAll(x => x.StoreId == idStore).ToList();
					_logger.LogInfo($"Store with ID {idStore} not found");
					return Ok(comments);
				}
				throw new Exception();
			}
			catch (Exception ex)
			{ 
				_logger.LogError($"An error occurred while retrieving comments for store with ID {idStore}: {ex.Message}");
				return StatusCode(StatusCodes.Status400BadRequest, "Cửa hàng không tồn tại");
			}
		}

		[HttpGet("{idShipper}")]
		public IActionResult GetAllCommentByShipperId(string? idShipper)
		{
			try
			{
				_logger.LogInfo($"Retrieving comments for shipper with ID: {idShipper}");
				if (idShipper == null)
				{
					_logger.LogInfo("Invalid request. Shipper ID is null.");
					return BadRequest("Shipper không tồn tại!");
				}
				List<Comment> comments = _commentRepository.FindAll(x => x.ShipperId == idShipper, u => u.User).ToList();
				_logger.LogInfo($"Comments retrieved successfully for shipper with ID: {idShipper}");
				return Ok(_mapper.Map<List<CommentDTO>>(comments));
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving comments for shipper with ID {idShipper}: {ex.Message}");
				throw new Exception(ex.Message);
			}
		}


	}
}
