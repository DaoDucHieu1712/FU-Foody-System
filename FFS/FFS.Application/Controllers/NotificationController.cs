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

      
     
		private readonly INotificationRepository _notifyRepository;

		public NotificationController(INotificationRepository notifyRepository )
        {
            

			_notifyRepository = notifyRepository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUserId(string userId)
		{
			var notifications = await _notifyRepository.GetNotificationsByUserId(userId);

			return Ok(notifications);
        }


	}
}
