using FFS.Application.DTOs.Store;

namespace FFS.Application.Repositories {
    public interface IStoreRepository {
        Task<StoreInforDTO> GetInformationStore(int id);
        Task<StoreInforDTO> UpdateStore(int id, StoreInforDTO storeInforDTO);
        Task<byte[]> ExportFood(int id);
        Task<byte[]> ExportInventory(int id);
    }
}
