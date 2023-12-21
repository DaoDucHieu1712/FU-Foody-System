using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using FFS.Application.DTOs;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Helper;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
		private readonly IUserDiscountRepository _userDiscountRepository;
        private readonly IMapper _mapper;
		private ILoggerManager _logger;

		public DiscountController(IDiscountRepository discountRepository, IUserDiscountRepository userDiscountRepository, IMapper mapper, ILoggerManager logger)
		{
			_discountRepository = discountRepository;
			_userDiscountRepository = userDiscountRepository;
			_mapper = mapper;
			_logger = logger;
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpGet]
        public  IActionResult ListDiscoutByStore([FromQuery] DiscountParameters discountParameters)
        {
            try
            {
				_logger.LogInfo("Retrieving discounts for store");
				var query = _discountRepository.FindAll(x => x.StoreId == discountParameters.StoreId && x.IsDelete == false);
                if (!string.IsNullOrEmpty(discountParameters.CodeName))
                {
                    var codeNameLower = discountParameters.CodeName.ToLower();

                    query = query.ToList()
                        .Where(i => CommonService.RemoveDiacritics(i.Code.ToLower()).Contains(CommonService.RemoveDiacritics(codeNameLower))).AsQueryable();
                }
                var discounts = PagedList<Discount>.ToPagedList(
                query,
                discountParameters.PageNumber,
                discountParameters.PageSize
            );
                var metadata = new
                {
                    discounts.TotalCount,
                    discounts.PageSize,
                    discounts.CurrentPage,
                    discounts.TotalPages,
                    discounts.HasNext,
                    discounts.HasPrevious
                };
                var discountDtos = _mapper.Map<List<DiscountDTO>>(discounts);
				_logger.LogInfo("Discounts retrieved successfully for store");
				return Ok(new {Discounts = discountDtos,
                Metadata = metadata});
            }
            catch (Exception ex)
            {
				_logger.LogError($"An error occurred while retrieving discounts for store: {ex.Message}");
				throw new Exception(ex.Message);
            }

        }

		[Authorize(Roles = "StoreOwner")]
		[HttpPost]
        public async Task<IActionResult> CreateDiscount([FromBody] DiscountDTO discountDTO)
        {
            try
            {
				var discountExist = await _discountRepository.FindSingle(x=>x.Code == discountDTO.Code);
				if(discountExist != null) {
					return BadRequest($"Mã giảm giá {discountExist.Code} đã tồn tại. Vui lòng tạo mã khác!");
				}
                await _discountRepository.Add(_mapper.Map<Discount>(discountDTO));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


		[Authorize(Roles = "StoreOwner")]
		[HttpPut]
		public async Task<IActionResult> UpdateDiscount(int id, DiscountDTO discountDTO)
		{
			try
			{
				_logger.LogInfo("Update new discount");
				var discountUpdate = await _discountRepository.FindById(id, null);
				if (discountUpdate == null)
				{
					return NotFound();
				}
				var discountNameExist = await _discountRepository.FindSingle(x => x.Code == discountDTO.Code);
				discountDTO.Id = id;
				discountDTO.StoreId = discountUpdate.StoreId;
				_mapper.Map(discountDTO, discountUpdate);
				await _discountRepository.Update(discountUpdate);
				return Ok("Sửa thành công");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while creating discount: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteDiscount(int id)
		{
			try
			{
				_logger.LogInfo($"Deleting discount with ID: {id}");
				var discountDelete = await _discountRepository.FindSingle(x => x.Id == id);
				if (discountDelete == null)
				{
					_logger.LogInfo($"Discount with ID {id} not found");
					return NotFound();
				}
				await _discountRepository.Remove(discountDelete);
				_logger.LogInfo($"Discount with ID {id} deleted successfully");
				return Ok("Xóa thành công");
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while deleting discount with ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

        [HttpPost]
        public async Task<IActionResult> CheckDiscount(string Code, string UserId, decimal TotalPrice , int[] storeIds)
        {
            try
            {
                var discount = await _discountRepository
					.FindSingle(x => x.Code == Code);

				if(discount == null) return StatusCode(500, new
				{
					msg = "Mã giảm giá không tồn tại !!!",
					IsUse = false,
				});

				var checkstore = false;
				for (int i = 0; i < storeIds.Length; i++)
				{
				   if(discount.StoreId == storeIds[i])
					{
						checkstore = true;
					}
				}

				if (!checkstore) return StatusCode(500, new
				{
					msg = "Mã giảm giá không hợp lệ !!!",
					IsUse = false,
				});

				if (discount.Quantity <= 0) return StatusCode(500, new
				{
					msg = "Đã hết số lượng mã !!",
					IsUse = false,
				});

				if (discount.ConditionPrice > TotalPrice) return StatusCode(500, new
				{
					msg = "Đơn hàng không đủ điều kiện để sử dụng mã !!",
					IsUse = false,
				});

				if (discount.Expired < DateTime.Now) return StatusCode(500, new
				{
					msg = "Mã giảm giá đã hết hạn !!",
					IsUse = false,
				});

				var check = await _userDiscountRepository
					.FindSingle(x => x.DiscountId == discount.Id && x.UserId == UserId , x => x.Discount);

				if(check != null) return StatusCode(500, new
				{
					msg = "Bạn đã sử dụng mã này rồi !!!",
					IsUse = false,
				});

				discount.Quantity = discount.Quantity - 1;
				await _discountRepository.Update(discount);

				await _userDiscountRepository.Add(new UserDiscount
				{
					DiscountId = discount.Id,
					UserId = UserId
				});

				return Ok(new
				{
					msg = "Mã hợp lệ <3 ",
					discount = (decimal)discount.Percent / 100,
					storeId  = discount.StoreId,
					IsUse = true,
				});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

		[HttpGet]
		public async Task<IActionResult> UseDiscount(string Code, string UserId)
		{
			try
			{
				_logger.LogInfo($"Applying discount with code: {Code} for user with ID: {UserId}");
				var discount = await _discountRepository
					.FindSingle(x => x.Code == Code);

				discount.Quantity = discount.Quantity - 1;
				await _discountRepository.Update(discount);

				await _userDiscountRepository.Add(new UserDiscount
				{
					DiscountId = discount.Id,
					UserId = UserId
				});
				_logger.LogInfo($"Discount with code {Code} applied successfully for user with ID {UserId}");
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while applying discount with code {Code} for user with ID {UserId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}
    }
}
