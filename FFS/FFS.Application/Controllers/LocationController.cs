using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using FFS.Application.Data;
using FFS.Application.DTOs.Location;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;
using FFS.Application.Repositories;
using Microsoft.AspNetCore.Authorization;
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
		private ILoggerManager _logger;


		public LocationController(ILocationRepository locaRepo, IMapper mapper, IStoreRepository storeRepository, ILoggerManager logger)
        {
            _locaRepo = locaRepo;
            _mapper = mapper;
			_storeRepository = storeRepository;
			_logger = logger;
        }

		[Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Location>>> ListLocation(string email)
        {
            try
            {
				_logger.LogInfo($"Listing locations for email: {email}");
				var locations = await _locaRepo.GetList(x => x.User.Email == email && x.IsDelete == false, x => x.User);
				_logger.LogInfo($"Locations listed successfully for email: {email}");
				return Ok(locations);
            }
            catch (Exception ex)
            {
				_logger.LogError($"An error occurred while listing locations: {ex.Message}");
				return StatusCode(500, ex.Message);
            }
        }

		[Authorize]
		[HttpPost]
        public async Task<ActionResult<Location>> AddLocation([FromBody] Location locationDTO)
        {
            try
            {
				_logger.LogInfo("Adding location");
				await _locaRepo.Add(locationDTO);
				_logger.LogInfo("Location added successfully");
				return Ok("Thêm thành công");
            }
            catch (Exception ex)
            {
				_logger.LogError($"An error occurred while adding location: {ex.Message}");
				return StatusCode(500, ex.Message);
            }
        }

		[Authorize]
		[HttpPut("{id}")]
        public async Task<ActionResult> UpdateLocation(int id, LocationDTO locationDTO)
        {
            try
            {
				_logger.LogInfo($"Updating location with ID: {id}");
				var locationUpdate = await _locaRepo.FindById(id, null);
                if (locationUpdate == null)
                {
					_logger.LogInfo($"Location with ID: {id} not found");
					return NotFound();
                }
                locationDTO.IsDefault = locationUpdate.IsDefault;
                _mapper.Map(locationDTO, locationUpdate);
                await _locaRepo.Update(locationUpdate);
				_logger.LogInfo("Location updated successfully");
				return Ok("Sửa thành công");
            }
            catch (Exception ex)
            {
				_logger.LogError($"An error occurred while updating location: {ex.Message}");
				return StatusCode(500, ex.Message);
            }
        }

		[Authorize]
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
				_logger.LogInfo($"Deleting location with ID: {id}");
				var locationDelete = await _locaRepo.FindSingle(x => x.Id == id);
                if (locationDelete == null)
                {
                    return NotFound();
                }
                await _locaRepo.Remove(locationDelete);
				_logger.LogInfo("Location deleted successfully");
				return Ok("Xóa thành công");
            }
            catch (Exception ex) {
				_logger.LogError($"An error occurred while deleting location: {ex.Message}");
				return StatusCode(500, ex.Message);
            }
        }

		[Authorize]
		[HttpGet("{storeId}")]
		public async Task<IActionResult> GetLocation(int storeId)
		{
			try
			{
				_logger.LogInfo($"Getting location for Store ID: {storeId}");
				var store = await _storeRepository.FindSingle(x => x.Id == storeId);
				if(store != null)
				{
					var location = await _locaRepo.FindSingle(x => x.UserId == store.UserId, null);
					if(location == null)
					{
						_logger.LogInfo($"Location not found for Store ID: {storeId}");
						return NotFound();
					}
					_logger.LogInfo($"Location retrieved successfully for Store ID: {storeId}");
					return Ok(location);
				}
				_logger.LogInfo($"Store not found for Store ID: {storeId}");
				return NotFound();
			}
			catch (Exception ex)
			{
				_logger.LogError($"An error occurred while getting location: {ex.Message}");
				return StatusCode(500, ex.Message);
			}
		}

		[Authorize]
		[HttpPut("{id}")]
        public async Task<ActionResult> UpdateDefaultLocation(int id, string email)
		{
			try
            {
				_logger.LogInfo($"Updating default location with ID: {id} for email: {email}");
				var locationUpdate = await _locaRepo.FindById(id, null);
                if (locationUpdate == null)
                {
					_logger.LogInfo($"Location with ID: {id} not found");
					return NotFound();
                }
                locationUpdate.IsDefault = true;
                var locationToUpdate = await _locaRepo.FindSingle(x => x.User.Email == email && x.IsDefault == true);

                if (locationToUpdate == null)
                {
                    await _locaRepo.Update(locationUpdate);
					_logger.LogInfo($"Default location updated successfully for email: {email}");
					return Ok();

                }
                locationToUpdate.IsDefault = false;
                locationToUpdate.UpdatedAt = DateTime.Now;

                await _locaRepo.Update(locationUpdate);
                await _locaRepo.Update(locationToUpdate);
				_logger.LogInfo($"Default location updated successfully for email: {email}");
				return Ok();
            }
            catch (Exception ex)
            {
				_logger.LogError($"An error occurred while updating default location: {ex.Message}");
				return StatusCode(500, ex.Message);
            }
        }
    }
}
