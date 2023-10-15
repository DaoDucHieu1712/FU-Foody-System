using FFS.Application.DTOs.Store;

namespace FFS.Application.Repositories {
    public interface IStoreRepository {
        Task<StoreInforDTO> GetInformationStore(int id);
    }
}
