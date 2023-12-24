using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Migrations;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationRepository _notifyRepository;
		private ILoggerManager _logger;


		public NotificationController(INotificationRepository notifyRepository, ILoggerManager logger)
		{
			_notifyRepository = notifyRepository;
			_logger = logger;
		}

		[HttpGet("{userId}")]
		public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUserId(string userId)
		{
			_logger.LogInfo($"Getting notifications for user ID: {userId}");
			var notifications = await _notifyRepository.GetNotificationsByUserId(userId);
			_logger.LogInfo($"Notifications retrieved successfully for user ID: {userId}");
			return Ok(notifications);
		}


		[HttpPost("{userId}")]
		public async Task<IActionResult> MarkAllAsRead(string userId)
		{
			try
			{
				var unreadNotifications = await _notifyRepository.FindAll(
						   notification => notification.UserId == userId && !notification.IsRead
					   ).ToListAsync();

				if (unreadNotifications.Any())
				{
					foreach (var notification in unreadNotifications)
					{
						notification.IsRead = true;
						await _notifyRepository.Update(notification);
					}

					_logger.LogInfo($"Marked {unreadNotifications.Count} notifications as read for user ID: {userId}");
				}

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while marking all notifications as read: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

	}
}
