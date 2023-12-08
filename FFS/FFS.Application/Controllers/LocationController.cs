using AutoMapper;
using FFS.Application.Data;
using FFS.Application.DTOs.Location;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;

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
		private readonly IStoreRepository _storeRepository;

		private readonly IMapper _mapper;

        public LocationController(ILocationRepository locaRepo, IMapper mapper, IStoreRepository storeRepository)
        {
            _locaRepo = locaRepo;
            _mapper = mapper;
			_storeRepository = storeRepository;
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
        public async Task<ActionResult<Location>> AddLocation([FromBody] Location locationDTO)
        {
            try
            {
                await _locaRepo.Add(locationDTO);
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
            catch (Exception ex) { 
                return StatusCode(500, ex.Message);
            }
        }

		[HttpGet("{storeId}")]
		public async Task<IActionResult> GetLocation(int storeId)
		{
			try
			{
				var store = await _storeRepository.FindSingle(x => x.Id == storeId);
				if(store != null)
				{
					var location = await _locaRepo.FindSingle(x => x.UserId == store.UserId, null);
					if(location == null)
					{
						return NotFound();
					}
					return Ok(location);
				}
				return NotFound();
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
