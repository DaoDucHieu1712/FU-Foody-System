
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
        Task<UserClientDTO> Login(string email, string password);
        Task<UserClientDTO> LoginWithFptMail(UserRegisterDTO userRegisterDTO);
        Task ShipperRegister(ShipperRegisterDTO userRegisterDTO);
        Task<ApplicationUser> Profile(string email);
        Task ProfileEdit(string email, UserCommandDTO userCommandDTO);
        Task<ApplicationUser> GetShipperById(string userId);
   
    }
}
