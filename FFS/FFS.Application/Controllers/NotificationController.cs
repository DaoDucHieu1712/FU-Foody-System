using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using FFS.Application.Data;
using FFS.Application.Entities;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
		private readonly INotificationRepository _notifyRepository;

		public NotificationController(ApplicationDbContext db, INotificationRepository notifyRepository )
        {
            _db = db;
			_notifyRepository = notifyRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUserId(string userId)
		{
			var notifications = await _notifyRepository.GetNotificationsByUserId(userId);

			return Ok(notifications);
        }

		[HttpPost]
		public IActionResult CreateNotification(Notification notification)
		{
			var createdNotify = _notifyRepository.AddNotification(notification);

			return Ok(createdNotify);
		}
	}
}
