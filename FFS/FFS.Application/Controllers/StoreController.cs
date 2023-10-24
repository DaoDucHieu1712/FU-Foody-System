using FFS.Application.DTOs.Store;
using FFS.Application.Repositories;
using FFS.Application.Repositories.Impls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        [HttpPost("exportfood")]
        public async Task<IActionResult> ExportFood(int id)
        {
            try
            {
                var data = await _storeRepository.ExportFood(id);
                string uniqueFileName = "ThongKe_MonAn_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("exportinventory")]
        public async Task<IActionResult> ExportInventory(int id)
        {
            try
            {
                var data = await _storeRepository.ExportInventory(id);
                string uniqueFileName = "ThongKe_Kho_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", uniqueFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(int id, StoreInforDTO storeInforDTO)
        {
            try
            {
                StoreInforDTO inforDTO = await _storeRepository.UpdateStore(id, storeInforDTO);
                return Ok(inforDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
