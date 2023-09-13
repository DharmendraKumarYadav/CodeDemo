using Core.Models;
using DAL.Models.Idenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core.Interfaces
{
    public interface IAccountManager
    {

        Task<bool> CheckPasswordAsync(User user, string password);
        Task<(bool Succeeded, string[] Errors)> CreateRoleAsync(Role role);
        Task<(bool Succeeded, string[] Errors)> CreateUserAsync(User user,string roles, string password);
        Task<(bool Succeeded, string[] Errors)> DeleteUserAsync(User user);
        Task<(bool Succeeded, string[] Errors)> DeleteUserAsync(string userId);
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<Role> GetRoleLoadRelatedAsync(string roleName);
        Task<List<Role>> GetRolesLoadRelatedAsync(int page, int pageSize);
        Task<(User User, string Role)?> GetUserAndRolesAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByUserNameAsync(string userName);
        Task<IList<string>> GetUserRolesAsync(User user);
        Task<List<(User User, string Role)>> GetUsersAndRolesAsync(int page, int pageSize);
        Task<List<User>> GetDealers();

        Task<List<User>> GetAdminstrator();
        Task<(bool Succeeded, string[] Errors)> ResetPasswordAsync(User user, string newPassword);
        Task<(bool Succeeded, string[] Errors)> UpdatePasswordAsync(User user, string currentPassword, string newPassword);
        Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(User user);
        Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(User user, string roles);

        Task<User> GetUserByPhoneOrEmailAsync(string phone);

        Task<int> GetUserCount();

        Task<List<DashboardMonthlyModel>> GetMonthlyUser(int year);

        Task<List<ReportDropDown>> GetDealerData();

        Task<(bool Succeeded, string[] Errors)> UpdateDealerAsync(User user);
        Task<User> GetDealerById(string userId);
    }
}
