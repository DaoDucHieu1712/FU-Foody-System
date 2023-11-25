using AutoMapper;
using FFS.Application.DTOs.Chat;
using FFS.Application.Entities;
using FFS.Application.Hubs;
using FFS.Application.Repositories;
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

		public ChatController(IChatRepository chatRepository, IMessageRepository messageRepositoy, IHubContext<ChatHub> hubChatContext, IMapper mapper)
		{
			_chatRepository = chatRepository;
			_messageRepositoy = messageRepositoy;
			_hubChatContext = hubChatContext;
			_mapper = mapper;
		}

		[HttpGet("{UserId}")]
		public async Task<IActionResult> GetAllByUserId(string UserId)
		{
			try
			{
				var boxs = await _chatRepository
					.FindAll(x => x.ToUserId == UserId || x.FromUserId == UserId ,x => x.ToUser , x => x.FormUser)
					.ToListAsync();
				return Ok(_mapper.Map<List<ChatResponseDTO>>(boxs));
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> FindById(int id)
		{
			try
			{
				var box = await _chatRepository.FindSingle(x => x.Id == id, x => x.Messages, x => x.ToUser, x => x.FormUser);
				var _box = _mapper.Map<ChatResponseDTO>(box);
				return Ok(_box);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateChatBox(ChatRequestDTO chatRequestDTO)
		{
			try
			{
				var check = await _chatRepository.FindSingle(
			    x => x.ToUserId == chatRequestDTO.ToUserId &&
				x.FromUserId == chatRequestDTO.FromUserId ||
				x.ToUserId == chatRequestDTO.FromUserId &&
				x.FromUserId == chatRequestDTO.ToUserId 
				);

				if(check != null)
				{
					await _hubChatContext.Clients.All.SendAsync("FuFoodySendMessage");
					return StatusCode(500, "...");
				}

				await _chatRepository.Add(_mapper.Map<Chat>(chatRequestDTO));
				await _hubChatContext.Clients.All.SendAsync("FuFoodySendMessage");
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> SendMessage(MessageRequestDTO messageRequestDTO)
		{
			try
			{
				await _messageRepositoy.Add(_mapper.Map<Message>(messageRequestDTO));
				await _hubChatContext.Clients.All.SendAsync("FuFoodySendMessage" , "ok");
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

	}
}
