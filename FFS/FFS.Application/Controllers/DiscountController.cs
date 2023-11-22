using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using FFS.Application.DTOs;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
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
        private readonly IMapper _mapper;

        public DiscountController(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public  IActionResult ListDiscoutByStore([FromQuery] DiscountParameters discountParameters)
        {
            try
            {
                var query = _discountRepository.FindAll(x => x.StoreId == discountParameters.StoreId && x.IsDelete == false);
                if (!string.IsNullOrEmpty(discountParameters.CodeName))
                {
                    var codeNameLower = discountParameters.CodeName.ToLower();

                    query = query
                        .Where(i => i.Code.ToLower().Contains(codeNameLower));
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
                return Ok(new {Discounts = discountDtos,
                Metadata = metadata});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateDiscount([FromBody] DiscountDTO discountDTO)
        {
            try
            {
                await _discountRepository.Add(_mapper.Map<Discount>(discountDTO));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDiscount(int id, DiscountDTO discountDTO)
        {
            try
            {
                var discountUpdate = await _discountRepository.FindById(id, null);
                if (discountUpdate == null)
                {
                    return NotFound();
                }
                discountDTO.Id = id;
                discountDTO.StoreId = discountUpdate.StoreId;
                _mapper.Map(discountDTO, discountUpdate);
                await _discountRepository.Update(discountUpdate);
                return Ok("Sửa thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            try
            {
                var discountDelete = await _discountRepository.FindSingle(x => x.Id == id);
                if (discountDelete == null)
                {
                    return NotFound();
                }
                await _discountRepository.Remove(discountDelete);
                return Ok("Xóa thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UseDiscount(string Code)
        {
            try
            {
                var check = await _discountRepository.FindSingle(x => x.Code == Code);
                if (check == null) return Ok(false);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
