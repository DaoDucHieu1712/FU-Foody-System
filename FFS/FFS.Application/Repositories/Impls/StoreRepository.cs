using AutoMapper;
using ClosedXML.Excel;
using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FFS.Application.Repositories.Impls
{
    public class StoreRepository : IStoreRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StoreRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<byte[]> ExportFood(int id)
        {
            try
            {
                List<Food> foodsOfStore = await _context.Foods
                    .Where(x => x.StoreId == id)
                    .Include(x => x.Category)
                    .ToListAsync();

                var exportFoodDTOs = _mapper.Map<List<ExportFoodDTO>>(foodsOfStore);

                using (var workbook = new XLWorkbook())
                {
                    ExcelConfiguration.ExportFood(exportFoodDTOs, workbook);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return await Task.FromResult(stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<byte[]> ExportInventory(int id)
        {
            try
            {
                List<Inventory> inventoriesOfStore = await _context.Inventories
                    .Where(x => x.StoreId == id)
                    .Include(x => x.Food).ThenInclude(a => a.Category)
                    .ToListAsync();

                var exportInventoryDTOs = _mapper.Map<List<ExportInventoryDTO>>(inventoriesOfStore);

                using (var workbook = new XLWorkbook())
                {
                    ExcelConfiguration.ExportInventory(exportInventoryDTOs, workbook);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return await Task.FromResult(stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<StoreInforDTO> GetInformationStore(int id)
            {
                try
                {
                    Store? stores = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
                    if (stores == null)
                    {
                        throw new Exception("Cửa hàng không tồn tại!");
                    }
                    else
                    {
                        StoreInforDTO storeInforDTO = _mapper.Map<StoreInforDTO>(stores);
                        return storeInforDTO;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public async Task<StoreInforDTO> UpdateStore(int id, StoreInforDTO storeInforDTO)
            {
                try
                {
                    Store? store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
                    if (store == null)
                    {
                        throw new Exception("Cửa hàng không tồn tại!");
                    }
                    else
                    {
                        store.StoreName = storeInforDTO.StoreName;
                        store.AvatarURL = storeInforDTO.AvatarURL;
                        store.Address = storeInforDTO.Address;
                        store.PhoneNumber = storeInforDTO.PhoneNumber;
                        store.Description = storeInforDTO.Description;
                        store.TimeStart = storeInforDTO.TimeStart;
                        store.TimeEnd = storeInforDTO.TimeEnd;
                        await _context.SaveChangesAsync();
                        return storeInforDTO;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
