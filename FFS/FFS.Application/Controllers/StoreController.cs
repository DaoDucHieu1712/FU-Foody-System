using AutoMapper;

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

        public StoreController(IStoreRepository storeRepository, IFoodRepository foodRepository, IMapper mapper)
        {
            _storeRepository = storeRepository;
            _foodRepository = foodRepository;
            _mapper = mapper;
        }

        //[Authorize]
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

        [HttpPost("exportfood")]
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

        [HttpPost("exportinventory")]
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
        public IActionResult GetFoodByName([FromQuery]string name)
        {
            try
            {
                List<Food> foods = _foodRepository.FindAll(i => i.FoodName.Contains(name)).ToList();
                List<FoodDTO> foodDTOs = _mapper.Map<List<FoodDTO>>(foods);
                return Ok(foodDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
