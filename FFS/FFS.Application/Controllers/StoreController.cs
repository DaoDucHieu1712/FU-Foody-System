using AutoMapper;

using DocumentFormat.OpenXml.Office2010.Excel;

using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FFS.Application.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreController : ControllerBase {

        private readonly IMapper _mapper;
        private readonly IStoreRepository _storeRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IReportRepository _reportRepository;

        public StoreController(IMapper mapper, IStoreRepository storeRepository, IFoodRepository foodRepository, ICommentRepository commentRepository, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _storeRepository = storeRepository;
            _foodRepository = foodRepository;
            _commentRepository = commentRepository;
            _reportRepository = reportRepository;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreInformation(int id)
        {
            try
            {
                StoreInforDTO storeInforDTO = await _storeRepository.GetInformationStore(id);
                return Ok(storeInforDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetStoreByUid(string uId)
        {
            try
            {
                var StoreByUid = await _storeRepository.FindSingle(x => x.UserId == uId);
                return Ok(StoreByUid);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("exportfood")]
        public async Task<IActionResult> ExportFood(int id)
        {
            try
            {
                var data = await _storeRepository.ExportFood(id);
                string uniqueFileName = "ThongKe_MonAn_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("exportinventory")]
        public async Task<IActionResult> ExportInventory(int id)
        {
            try
            {
                var data = await _storeRepository.ExportInventory(id);
                string uniqueFileName = "ThongKe_Kho_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(int id, StoreInforDTO storeInforDTO)
        {
            try
            {
                StoreInforDTO inforDTO = await _storeRepository.UpdateStore(id, storeInforDTO);
                return Ok(inforDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> DetailStore(int id)
        {
            try
            {
                StoreInforDTO storeInforDTO = await _storeRepository.GetDetailStore(id);

                return Ok(storeInforDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{rate}/{id}")]
        [Authorize]
        public async Task<IActionResult> GetCommentByStore(int rate, int id)
        {
            try
            {
                dynamic comment = await _storeRepository.GetCommentByStore(rate, id);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCommentReply(int id)
        {
            try
            {
                dynamic comment = await _storeRepository.GetCommentReply(id);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("{idShop}/{idCategory}")]
        public async Task<IActionResult> GetFoodByCategory(int idShop, int idCategory)
        {
            try
            {
                List<FoodDTO> foodDTOs = await _storeRepository.GetFoodByCategory(idShop, idCategory);
                return Ok(foodDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetFoodByName([FromQuery]string? name)
        {
            try
            {
                List<Food> foods;

                if (string.IsNullOrEmpty(name))
                {
                    // If the name is empty or null, return all items.
                    foods = _foodRepository.FindAll().ToList();
                }
                else
                {
                    // If the name is not empty, perform the search.
                    foods = _foodRepository.FindAll(i => i.FoodName.Contains(name)).ToList();
                }
                List<FoodDTO> foodDTOs = _mapper.Map<List<FoodDTO>>(foods);
                return Ok(foodDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RatingStore([FromBody] StoreRatingDTO storeRatingDTO)
        {
            try
            {
                await _commentRepository.CreateComment(_mapper.Map<Comment>(storeRatingDTO));
                if(storeRatingDTO.ParentCommentId != null)
                {
                    dynamic comment = await _storeRepository.GetCommentReply(Convert.ToInt32(storeRatingDTO.ParentCommentId));
                    return Ok(comment);
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReportStore([FromBody] StoreReportDTO storeReportDTO)
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

    }
}
