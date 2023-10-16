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
                var uID = "1";
                var locations = await _db.Locations.Include(x => x.User).Where(x => x.UserId == uID).ToListAsync();
                return Ok(locations);
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
                var newLocation = new Location {
                    UserId = "1",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDefault = false,
                    IsDelete = false,
                    Address = locationDTO.Address,
                    Description = locationDTO.Description,
                    Receiver = locationDTO.Receiver,
                    PhoneNumber = locationDTO.PhoneNumber
                };
                await _db.Locations.AddAsync(newLocation);
                await _db.SaveChangesAsync();

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
                if (id != locationDTO.Id)
                {
                    return BadRequest();
                }
                var updateLocation = new Location
                {
                    UserId = "1",
                    UpdatedAt = DateTime.Now,
                    Id = (int)locationDTO.Id,
                    Address = locationDTO.Address,
                    Description = locationDTO.Description,
                    Receiver = locationDTO.Receiver,
                    PhoneNumber = locationDTO.PhoneNumber
                };
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
