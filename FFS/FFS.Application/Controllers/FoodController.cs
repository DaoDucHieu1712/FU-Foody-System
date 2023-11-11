using AutoMapper;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
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
        private readonly ICommentRepository _commentRepository;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IMapper _mapper;

        public FoodController(IFoodRepository foodRepo, IComboRepository comboRepository, ICommentRepository commentRepository, IWishlistRepository wishlistRepository, IMapper mapper)
        {
            _foodRepo = foodRepo;
            _comboRepository = comboRepository;
            _commentRepository = commentRepository;
            _wishlistRepository = wishlistRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult ListFood([FromBody] FoodParameters foodParameters)
        {
            try
            {
                var foods = _foodRepo.GetFoods(foodParameters);
               
                return Ok(foods);
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
                var foods = _foodRepo.FindSingle(x => x.Id == id, x => x.Category);
                return Ok(new { data = foods });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("{cateId}")]
        public async Task<IActionResult> GetFoodByCategoryid(int cateId)
        {
            try
            {
                var foods = _foodRepo.FindAll(x => x.CategoryId == cateId && x.IsDelete == false);
                return Ok(foods);
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
                var food = await _foodRepo.FindById(id, null);
                if (food == null)
                {
                    return NotFound();
                }
                food.CategoryId = (int)foodDTO.CategoryId;
                food.Description = foodDTO.Description;
                food.FoodName = foodDTO.FoodName;
                food.ImageURL = foodDTO.ImageURL;
                food.Price = (decimal)foodDTO.Price;

                await _foodRepo.Update(food);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            try
            {
                var food = await _foodRepo.FindById(id, null);
                if (food == null)
                {
                    return NotFound();
                }
                food.IsDelete = true;
                await _foodRepo.Update(food);
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

                List<Combo> combos = await _comboRepository.GetList(x => x.StoreId == idStore && x.IsDelete == false);
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

        [HttpDelete("{id}")]
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

        [HttpPost]
        public async Task<IActionResult> RatingFood([FromBody] FoodRatingDTO foodRatingDTO)
        {
            try
            {
                var wishlistByEmail = await _wishlistRepository.GetList(w => w.UserId == foodRatingDTO.UserId && w.FoodId == foodRatingDTO.FoodId);
                var checkExist = wishlistByEmail.Count() == 0 ? true : false;
                if (foodRatingDTO.Rate == 5 &&  checkExist == true)
                {
                    await _wishlistRepository.AddToWishlist(foodRatingDTO.UserId, foodRatingDTO.FoodId);
                }
                await _commentRepository.CreateComment(_mapper.Map<Comment>(foodRatingDTO));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult ListAllFood([FromQuery] AllFoodParameters allFoodParameters)
        {
            try
            {
                var foods = _foodRepo.GetAllFoods(allFoodParameters);

                var metadata = new
                {
                    foods.TotalCount,
                    foods.PageSize,
                    foods.CurrentPage,
                    foods.TotalPages,
                    foods.HasNext,
                    foods.HasPrevious
                };

                var foodDTOs = _mapper.Map<List<AllFoodDTO>>(foods);

                return Ok(
                new
                {
                    foodDTOs,
                    metadata
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
