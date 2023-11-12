﻿using System.Data;

using AutoMapper;
using ClosedXML.Excel;
using Dapper;

using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;

using FFS.Application.Data;
using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FFS.Application.Repositories.Impls
{
    public class StoreRepository : EntityRepository<Store, int>, IStoreRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public StoreRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<byte[]> ExportFood(int id)
        {
            try
            {
                List<Food> foodsOfStore = await _context.Foods
                    .Where(x => x.StoreId == id && x.IsDelete == false)
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
                    .Where(x => x.StoreId == id && x.IsDelete == false)
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

        public async Task<StoreInforDTO> GetDetailStore(int id)
        {
            try
            {
                Store? stores = await FindById(id, x => x.Foods, x => x.FoodCombos, x => x.Discounts, x => x.Categories);
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

        public async Task<dynamic> GetCommentByStore(int rate, int id)
        {
            try
            {
                dynamic returnData = null;
                var parameters = new DynamicParameters();
                parameters.Add("storeId", id);
                parameters.Add("rate", rate);


                using (var db = _context.Database.GetDbConnection())
                {
                    if (db.State != ConnectionState.Open)
                    {
                        db.Open();
                    }

                    returnData = await db.QueryAsync<dynamic>("GetCommentByStore", parameters, commandType: CommandType.StoredProcedure);
                }
                return returnData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FoodDTO>> GetFoodByCategory(int idShop, int idCategory)
        {
            try
            {
                List<Food> foods = await _context.Foods.Where(x => x.StoreId == idShop && x.CategoryId == idCategory).ToListAsync();
                if (foods.Count == 0)
                {
                    throw new Exception();
                }
                else
                {
                    List<FoodDTO> foodDTOs = _mapper.Map<List<FoodDTO>>(foods);
                    return foodDTOs;
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

        public async Task<dynamic> GetCommentReply(int id)
        {
            try
            {
                dynamic returnData = null;
                var parameters = new DynamicParameters();
                parameters.Add("commentId", id);


                using (var db = _context.Database.GetDbConnection())
                {
                    if (db.State != ConnectionState.Open)
                    {
                        db.Open();
                    }

                    returnData = await db.QueryAsync<dynamic>("GetReplyComment", parameters, commandType: CommandType.StoredProcedure);
                }
                return returnData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
