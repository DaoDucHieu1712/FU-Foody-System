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
        private readonly ApplicationDbContext _db;
        //private readonly ILocationRepository _locaRepo;
        private readonly IMapper _mapper;

        public LocationController(ApplicationDbContext db, IMapper mapper) {
            _db = db;
            //_locaRepo = locationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> ListLocation()
        {
            try
            {
                var uID = "1";
                var locations = await _db.Locations.Include(x => x.User).Where(x => x.UserId == uID).ToListAsync();
                //var locations = _locaRepo.GetList(x => x.UserId == uID, x => x.User);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                var location = await _db.Locations.FindAsync(id);
                if (location == null)
                {
                    return NotFound();
                }
                location.IsDelete= true;
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDefaultLocation(int id, LocationDTO locationDTO)
        {
            try
            {
                if (id != locationDTO.Id)
                {
                    return BadRequest();
                }
                var uID = "1";
                var locations = await _db.Locations.Where(x => x.UserId == uID).ToListAsync();
                Location locationToUpdate = null;
                foreach (var item in locations)
                {
                    if (item.IsDefault)
                    {
                        locationToUpdate = item;
                        break;
                    }
                }
                if (locationToUpdate != null)
                {
                    locationToUpdate.IsDefault = false;
                    locationToUpdate.UpdatedAt = DateTime.Now;
                }

                var defaultLocation = await _db.Locations.FirstOrDefaultAsync(x => x.Id == locationDTO.Id);
                if (defaultLocation != null)
                {
                    defaultLocation.IsDefault = true;
                    defaultLocation.UpdatedAt = DateTime.Now;
                }

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
