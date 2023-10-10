using FFS.Application.Data;
using FFS.Application.DTOs.Auth;
using FFS.Application.Entities;
using Microsoft.AspNetCore.Identity;

namespace FFS.Application.Repositories.Impls
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task StoreRegister(StoreRegisterDTO storeRegisterDTO)
        {
            //using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                ApplicationUser _user = await _userManager.FindByEmailAsync(storeRegisterDTO.Email);
                if(_user!=null) throw new Exception("Email đã tồn tại , Vui lòng thử lại !");
                if(storeRegisterDTO.Password != storeRegisterDTO.PasswordConfirm) throw new Exception("Vui lòng kiểm tra lại mật khẩu !");

                var NewUser = new ApplicationUser
                {
                    FirstName = storeRegisterDTO.FirstName,
                    LastName = storeRegisterDTO.LastName,
                    Avatar = storeRegisterDTO.Avatar,
                    Allow = storeRegisterDTO.Allow,
                    Gender = storeRegisterDTO.Gender,
                    BirthDay = storeRegisterDTO.BirthDay,
                    UserName = storeRegisterDTO.Email,
                    Email = storeRegisterDTO.Email,
                };

                var result = await _userManager.CreateAsync(NewUser, storeRegisterDTO.Password);
                if (result.Succeeded == false)
                {
                    var specificErrors = result.Errors.FirstOrDefault();
                    throw new Exception(specificErrors?.Description);
                }
                var role_rs = await _userManager.AddToRoleAsync(NewUser, "StoreOwner");
                if (role_rs.Succeeded == false) throw new Exception("Đã có lỗi xảy ra");

                var _newuser = _context.ApplicationUsers.FirstOrDefault(x => x.Email == NewUser.Email);

                var NewStore = new Store
                {
                    UserId = _newuser.Id,
                    StoreName= storeRegisterDTO.StoreName,
                    AvatarURL = storeRegisterDTO.AvatarURL,
                    Description = storeRegisterDTO.Description,
                    Address = storeRegisterDTO.Address,
                    TimeStart= storeRegisterDTO.TimeStart,
                    TimeEnd = storeRegisterDTO.TimeEnd,
                    PhoneNumber = storeRegisterDTO.PhoneNumber,
                };

                await _context.Stores.AddAsync(NewStore);
                await _context.SaveChangesAsync();
            
            }
            catch (Exception ex)
            {
                //transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}
