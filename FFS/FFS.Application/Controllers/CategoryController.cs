using AutoMapper;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _cateRepo;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _cateRepo = categoryRepository;
        }

        [HttpGet("{storeId}")]
        public IActionResult ListCategoryByStoreId(int storeId)
        {
            try
            {
                var categories = _cateRepo.GetList(x => x.StoreId == storeId);
                return Ok(new { data = categories });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
