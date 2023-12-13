using AutoMapper;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
		private ILoggerManager _logger;

		public ReportController(IReportRepository reportRepository, IMapper mapper, ILoggerManager logger)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
			_logger = logger;
        }

		[Authorize]
        [HttpPost]
        public async Task<IActionResult> Report([FromBody] ReportDTO storeReportDTO)
        {
            try
            {
				_logger.LogInfo($"Attempting to create a new report...");
				await _reportRepository.CreateReport(_mapper.Map<Report>(storeReportDTO));
				_logger.LogInfo($"Report created successfully.");
				return Ok();
            }
            catch (Exception ex)
            {
				_logger.LogError($"An error occurred while creating a report: {ex.Message}");
				return BadRequest(ex.Message);
            }
        }

		
	}
}
