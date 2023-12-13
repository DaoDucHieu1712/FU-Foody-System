using AutoMapper;

using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;


using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

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
		private ILoggerManager _logger;

		public FoodController(IFoodRepository foodRepo, IComboRepository comboRepository, ICommentRepository commentRepository, IWishlistRepository wishlistRepository, IOrderRepository orderRepository, IMapper mapper, IInventoryRepository inventoryRepository, ILoggerManager logger)
		{
			_foodRepo = foodRepo;
			_comboRepository = comboRepository;
			_commentRepository = commentRepository;
			_wishlistRepository = wishlistRepository;
			_orderRepository = orderRepository;
			_mapper = mapper;
			_inventoryRepository = inventoryRepository;
			_logger = logger;
		}

		[HttpPost]
		public IActionResult ListFood([FromBody] FoodParameters foodParameters)
		{
			try
			{
				_logger.LogInfo("Retrieving foods");
				var foods = _foodRepo.GetFoods(foodParameters);
				int total = _foodRepo.CountGetFoods(foodParameters);

				var data = new
				{
					data = foods,
					totalPage = total
				};
				_logger.LogInfo("Foods retrieved successfully");
				return Ok(data);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving foods: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetFoodById(int id)
		{
			try
			{
				_logger.LogInfo($"Retrieving food with ID: {id}");
				var food = await _foodRepo.GetFoodById(id);
				if (food == null)
				{
					_logger.LogInfo($"Food with ID {id} not found");
					return NotFound();
				}
				_logger.LogInfo($"Food with ID {id} retrieved successfully");
				return Ok(food);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving food with ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{cateId}")]
		public async Task<IActionResult> GetFoodByCategoryid(int cateId)
		{
			try
			{
				_logger.LogInfo($"Retrieving food items for category with ID: {cateId}");
				var foods = _foodRepo.FindAll(x => x.CategoryId == cateId && x.IsDelete == false);
				_logger.LogInfo($"Food items for category with ID {cateId} retrieved successfully");
				return Ok(foods);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving food items for category with ID {cateId}: {ex.Message}");
				return StatusCode(500, ex.Message);

            }
        }

		[Authorize(Roles = "StoreOwner")]
		[HttpPost]
		public async Task<IActionResult> AddFood(FoodDTO foodDTO)
		{
			try
			{

				_logger.LogInfo("Adding a new food item");
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
				_logger.LogInfo("New food item added successfully");
				return Ok(newFood);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while adding a new food item: {ex.Message}");
				return StatusCode(500, ex.Message);

            }
        }

		[Authorize(Roles = "StoreOwner")]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateFood(int id, FoodDTO foodDTO)
		{
			try
			{

				_logger.LogInfo($"Updating food item with ID: {id}");
				var food = await _foodRepo.FindById(id, null);
				if (food == null)
				{
					_logger.LogInfo($"Food item with ID {id} not found");
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
				_logger.LogInfo($"Food item with ID {id} updated successfully");
				return Ok("Sửa thành công!");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while updating food item with ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);

            }
        }

		[Authorize(Roles = "StoreOwner")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFood(int id)
		{
			try
			{

				_logger.LogInfo($"Deleting food item with ID: {id}");
				var food = await _foodRepo.FindById(id, null);
				if (food == null)
				{
					_logger.LogInfo($"Food item with ID {id} not found");
					return NotFound();
				}
				food.IsDelete = true;
				await _foodRepo.Update(food);
				_logger.LogInfo($"Food item with ID {id} deleted successfully");
				return Ok("Xóa thành công!");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while deleting food item with ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("{idStore}")]
		public async Task<IActionResult> GetListCombo(int idStore)
		{
			try
			{
				_logger.LogInfo($"Retrieving list of combos for store with ID: {idStore}");

				List<dynamic> res = new List<dynamic>();
				List<Combo> combos = await _comboRepository.GetList(x => x.StoreId == idStore && x.IsDelete == false);
				foreach (Combo combo in combos)
				{
					var c = new
					{
						combo = combo,
						detail = await _comboRepository.GetDetail(combo.Id),
					};
					res.Add(c);
				}
				_logger.LogInfo($"List of combos for store with ID {idStore} retrieved successfully");
				return Ok(res);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving list of combos for store with ID {idStore}: {ex.Message}");
				throw new Exception(ex.Message);
			}
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetDetailCombo(int id)
		{
			try
			{
				_logger.LogInfo($"Retrieving details of combo with ID: {id}");
				List<dynamic> combo = await _comboRepository.GetDetail(id);
				_logger.LogInfo($"Details of combo with ID {id} retrieved successfully");
				return Ok(combo);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving details of combo with ID {id}: {ex.Message}");
				throw new Exception(ex.Message);

            }
        }

		[Authorize(Roles = "StoreOwner")]
		[HttpPost]
		public async Task<IActionResult> CreateCombo(ComboFoodDTO comboFoodDTO)
		{
			try
			{

				_logger.LogInfo("Creating a new combo");
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
				_logger.LogInfo("New combo created successfully");
				return Ok("Tạo thành công Combo!");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while creating a new combo: {ex.Message}");
				throw new Exception(ex.Message);

            }
        }

		[Authorize(Roles = "StoreOwner")]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCombo(int id, ComboFoodDTO comboFoodDTO)
		{
			try
			{

				_logger.LogInfo($"Updating combo with ID: {id}");
				Combo combo = new Combo()
				{
					Id = id,
					Name = comboFoodDTO.Name,
					StoreId = comboFoodDTO.StoreId,
					Percent = comboFoodDTO.Percent,
					Image = comboFoodDTO.Image

				};

				await _comboRepository.Update(combo);

				_comboRepository.UpdateComboFood(combo.Id, comboFoodDTO.StoreId, comboFoodDTO.IdFoods);
				_logger.LogInfo($"Combo with ID {id} updated successfully");
				return Ok("Cập nhật thành công Combo!");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while updating combo with ID {id}: {ex.Message}");
				throw new Exception(ex.Message);

            }
        }

		[Authorize(Roles = "StoreOwner")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCombo(int id)
		{
			try
			{

				_logger.LogInfo($"Deleting combo with ID: {id}");
				_comboRepository.DeleteCombo(id);
				_logger.LogInfo($"Combo with ID {id} deleted successfully");
				return Ok("Xóa thành công Combo!");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while deleting combo with ID {id}: {ex.Message}");
				throw new Exception(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteDetail(int id)
		{
			try
			{
				_logger.LogInfo($"Deleting combo with ID: {id}");
				_comboRepository.DeleteDetail(id);
				_logger.LogInfo($"Combo with ID {id} deleted successfully");
				return Ok("Xóa thành công detail Combo!");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while deleting combo with ID {id}: {ex.Message}");
				throw new Exception(ex.Message);
			}
		}




		[HttpGet("{storeId}")]
		public async Task<IActionResult> GetFoodByStoreId(int storeId)
		{
			try
			{
				_logger.LogInfo($"Retrieving food items for store with ID: {storeId}");
				var foodList = await _foodRepo.GetFoodListByStoreId(storeId);
				if (foodList == null || foodList.Count == 0)
				{
					_logger.LogInfo($"No food items found for store with ID: {storeId}");
					return NotFound();
				}
				_logger.LogInfo($"Food items for store with ID {storeId} retrieved successfully");
				return Ok(foodList);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving food items for store with ID {storeId}: {ex.Message}");
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


		[Authorize] 
		[HttpPost]
		public async Task<IActionResult> RatingFood([FromBody] FoodRatingDTO foodRatingDTO)
		{
			try
			{

				_logger.LogInfo("Rating food");
				var wishlistByEmail = await _wishlistRepository.GetList(w => w.UserId == foodRatingDTO.UserId && w.FoodId == foodRatingDTO.FoodId);
				var checkExist = wishlistByEmail.Count() == 0 ? true : false;
				if (foodRatingDTO.Rate == 5 && checkExist == true)
				{
					await _wishlistRepository.AddToWishlist(foodRatingDTO.UserId, foodRatingDTO.FoodId);
				}
				await _commentRepository.RatingFood(_mapper.Map<Comment>(foodRatingDTO));
				_logger.LogInfo("Food rated successfully");
				return Ok(new { IsCanRate = true });
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while rating food: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}
		[HttpGet]
		public IActionResult ListAllFood([FromQuery] AllFoodParameters allFoodParameters)
		{
			try
			{
				_logger.LogInfo("Retrieving all food items");
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
				_logger.LogInfo("All food items retrieved successfully");
				return Ok(
				new
				{
					foodDTOs,
					metadata
				});
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving all food items: {ex.Message}");
				return BadRequest(ex.Message);
			}

		}

		[HttpGet]
		public async Task<IActionResult> GetFoodRecommend()
		{
			try
			{
				_logger.LogInfo("Retrieving recommended food items");
				var homeFood = _foodRepo.FindAll(x => x.IsDelete == false).OrderBy(x => Guid.NewGuid()).Take(10).ToList();
				_logger.LogInfo("Recommended food items retrieved successfully");
				return Ok(_mapper.Map<List<AllFoodDTO>>(homeFood));
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving recommended food items: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CommentFood(Comment comment)
		{
			try
			{
				_logger.LogInfo("Adding comment for food");
				await _commentRepository.Add(comment);
				_logger.LogInfo("Comment added successfully");
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while adding comment for food: {ex.Message}");
				return BadRequest(ex.Message);
			}
		}

	}
}
