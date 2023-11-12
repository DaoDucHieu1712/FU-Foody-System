using Dapper;
using System.Data;

using FFS.Application.Data;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;

using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using FFS.Application.DTOs.Report;
using FFS.Application.Helper;
using FFS.Application.Entities.Constant;
using System.Drawing;
using FFS.Application.DTOs.User;
using DocumentFormat.OpenXml.Spreadsheet;
using FFS.Application.Entities.Enum;

namespace FFS.Application.Repositories.Impls
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly DapperContext _dapperContext;


        public UserRepository(ApplicationDbContext context, DapperContext dapperContext)
        {
            _context = context;
            _dapperContext = dapperContext;
        }

       

        public int CountGetUsers(UserParameters userParameters)
        {
            try
            {
                dynamic returnData = null;
                var parameters = new DynamicParameters();
                //parameters.Add("userId", reportParameters.uId);
                parameters.Add("username", userParameters.Username);
                parameters.Add("email", userParameters.Email);
                parameters.Add("role", userParameters.Role);


                using var db = _dapperContext.connection;

                returnData = db.QuerySingle<int>("CountGetUsers", parameters, commandType: CommandType.StoredProcedure);
                db.Close();
                return returnData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<byte[]> ExportUser()
        {
            try
            {
                var usersWithRoles = await _context.Users
                    .Join(
                        _context.UserRoles,
                        user => user.Id,
                        userRole => userRole.UserId,
                        (user, userRole) => new
                        {
                            User = user,
                            RoleId = userRole.RoleId
                        }
                    ).Join(_context.Roles,
                        user => user.RoleId,
                        role => role.Id,
                        (user, role) => new
                        {
                             User = user, RoleName = role.Name
                        }
                    )
                    .ToListAsync();

                var Users = usersWithRoles
                    .Select((item, index) => new UserExportDTO
                    {
                        Number = index + 1,
                        Username = item.User.User.UserName,
                        Email = item.User.User.Email,
                        Role = item.RoleName
                    })
                    .ToList();
                using (var workbook = new XLWorkbook())
                {
                    ExcelConfiguration.ExportUser(Users, workbook);

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return await Task.FromResult(stream.ToArray());
                    }
                }
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<dynamic> GetRequestCreateAccount(UserParameters userParameters)
        {
            try
            {
                dynamic returnData = null;
                var parameters = new DynamicParameters();
                //parameters.Add("userId", reportParameters.uId);
                parameters.Add("username", userParameters.Username);
                parameters.Add("email", userParameters.Email);
                parameters.Add("role", userParameters.Role);
                parameters.Add("status", userParameters.Status);
                parameters.Add("pageNumber", userParameters.PageNumber);
                parameters.Add("pageSize", userParameters.PageSize);

                using var db = _dapperContext.connection;

                returnData = db.Query<dynamic>("GetRequestCreateAccount", parameters, commandType: CommandType.StoredProcedure);
                db.Close();
                return returnData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int CountGetRequestCreateAccount(UserParameters userParameters)
        {
            try
            {
                dynamic returnData = null;
                var parameters = new DynamicParameters();
                //parameters.Add("userId", reportParameters.uId);
                parameters.Add("username", userParameters.Username);
                parameters.Add("email", userParameters.Email);
                parameters.Add("role", userParameters.Role);
                parameters.Add("status", userParameters.Status);



                using var db = _dapperContext.connection;

                returnData = db.QuerySingle<int>("CountGetRequestCreateAccount", parameters, commandType: CommandType.StoredProcedure);
                db.Close();
                return returnData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

        public IEnumerable<dynamic> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public IEnumerable<dynamic> GetUsers(UserParameters userParameters)
        {
            try
            {
                dynamic returnData = null;
                var parameters = new DynamicParameters();
                //parameters.Add("userId", reportParameters.uId);
                parameters.Add("username", userParameters.Username);
                parameters.Add("email", userParameters.Email);
                parameters.Add("role", userParameters.Role);
                parameters.Add("pageNumber", userParameters.PageNumber);
                parameters.Add("pageSize", userParameters.PageSize);

                using var db = _dapperContext.connection;

                returnData = db.Query<dynamic>("GetUsers", parameters, commandType: CommandType.StoredProcedure);
                db.Close();

                List<Combo> c = _context.Combos.ToList();
                return returnData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void BanAccount(string idBan)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == idBan);
                if(user is null)
                {
                    throw new Exception("Người dùng không tồn tại, xin vui lòng kiểm tra lại!");
                }
                user.Allow = false;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void UnBanAccount(string idUnBan)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == idUnBan);
                if (user is null)
                {
                    throw new Exception("Người dùng không tồn tại, xin vui lòng kiểm tra lại!");
                }
                user.Allow = true;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public void ApproveUser(string id, string action)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == id);
                if (user is null)
                {
                    throw new Exception("Người dùng không tồn tại, xin vui lòng kiểm tra lại!");
                }
                if(action == "Accept")
                {
                    user.Status = StatusUser.Accept;
                }
                if (action == "Reject")
                {
                    user.Status = StatusUser.Reject;
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
