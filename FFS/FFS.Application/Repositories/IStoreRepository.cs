using FFS.Application.DTOs.Common;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories {
    public interface IStoreRepository : IRepository<Store, int> {
        Task<List<Store>> GetTop8PopularStore();
        PagedList<Store> GetAllStores(AllStoreParameters allStoreParameters);
        Task<StoreInforDTO> GetInformationStore(int id);
        Task<StoreInforDTO> UpdateStore(int id, StoreInforDTO storeInforDTO);
        Task<byte[]> ExportFood(int id);
        Task<byte[]> ExportInventory(int id);
        Task<StoreInforDTO> GetDetailStore(int id);
        Task<dynamic> GetCommentByStore(int rate, int id);
        Task<dynamic> GetCommentReply(int id);
        Task<List<FoodDTO>> GetFoodByCategory(int idShop, int idCategory);

    }
}
