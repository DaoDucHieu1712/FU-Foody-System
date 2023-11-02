using AutoMapper;
using FFS.Application.Data;
using FFS.Application.DTOs;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locaRepo;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public LocationController(ILocationRepository locaRepo, IMapper mapper, ApplicationDbContext context)
        {
            _locaRepo = locaRepo;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Location>>> ListLocation(string email)
        {
            try
            {
                var locations = await _locaRepo.GetList(x => x.User.Email == email && x.IsDelete == false, x => x.User);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Location>> AddLocation([FromBody] LocationDTO locationDTO)
        {
            try
            {
                var locationEntity = _mapper.Map<Location>(locationDTO);
                var locations = _locaRepo.FindAll(x=>x.User.Email == locationDTO.Email);
                await _locaRepo.Add(locationEntity);
                return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLocation(int id, LocationDTO locationDTO)
        {
            try
            {
                var locationUpdate = await _locaRepo.FindById(id, null);
                if (locationUpdate == null)
                {
                    return NotFound();
                }
                locationDTO.IsDefault = locationUpdate.IsDefault;
                _mapper.Map(locationDTO, locationUpdate);
                await _locaRepo.Update(locationUpdate);
                return Ok("Sửa thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                var locationDelete = await _locaRepo.FindSingle(x => x.Id == id);
                if (locationDelete == null)
                {
                    return NotFound();
                }
                await _locaRepo.Remove(locationDelete);
                return Ok("Xóa thành công");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDefaultLocation(int id, string email)
        {
            try
            {
                var locationUpdate = await _locaRepo.FindById(id, null);
                if (locationUpdate == null)
                {
                    return NotFound();
                }
                locationUpdate.IsDefault = true;
                var locationToUpdate = await _locaRepo.FindSingle(x => x.User.Email == email && x.IsDefault == true);

                if (locationToUpdate == null)
                {
                    await _locaRepo.Update(locationUpdate);
                    return Ok();

                }
                locationToUpdate.IsDefault = false;
                locationToUpdate.UpdatedAt = DateTime.Now;

                await _locaRepo.Update(locationUpdate);
                await _locaRepo.Update(locationToUpdate);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}