using AutoMapper;
using DocumentFormat.OpenXml.VariantTypes;
using FFS.Application.Data;
using FFS.Application.DTOs.FlashSale;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.Post;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Migrations;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class FlashSaleController : ControllerBase
	{
		private readonly IFlashSaleRepository _fsRepo;
		private readonly IFlashSaleDetailRepository _fsDetailRepo;
		private readonly IMapper _mapper;
		private readonly ApplicationDbContext _context;

		public FlashSaleController(ApplicationDbContext context, IFlashSaleRepository fsRepo, IFlashSaleDetailRepository fsDetailRepo, IMapper mapper)
		{
			_fsRepo = fsRepo;
			_fsDetailRepo = fsDetailRepo;
			_mapper = mapper;
			_context = context;
		}
		[HttpGet()]
		public IActionResult ListFoodAvailable([FromQuery] CheckFoodFlashSaleParameters parameters)
		{
			try
			{
				var listFoodAvailable = _fsRepo.ListFoodAvailable(parameters);

				var metadata = new
				{
					listFoodAvailable.TotalCount,
					listFoodAvailable.PageSize,
					listFoodAvailable.CurrentPage,
					listFoodAvailable.TotalPages,
					listFoodAvailable.HasNext,
					listFoodAvailable.HasPrevious
				};

				var foodAvailable = _mapper.Map<List<FoodFlashSaleDTO>>(listFoodAvailable);

				return Ok(
				new
				{
					foodAvailable,
					metadata
				});
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateFlashSale([FromBody] FlashSaleDTO flashSaleDTO)
		{
			try
			{
				var flashSaleEntity = _mapper.Map<FlashSale>(flashSaleDTO);
				await _fsRepo.Add(flashSaleEntity);
				if (flashSaleDTO.FlashSaleDetails != null)
				{
					foreach (var detailDTO in flashSaleDTO.FlashSaleDetails)
					{
						detailDTO.FlashSaleId = flashSaleEntity.Id;
						var detailEntity = _mapper.Map<FlashSaleDetail>(detailDTO);
						// Check if the entity is already tracked
						var existingEntity = await _fsDetailRepo.GetFlashSaleDetail(detailEntity.FoodId, detailEntity.FlashSaleId);

						if (existingEntity != null)
						{
							// Update existing entity
							_context.Entry(existingEntity).CurrentValues.SetValues(detailEntity);
						}
						else
						{
							// Add new entity
							await _fsDetailRepo.CreateFlashSaleDetail(detailEntity);
						}
					}
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateFlashSale(int id, FlashSaleDTO flashSaleDTO)
		{
			try
			{
				var flashSaleUpdate = await _fsRepo.FindSingle(x => x.Id == id, x => x.FlashSaleDetails);
				if (flashSaleUpdate == null)
				{
					return NotFound();
				}
				flashSaleDTO.Id = id;
				flashSaleDTO.StoreId = flashSaleUpdate.StoreId;
				_mapper.Map(flashSaleDTO, flashSaleUpdate);
				await _fsRepo.Update(flashSaleUpdate);
				if (flashSaleDTO.FlashSaleDetails != null)
				{
					foreach (var detailDTO in flashSaleDTO.FlashSaleDetails)
					{
						detailDTO.FlashSaleId = flashSaleUpdate.Id;
						var detailEntity = _mapper.Map<FlashSaleDetail>(detailDTO);

						// Check if the entity is already tracked
						var existingEntity = await _fsDetailRepo.GetFlashSaleDetail(detailEntity.FoodId, detailEntity.FlashSaleId);

						if (existingEntity != null)
						{
							// Update existing entity
							_context.Entry(existingEntity).CurrentValues.SetValues(detailEntity);
						}
						else
						{
							// Add new entity
							await _fsDetailRepo.CreateFlashSaleDetail(detailEntity);
						}
					}
				}
				return Ok();

			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteFlashSale(int id)
		{
			try
			{
				var existingFlashSaleEntity = await _fsRepo.FindSingle(x => x.Id == id);

				if (existingFlashSaleEntity == null)
				{
					return NotFound($"Flash sale with ID {id} not found");
				}
				// Delete flash sale
				await _fsRepo.DeleteFlashSale(existingFlashSaleEntity.Id);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{flashSaleId}/{foodId}")]
		public async Task<IActionResult> DeleteFlashSaleDetail(int flashSaleId, int foodId)
		{
			try
			{
				await _fsRepo.DeleteFlashSaleDetail(flashSaleId, foodId);

				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{flashSaleId}")]
		public async Task<IActionResult> FlashSaleDetail(int flashSaleId)
		{
			try
			{
				var flashSaleById = await _fsRepo.FindSingle(x => x.Id == flashSaleId, x => x.FlashSaleDetails);
				if (flashSaleById == null)
				{
					return NotFound();
				}
				var postDTO = _mapper.Map<FlashSaleDTO>(flashSaleById);
				return Ok(postDTO);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpGet("{storeId}")]
		public IActionResult ListFlashSaleByStore(int storeId, [FromQuery] FlashSaleParameter parameter)
		{
			try
			{
				var flashSales = _fsRepo.ListFlashSaleByStore(storeId, parameter);

				var metadata = new
				{
					flashSales.TotalCount,
					flashSales.PageSize,
					flashSales.CurrentPage,
					flashSales.TotalPages,
					flashSales.HasNext,
					flashSales.HasPrevious
				};
				var flashSaleDTOs = _mapper.Map<List<FlashSaleDTO>>(flashSales);
				foreach (var flashSaleDTO in flashSaleDTOs)
				{
					flashSaleDTO.NoOfParticipateFoodSale = flashSaleDTO.FlashSaleDetails
													   ?.Select(detail => detail.FoodId)
													   .Distinct()
													   .Count() ?? 0;
					flashSaleDTO.FlashSaleStatus = GetFlashSaleStatus(flashSaleDTO.Start, flashSaleDTO.End);
				}
				return Ok(
				new
				{
					flashSaleDTOs,
					metadata
				});
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}
		private string GetFlashSaleStatus(DateTime start, DateTime end)
		{
			DateTime oneDayBeforeNow = DateTime.Now.AddDays(-1);

			if (start > DateTime.Now)
			{
				if (start > oneDayBeforeNow)
				{
					return "Sắp diễn ra";
				}
				else
				{
					return "Chưa diễn ra";
				}
			}
			else if (start <= DateTime.Now && end >= DateTime.Now)
			{
				return "Đang diễn ra";
			}
			else
			{
				return "Đã kết thúc";
			}
		}

	}
}
