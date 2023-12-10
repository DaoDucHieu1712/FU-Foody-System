using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;

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
using Microsoft.EntityFrameworkCore;
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
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
		private readonly IInventoryRepository _inventoryRepository;

        public FoodController(IFoodRepository foodRepo, IComboRepository comboRepository, ICommentRepository commentRepository, IWishlistRepository wishlistRepository, IOrderRepository orderRepository, IMapper mapper, IInventoryRepository inventoryRepository)
		{
			_foodRepo = foodRepo;
			_comboRepository = comboRepository;
			_commentRepository = commentRepository;
			_wishlistRepository = wishlistRepository;
			_orderRepository = orderRepository;
			_mapper = mapper;
			_inventoryRepository = inventoryRepository;
		}

		[HttpPost]
        public IActionResult ListFood([FromBody] FoodParameters foodParameters)
        {
            try
            {
                var foods = _foodRepo.GetFoods(foodParameters);
                int total = _foodRepo.CountGetFoods(foodParameters);

                var data = new
                {
                    data = foods,
                    totalPage = total
                };

                return Ok(data);
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
                var food = await _foodRepo.GetFoodById(id);
                if (food == null)
                {
                    return NotFound();
                }
                return Ok(food);
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
				await _foodRepo.Add(newFood);

				Inventory inventory = new Inventory()
				{
					FoodId = newFood.Id,
					quantity = 1,
					StoreId = (int)foodDTO.StoreId,
				};

				await _inventoryRepository.Add(inventory);

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

                if (string.IsNullOrEmpty(foodDTO.ImageURL))
                {
                    await _foodRepo.Update(food, "ImageURL");
                }
                else
                {
                    await _foodRepo.Update(food);
                }
                return Ok("Sửa thành công!");
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
                return Ok("Xóa thành công!");
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
				List<dynamic> res = new List<dynamic>();
                List<Combo> combos = await _comboRepository.GetList(x => x.StoreId == idStore && x.IsDelete == false);
				foreach(Combo combo in combos)
				{
					var c = new
					{
						combo = combo,
						detail = await _comboRepository.GetDetail(combo.Id),
					};
					res.Add(c);
				}
                return Ok(res);
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
                List< dynamic> combo = await _comboRepository.GetDetail(id);
                return Ok(combo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCombo(ComboFoodDTO comboFoodDTO)
        {
            try
            {
                Combo combo = new Combo()
                {
                    Name = comboFoodDTO.Name,
                    StoreId = comboFoodDTO.StoreId,
                    Percent = comboFoodDTO.Percent,
					Image = comboFoodDTO.Image,
                };
                await _comboRepository.Add(combo);

				int id = combo.Id;

                await _comboRepository.AddComboFood(combo.Id, comboFoodDTO.StoreId, comboFoodDTO.IdFoods);

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
    //    [HttpGet]
    //    public async Task<IActionResult> IsCanRate(string Uid, int foodId)
    //    {
    //        try
    //        {
    //            var order = await _orderRepository.FindSingle(x =>
    //x.CustomerId.Equals(Uid) &&
    //x.OrderDetails.Any(od => od.FoodId.Equals(foodId)));
    //            bool isCanRate = order != null ? true : false;
    //            return Ok(isCanRate);
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(ex.Message);
    //        }
    //    }

        [HttpPost]
        public async Task<IActionResult> RatingFood([FromBody] FoodRatingDTO foodRatingDTO)
        {
            try
            {
                var wishlistByEmail = await _wishlistRepository.GetList(w => w.UserId == foodRatingDTO.UserId && w.FoodId == foodRatingDTO.FoodId);
                var checkExist = wishlistByEmail.Count() == 0 ? true : false;
                if (foodRatingDTO.Rate == 5 && checkExist == true)
                {
                    await _wishlistRepository.AddToWishlist(foodRatingDTO.UserId, foodRatingDTO.FoodId);
                }
                await _commentRepository.RatingFood(_mapper.Map<Comment>(foodRatingDTO));
                return Ok(new { IsCanRate = true });
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

		[HttpGet]
		public async Task<IActionResult> GetFoodRecommend()
		{
			try
			{
				var homeFood = _foodRepo.FindAll(x=>x.IsDelete==false).OrderBy(x => Guid.NewGuid()).Take(10).ToList();
				return Ok(_mapper.Map<List<AllFoodDTO>>(homeFood));
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CommentFood(Comment comment)
		{
			try
			{
				await _commentRepository.Add(comment);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

	}
}
