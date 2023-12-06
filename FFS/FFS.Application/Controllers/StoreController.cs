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
		private readonly IComboRepository _comboRepository;
		private readonly IOrderRepository _orderRepository;

		public StoreController(IMapper mapper, IStoreRepository storeRepository, IFoodRepository foodRepository, ICommentRepository commentRepository, IComboRepository comboRepository, IOrderRepository orderRepository)
		{
			_mapper = mapper;
			_storeRepository = storeRepository;
			_foodRepository = foodRepository;
			_commentRepository = commentRepository;
			_comboRepository = comboRepository;
			_orderRepository = orderRepository;
		}

		[HttpGet]
        public IActionResult ListAllStore([FromQuery] AllStoreParameters allStoreParameters)
        {
            try
            {
                var Stores = _storeRepository.GetAllStores(allStoreParameters);

                var metadata = new
                {
                    Stores.TotalCount,
                    Stores.PageSize,
                    Stores.CurrentPage,
                    Stores.TotalPages,
                    Stores.HasNext,
                    Stores.HasPrevious
                };

                var StoreDTOs = _mapper.Map<List<AllStoreDTO>>(Stores);

                return Ok(
                new
                {
                    StoreDTOs,
                    metadata
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> GetTop10Store()
        {
            try
            {
                var top8Store = await _storeRepository.GetTop10PopularStore();
                var top8StoreDTO =  _mapper.Map<List<AllStoreDTO>>(top8Store);
                return Ok(top8StoreDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
        public async Task<IActionResult> DetailStore(int id)
        {
            try
            {
                StoreInforDTO storeInforDTO = await _storeRepository.GetDetailStore(id);
				List<Combo> combos = await _comboRepository.GetList(x => x.StoreId == id && x.IsDelete == false);
				storeInforDTO.Combos = combos;

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
                await _commentRepository.RatingStore(_mapper.Map<Comment>(storeRatingDTO));
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
		[HttpGet("{storeId}")]
		public IActionResult OrderStatistic(int storeId)
		{
			try
			{
				List<OrderStatistic> orderStatistics = _orderRepository.OrderStatistic(storeId);
				return Ok(new
				{
					TotalOrder = _orderRepository.CountTotalOrder(storeId),
					OrdersStatistic = orderStatistics
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}

		}

		[HttpGet("{storeId}")]
		public IActionResult GetFoodDetailStatistics(int storeId)
		{
			try
			{
				List<FoodDetailStatistic> foodDetailStatistics = _orderRepository.FoodDetailStatistics(storeId);
				return Ok(foodDetailStatistics);

			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}

		}

		[HttpGet("{storeId}/{year}")]
		public IActionResult GetRevenuePerMonth(int storeId, int year)
		{
			try
			{
				List<RevenuePerMonth> revenuePerMonths = _orderRepository.RevenuePerMonth(storeId, year);
				return Ok(revenuePerMonths);

			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal Server Error");
			}
		}
	}
}
