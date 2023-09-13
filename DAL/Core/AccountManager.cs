using Core.Models;
using DAL.Core.Interfaces;
using DAL.Models.Idenity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core
{
    public class AccountManager : IAccountManager
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public AccountManager(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IHttpContextAccessor httpAccessor)
        {
            _context = context;
            _context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(ClaimConstants.Subject)?.Value?.Trim();
            _userManager = userManager;
            _roleManager = roleManager;

        }




        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<(bool Succeeded, string[] Errors)> CreateUserAsync(User user, string roles, string password)
        {
            bool IsPhoneAlreadyRegistered = _userManager.Users.Any(item => item.PhoneNumber == user.PhoneNumber);
            if (IsPhoneAlreadyRegistered)
            {
                string[] str = new string[1];
                str[0] = "Phone number already used";
                return (false, str);
            }
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());


            user = await _userManager.FindByNameAsync(user.UserName);

            try
            {
                var roleArray = new[] { roles };
                result = await this._userManager.AddToRolesAsync(user, roleArray);
            }
            catch
            {
                await DeleteUserAsync(user);
                throw;
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(user);
                return (false, result.Errors.Select(e => e.Description).ToArray());
            }

            return (true, new string[] { });
        }


        public async Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(User user)
        {
            return await UpdateUserAsync(user, null);
        }


        public async Task<(bool Succeeded, string[] Errors)> UpdateUserAsync(User user, string roles)
        {
            var userData = _userManager.Users.Where(item => item.PhoneNumber == user.PhoneNumber).FirstOrDefault();
            if (userData != null)
            {
                if (userData?.Id != user.Id)
                {
                    string[] str = new string[1];
                    str[0] = "Phone number already used";
                    return (false, str);
                }
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());


            if (roles != null)
            {
                var rolesArray = new[] { roles };
                var userRoles = await _userManager.GetRolesAsync(user);

                var rolesToRemove = userRoles.Except(rolesArray).ToArray();
                var rolesToAdd = rolesArray.Except(userRoles).Distinct().ToArray();

                if (rolesToRemove.Any())
                {
                    result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!result.Succeeded)
                        return (false, result.Errors.Select(e => e.Description).ToArray());
                }

                if (rolesToAdd.Any())
                {
                    result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!result.Succeeded)
                        return (false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            return (true, new string[] { });
        }


        public async Task<(bool Succeeded, string[] Errors)> ResetPasswordAsync(User user, string newPassword)
        {
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            return (true, new string[] { });
        }

        public async Task<(bool Succeeded, string[] Errors)> UpdatePasswordAsync(User user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            return (true, new string[] { });
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                if (!_userManager.SupportsUserLockout)
                    await _userManager.AccessFailedAsync(user);

                return false;
            }

            return true;
        }


        public async Task<(bool Succeeded, string[] Errors)> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
                return await DeleteUserAsync(user);

            return (true, new string[] { });
        }


        public async Task<(bool Succeeded, string[] Errors)> DeleteUserAsync(User user)
        {
            var result = await _userManager.DeleteAsync(user);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<Role> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }


        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }


        public async Task<Role> GetRoleLoadRelatedAsync(string roleName)
        {
            var role = await _context.Roles
                .Include(r => r.Claims)
                .Include(r => r.UserRoles)
                .Where(r => r.Name == roleName)
                .SingleOrDefaultAsync();

            return role;
        }


        public async Task<List<Role>> GetRolesLoadRelatedAsync(int page, int pageSize)
        {
            IQueryable<Role> rolesQuery = _context.Roles
                .Include(r => r.Claims)
                .Include(r => r.UserRoles)
                .OrderBy(r => r.Name);

            if (page != -1)
                rolesQuery = rolesQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                rolesQuery = rolesQuery.Take(pageSize);

            var roles = await rolesQuery.ToListAsync();

            return roles;
        }


        public async Task<(bool Succeeded, string[] Errors)> CreateRoleAsync(Role role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());


            role = await _roleManager.FindByNameAsync(role.Name);


            return (true, new string[] { });
        }

        public async Task<(User User, string Role)?> GetUserAndRolesAsync(string userId)
        {
            var user = await _context.Users
                .Include(m => m.UserRoles)
                    .Where(u => u.Id.ToString() == userId)
                    .SingleOrDefaultAsync();

            if (user == null)
                return null;

            var userRoleIds = user.UserRoles.Select(r => r.RoleId).ToList();

            var roles = await _context.Roles
                .Where(r => userRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .FirstOrDefaultAsync();

            return (user, roles);
        }

        public async Task<List<(User User, string Role)>> GetUsersAndRolesAsync(int page, int pageSize)
        {
            try
            {
                IQueryable<User> usersQuery = _context.Users
                               .Include(u => u.UserRoles)
                               .Include(m=>m.Brokers)
                               .OrderBy(u => u.UserName);

                if (page != -1)
                    usersQuery = usersQuery.Skip((page - 1) * pageSize);

                if (pageSize != -1)
                    usersQuery = usersQuery.Take(pageSize);

                var users = await usersQuery.ToListAsync();

                var roles1 = await _context.Roles.ToListAsync();

                var userRoleIds = users.SelectMany(u => u.UserRoles.Select(r => r.RoleId)).ToList();

                //var brokerDealers = users.SelectMany(u => u.Brokers.Select(r => r.DealerId.ToString())).ToList();

                var roles = await _context.Roles
                    .Where(r => userRoleIds.Contains(r.Id))
                    .ToArrayAsync();

                return users
                    .Select(u => (u, roles.Where(r => u.UserRoles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).FirstOrDefault()))
                    .ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task<User> GetUserByPhoneOrEmailAsync(string data)
        {
            var appuser = await _context.Users.Where(m => m.PhoneNumber == data || m.Email== data).FirstOrDefaultAsync();
            return appuser;
        }
        public async Task<int> GetUserCount()
        {
            var userCount = await (from user in _context.Users
                                   join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                   join roles in _context.Roles on userRole.RoleId equals roles.Id
                                   where user.IsEnabled == true
                                   select new { Name = user.UserName }).CountAsync();

            return (userCount);
        }
        public async Task<List<DashboardMonthlyModel>> GetMonthlyUser(int year)
        {
            List<DashboardMonthlyModel> dashboardMonthlyModels= new List<DashboardMonthlyModel>();

            var dealerCount = await _context.Users.Where(m => m.IsEnabled).Where(w => w.CreatedDate.Year == year).GroupBy(g => g.CreatedDate.Month).Select(g => new DashboardMonthlyModel { MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key) , Count = g.Count() }).ToListAsync();
            var months = (from month in GetMonths()
                          join bookings in dealerCount
                          on month equals bookings.MonthName
                          into EmployeeAddressGroup
                          from address in EmployeeAddressGroup.DefaultIfEmpty()
                          select new DashboardMonthlyModel { MonthName = month, Count = address != null ? address.Count : 0 }).ToList();


            return months;
        }

        public async Task<List<ReportDropDown>> GetDealerData()
        {

            var dealer = await (from user in _context.Users
                                     join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                     join roles in _context.Roles on userRole.RoleId equals roles.Id
                                     where roles.NormalizedName == "DEALER"
                                     select new ReportDropDown { Id=user.Id.ToString(),Name=user.FullName }).ToListAsync();


            return dealer;
        }

        public List<string> GetMonths()
        {
            List<string> months = new List<string>();
            months.Add("January");
            months.Add("February");
            months.Add("March");
            months.Add("April");
            months.Add("May");
            months.Add("June");
            months.Add("July");
            months.Add("August");
            months.Add("September");
            months.Add("October");
            months.Add("November");
            months.Add("December");
            return months;
        }

        public async Task<List<User>> GetDealers()
        {
            var dealerList = await (from user in _context.Users
                                           join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                           join roles in _context.Roles on userRole.RoleId equals roles.Id
                                           where user.IsEnabled == true && roles.Name=="dealer"
                                           select user).ToListAsync();
            return dealerList;
        }
        public async Task<User> GetDealerById(string userId)
        {
            return  await (from user in _context.Users
                                    join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                    join roles in _context.Roles on userRole.RoleId equals roles.Id
                                    where user.IsEnabled == true && roles.Name == "dealer" && user.Id.ToString()== userId
                                    select user).FirstOrDefaultAsync();
            
        }
        public async Task<(bool Succeeded, string[] Errors)> UpdateDealerAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToArray());

            return (true, new string[] { });
        }


        public async Task<List<User>> GetAdminstrator()
        {
            var query = await (from user in _context.Users
                                    join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                    join roles in _context.Roles on userRole.RoleId equals roles.Id
                                    where user.IsEnabled == true && roles.Name == "administrator"
                                    select user).ToListAsync();
            return query;
        }
    }
}
