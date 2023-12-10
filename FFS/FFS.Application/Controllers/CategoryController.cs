using AutoMapper;

using FFS.Application.DTOs.Category;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Entities.Common;
using FFS.Application.Infrastructure.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;

		public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}
		[HttpGet]
		public async Task<IActionResult> ListCategoryByStoreId([FromQuery] CategoryParameters categoryParameters)
		{
			try
			{
				var categories = _categoryRepository.GetCategoriesByStoreId(categoryParameters);
				var entityCatetory = _mapper.Map<List<CategoryDTO>>(categories);
				var metadata = new
				{
					categories.TotalCount,
					categories.PageSize,
					categories.CurrentPage,
					categories.TotalPages,
					categories.HasNext,
					categories.HasPrevious
				};
				return Ok(new
				dataReturnCate {
					entityCatetory = entityCatetory,
					metadata = metadata
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Create(CategoryRequestDTO categoryRequestDTO)
		{
			try
			{
				if(categoryRequestDTO is null)
				{
					return BadRequest();
				}
				else
				{
					if (string.IsNullOrEmpty(categoryRequestDTO.CategoryName))
					{
						return BadRequest(new { message = "CategoryName is required." });
					}
					var check = _categoryRepository.FindSingle(x => x.CategoryName == categoryRequestDTO.CategoryName && x.StoreId == categoryRequestDTO.StoreId);
					if(check is not null)
					{
						return Conflict(new { message = "Danh mục đã tồn tại" });
					}
					await _categoryRepository.Add(_mapper.Map<Category>(categoryRequestDTO));
					return NoContent();
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, CategoryRequestDTO categoryRequestDTO)
		{
			try
			{
				if (categoryRequestDTO is null)
				{
					return BadRequest();
				}
				if (string.IsNullOrEmpty(categoryRequestDTO.CategoryName))
				{
					return BadRequest(new { message = "CategoryName is required." });
				}
				var check = _categoryRepository.FindSingle(x => x.CategoryName == categoryRequestDTO.CategoryName && x.StoreId == categoryRequestDTO.StoreId);
				if (check is not null)
				{
					return Conflict(new { message = "Danh mục đã tồn tại" });
				}
				categoryRequestDTO.Id = id;

				await _categoryRepository.Update(_mapper.Map<Category>(categoryRequestDTO), nameof(BaseEntity<int>.CreatedAt));
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var check = await _categoryRepository.FindById(id, null);
				if(check is null)
				{
					return BadRequest("Danh mục không tồn tại");
				}
				await _categoryRepository.Remove(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> ListTop8PopularCategory()
		{
			try
			{
				var categories = await _categoryRepository.Top8PopularCategories();
				var categoriesDTO = _mapper.Map<List<CategoryPopularDTO>>(categories);
				return Ok(categoriesDTO);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> ExportCategory(int id)
		{
			try
			{
				var data = await _categoryRepository.ExportCategory(id);
				string uniqueFileName = "ThongKe_DanhMuc_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

				return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}

	public class dataReturnCate
	{
		public dynamic entityCatetory { get; set; }
		public dynamic metadata { get; set; }

	}


}
