using FFS.Application.DTOs.Store;
using FFS.Application.Repositories;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FFS.Application.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreController : ControllerBase {
        private readonly IStoreRepository _storeRepository;
        public StoreController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreInformation(int id)
        {
            try
            {
                StoreInforDTO storeInforDTO = await _storeRepository.GetInformationStore(id);
                return Ok(storeInforDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
