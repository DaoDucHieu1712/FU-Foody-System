using FFS.Application.DTOs.Admin;
using FFS.Application.DTOs.QueryParametter;
using FFS.Application.Entities;
using FFS.Application.Infrastructure.Interfaces;

namespace FFS.Application.Repositories {
    public interface IUserRepository {
        IEnumerable<dynamic> GetUsers(UserParameters userParameters);
        int CountGetUsers(UserParameters userParameters);
        IEnumerable<dynamic> GetRoles();
        Task<byte[]> ExportUser();
        IEnumerable<dynamic> GetRequestCreateAccount(UserParameters userParameters);
        int CountGetRequestCreateAccount(UserParameters userParameters);
        void BanAccount(string idBan);
        void UnBanAccount(string idUnBan);
        void ApproveUser(string id, string action);

		void ApprovePost(int postId, string action);
		List<AccountStatistic> AccountsStatistic();
		int CountTotalUsers();
		int CountGetPosts(UserParameters userParameters);
		IEnumerable<dynamic> GetPosts(UserParameters userParameters);
	}
}
