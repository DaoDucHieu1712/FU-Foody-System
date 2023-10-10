using AutoMapper;
using FFS.Application.Data;
using FFS.Application.DTOs;
using FFS.Application.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FFS.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public LocationController(ApplicationDbContext db, IMapper mapper) {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> ListLocation()
        {
            try
            {
                var locations = await _db.Locations.ToListAsync();
                return Ok(_mapper.Map<List<LocationDTO>>(locations));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Location>> AddLocation(LocationDTO locationDTO)
        {
            try
            {
                var newLocation = _mapper.Map<Location>(locationDTO);
                _db.Locations.Add(newLocation);
                await _db.SaveChangesAsync();

                return Ok();
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
                if (id != locationDTO.Id)
                {
                    return BadRequest();
                }
                var updateLocation = _mapper.Map<Location>(locationDTO);
                _db.Entry(updateLocation).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Ok();
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
                var location = await _db.Locations.FindAsync(id);
                if (location == null)
                {
                    return NotFound();
                }
                _db.Locations.Remove(location);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
