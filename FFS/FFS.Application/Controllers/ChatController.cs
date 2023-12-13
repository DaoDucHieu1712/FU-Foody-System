using AutoMapper;
using FFS.Application.DTOs.Chat;
using FFS.Application.Entities;
using FFS.Application.Hubs;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ChatController : ControllerBase
	{
		private readonly IChatRepository _chatRepository;
		private readonly IMessageRepository _messageRepositoy;
		private IHubContext<ChatHub> _hubChatContext;
		private readonly IMapper _mapper;
		private ILoggerManager _logger;

		public ChatController(IChatRepository chatRepository, IMessageRepository messageRepositoy, IHubContext<ChatHub> hubChatContext, IMapper mapper, ILoggerManager logger)
		{
			_chatRepository = chatRepository;
			_messageRepositoy = messageRepositoy;
			_hubChatContext = hubChatContext;
			_mapper = mapper;
			_logger = logger;
		}

		[Authorize]
		[HttpGet("{UserId}")]
		public async Task<IActionResult> GetAllByUserId(string UserId)
		{
			_logger.LogInfo($"Retrieving chat boxes for user with ID: {UserId}");
			if (string.IsNullOrEmpty(UserId))
			{
				_logger.LogInfo("Invalid request. UserId is null or empty.");
				return BadRequest("Người dùng không tồn tại");
			}
			try
			{
				var boxs = await _chatRepository
					.FindAll(x => x.ToUserId == UserId || x.FromUserId == UserId,
					x => x.ToUser,
					x => x.FormUser,
					x => x.Messages.OrderByDescending(m => m.CreatedAt))
					.OrderByDescending(x => x.Messages.FirstOrDefault().CreatedAt)
					.ToListAsync();
				if(boxs is null)
				{
					_logger.LogInfo("No chat boxes found for the specified user.");
					return NotFound();
				}
				_logger.LogInfo($"Chat boxes retrieved successfully for user with ID: {UserId}");
				return Ok(_mapper.Map<List<ChatResponseDTO>>(boxs));
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving chat boxes for user with ID {UserId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> FindById(int id)
		{
			try
			{
				_logger.LogInfo($"Retrieving chat box with ID: {id}");
				var box = await _chatRepository.FindSingle(x => x.Id == id, x => x.Messages, x => x.ToUser, x => x.FormUser);
				var _box = _mapper.Map<ChatResponseDTO>(box);
				_logger.LogInfo($"Chat box with ID {id} retrieved successfully.");
				return Ok(_box);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving chat box with ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> CreateChatBox(ChatRequestDTO chatRequestDTO)
		{
			try
			{
				_logger.LogInfo("Creating chat box");
				var check = await _chatRepository.FindSingle(
				x => x.ToUserId == chatRequestDTO.ToUserId &&
				x.FromUserId == chatRequestDTO.FromUserId ||
				x.ToUserId == chatRequestDTO.FromUserId &&
				x.FromUserId == chatRequestDTO.ToUserId
				);

				if (check != null)
				{
					_logger.LogInfo("Chat box already exists");
					await _hubChatContext.Clients.All.SendAsync("FuFoodyCreateBox");
					return StatusCode(500, "...");
				}

				await _chatRepository.Add(_mapper.Map<Chat>(chatRequestDTO));
				await _hubChatContext.Clients.All.SendAsync("FuFoodyCreateBox" , new {
				   ToUser = chatRequestDTO.ToUserId,
				   FromUser = chatRequestDTO.FromUserId
				});
				_logger.LogInfo("Chat box created successfully");
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while creating chat box: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> SendMessage(MessageRequestDTO messageRequestDTO)
		{
			try
			{
				_logger.LogInfo("Sending message");
				await _messageRepositoy.Add(_mapper.Map<Message>(messageRequestDTO));
				await _hubChatContext.Clients.All.SendAsync("FuFoodySendMessage", new
				{
					Ok = "oK1",
					Ok2 = "Ok2"
				});
				_logger.LogInfo("Message sent successfully");
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while sending message: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

	}
}
