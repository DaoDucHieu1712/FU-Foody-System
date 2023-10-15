
﻿using FFS.Application.DTOs.Auth;

﻿using FFS.Application.Entities;


namespace FFS.Application.Repositories
{
    public interface IAuthRepository
    {
        Task StoreRegister(StoreRegisterDTO storeRegisterDTO);
        Task ChangePassword(ChangePasswordDTO changePasswordDTO);
        Task<string> GenerateToken(ApplicationUser us);
        Task<bool> ResetPassword(string email);
        Task<string> Login(string email, string password);
    }
}
