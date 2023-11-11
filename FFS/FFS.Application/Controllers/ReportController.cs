using AutoMapper;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
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

        public ReportController(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Report([FromBody] ReportDTO storeReportDTO)
        {
            try
            {
                await _reportRepository.CreateReport(_mapper.Map<Report>(storeReportDTO));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("exportreport")]
        public async Task<IActionResult> ExportReport()
        {
            try
            {
                var data = await _reportRepository.ExportReport();
                string uniqueFileName = "ThongKe_BaoCao_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
