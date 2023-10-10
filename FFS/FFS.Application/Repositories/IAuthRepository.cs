
﻿using FFS.Application.DTOs.Auth;

﻿using FFS.Application.Entities;


namespace FFS.Application.Repositories
{
    public interface IAuthRepository
    {
        Task StoreRegister(StoreRegisterDTO storeRegisterDTO);
        Task<string> GenerateToken(ApplicationUser us);
        Task<bool> ResetPassword(string email);
    }
}
