using AutoMapper;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;

using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository _foodRepo;
        private readonly IComboRepository _comboRepository;
        private readonly IMapper _mapper;

        public FoodController(IFoodRepository foodRepository, IComboRepository comboRepository, IMapper mapper)
        {
            _foodRepo = foodRepository;
            _mapper = mapper;
            _comboRepository = comboRepository;
        }


        [HttpGet()]
        public IActionResult ListFood([FromQuery] FoodParameters foodParameters)
        {
            try
            {
                var foods = _foodRepo.GetFoods(foodParameters);
                var metadata = new
                {
                    foods.TotalCount,
                    foods.PageSize,
                    foods.CurrentPage,
                    foods.TotalPages,
                    foods.HasNext,
                    foods.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                var result = new
                {
                    Data = _mapper.Map<List<FoodDTO>>(foods),
                    Metadata = metadata
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodById(int id)
        {
            try
            {
                var foods = _foodRepo.FindById(id, null);
                return Ok(new { data = foods });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFood(FoodDTO foodDTO)
        {
            try
            {
                var newFood = new Food
                {
                    CategoryId = (int)foodDTO.CategoryId,
                    StoreId = (int)foodDTO.StoreId,
                    FoodName = foodDTO.FoodName,
                    ImageURL = foodDTO.ImageURL,
                    Description = foodDTO.Description,
                    Price = (decimal)foodDTO.Price
                };
                _foodRepo.Add(newFood);
                return Ok(newFood);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFood(int id, FoodDTO foodDTO)
        {
            try
            {
                if (id != foodDTO.Id)
                {
                    return BadRequest();
                }
                var updateFood = _mapper.Map<Food>(foodDTO);
                _foodRepo.Update(updateFood);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            try
            {
                var food = _foodRepo.FindById(id, null);
                if (food == null)
                {
                    return NotFound();
                }
                Food newFood = new Food
                {
                    Id = id,
                    IsDelete=true
                };
                _foodRepo.Update(newFood);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{idStore}")]
        public async Task<IActionResult> GetListCombo(int idStore)
        {
            try
            {
              
               List<Combo> combos =await _comboRepository.GetList(x => x.StoreId == idStore && x.IsDelete == false);
                return Ok(combos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailCombo(int id)
        {
            try
            {
                Combo combo = await _comboRepository.FindById(id, null);
                return Ok(combo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateCombo(ComboFoodDTO comboFoodDTO)
        {
            try
            {
                Combo combo = new Combo()
                {
                    Name = comboFoodDTO.Name,
                    StoreId = comboFoodDTO.StoreId,
                    Percent = comboFoodDTO.Percent,
                };
                _comboRepository.Add(combo);

                _comboRepository.AddComboFood(combo.Id, comboFoodDTO.StoreId, comboFoodDTO.IdFoods);

                return Ok("Tạo thành công Combo!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCombo(int id, ComboFoodDTO comboFoodDTO)
        {
            try
            {

                Combo combo = new Combo()
                {
                    Id = id,
                    Name = comboFoodDTO.Name,
                    StoreId = comboFoodDTO.StoreId,
                    Percent = comboFoodDTO.Percent,
                };
                await _comboRepository.Update(combo);

                _comboRepository.UpdateComboFood(combo.Id, comboFoodDTO.StoreId, comboFoodDTO.IdFoods);

                return Ok("Cập nhật thành công Combo!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteCombo(int id)
        {
            try
            {
                _comboRepository.DeleteCombo(id);
                return Ok("Xóa thành công Combo!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{storeId}")]
        public async Task<IActionResult> GetFoodByStoreId(int storeId)
        {
            try
            {
                var foodList = await _foodRepo.GetFoodListByStoreId(storeId);
                if (foodList == null || foodList.Count == 0)
                {
                    return NotFound();
                }
                return Ok(foodList);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
