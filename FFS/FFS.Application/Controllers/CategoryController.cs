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
                {
                    entityCatetory,
                    metadata
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
                await _categoryRepository.Add(_mapper.Map<Category>(categoryRequestDTO));
                return NoContent();
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
                await _categoryRepository.Remove(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListTop5PopularCategory()
        {
            try
            {
                var categories = await _categoryRepository.Top5PopularCategories();
                var categoriesDTO = _mapper.Map<List<CategoryPopularDTO>>(categories);
                return Ok(categoriesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
