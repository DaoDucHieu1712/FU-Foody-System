using AutoMapper;

using FFS.Application.DTOs.Category;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Entities.Common;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
		private ILoggerManager _logger;

		public CategoryController(ICategoryRepository categoryRepository, IMapper mapper, ILoggerManager logger)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
			_logger = logger;
		}
		[HttpGet]
		public async Task<IActionResult> ListCategoryByStoreId([FromQuery] CategoryParameters categoryParameters)
		{
			try
			{
				_logger.LogInfo($"Retrieving categories by store ID: {categoryParameters.StoreId}");
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
				_logger.LogInfo($"Categories retrieved successfully by store ID: {categoryParameters.StoreId}");
				return Ok(new
				dataReturnCate {
					entityCatetory = entityCatetory,
					metadata = metadata
				});
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving categories by store ID {categoryParameters.StoreId}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpPost]
		public async Task<IActionResult> Create(CategoryRequestDTO categoryRequestDTO)
		{
			try
			{
				_logger.LogInfo("Creating a new category");
				if (categoryRequestDTO is null)
				{
					_logger.LogInfo("Invalid request. CategoryRequestDTO is null");
					return BadRequest();
				}
				else
				{
					if (string.IsNullOrEmpty(categoryRequestDTO.CategoryName))
					{
						_logger.LogInfo("Invalid request. CategoryName is required.");
						return BadRequest(new { message = "CategoryName is required." });
					}
					var check = await _categoryRepository.FindSingle(x => x.CategoryName == categoryRequestDTO.CategoryName && x.StoreId == categoryRequestDTO.StoreId);
					if(check is not null)
					{
						_logger.LogInfo("Category already exists.");
						return Conflict(new { message = "Danh mục đã tồn tại" });
					}
					await _categoryRepository.Add(_mapper.Map<Category>(categoryRequestDTO));
					_logger.LogInfo("Category created successfully");
					return NoContent();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while creating category: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, CategoryRequestDTO categoryRequestDTO)
		{
			try
			{
				_logger.LogInfo($"Updating category with ID: {id}");
				if (categoryRequestDTO is null)
				{
					_logger.LogInfo("Invalid request. CategoryRequestDTO is null");
					return BadRequest();
				}
				if (string.IsNullOrEmpty(categoryRequestDTO.CategoryName))
				{
					_logger.LogInfo("Invalid request. CategoryName is required.");
					return BadRequest(new { message = "CategoryName is required." });
				}
				var check = await _categoryRepository.FindSingle(x => x.CategoryName == categoryRequestDTO.CategoryName && x.StoreId == categoryRequestDTO.StoreId);
				if (check is not null)
				{
					_logger.LogInfo("Category already exists.");
					return Conflict(new { message = "Danh mục đã tồn tại" });
				}
				categoryRequestDTO.Id = id;

				await _categoryRepository.Update(_mapper.Map<Category>(categoryRequestDTO), nameof(BaseEntity<int>.CreatedAt));
				_logger.LogInfo($"Category updated successfully with ID: {id}");
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while updating category with ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize(Roles = "StoreOwner")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				_logger.LogInfo($"Deleting category with ID: {id}");
				var check = await _categoryRepository.FindById(id, null);
				if(check is null)
				{
					_logger.LogInfo("Category does not exist.");
					return BadRequest("Danh mục không tồn tại");
				}
				await _categoryRepository.Remove(id);
				_logger.LogInfo($"Category deleted successfully with ID: {id}");
				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while deleting category with ID {id}: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> ListTop8PopularCategory()
		{
			try
			{
				_logger.LogInfo("Retrieving top 8 popular categories");
				var categories = await _categoryRepository.Top8PopularCategories();
				var categoriesDTO = _mapper.Map<List<CategoryPopularDTO>>(categories);
				_logger.LogInfo("Top 8 popular categories retrieved successfully");
				return Ok(categoriesDTO);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while retrieving top 8 popular categories: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> ExportCategory(int id)
		{
			try
			{
				_logger.LogInfo($"Exporting category with ID: {id}");
				var data = await _categoryRepository.ExportCategory(id);
				string uniqueFileName = "ThongKe_DanhMuc_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
				_logger.LogInfo($"Category exported successfully with ID: {id}");
				return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while exporting category with ID {id}: {ex.Message}");
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
