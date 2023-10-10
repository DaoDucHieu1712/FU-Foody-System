using FFS.Application.DTOs.Auth;

namespace FFS.Application.Repositories
{
    public interface IAuthRepository
    {
        Task StoreRegister(StoreRegisterDTO storeRegisterDTO);
    }
}
