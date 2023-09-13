using DAL.Core;
using DAL.Core.Interfaces;
using DAL.Models.Idenity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }



    public class DatabaseInitializer : IDatabaseInitializer
    {
        readonly ApplicationDbContext _context;
        readonly IAccountManager _accountManager;
        readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, IAccountManager accountManager, ILogger<DatabaseInitializer> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);
            await SeedDefaultUsersAsync();
        }

        private async Task SeedDefaultUsersAsync()
        {
            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";
                const string dealerRoleName = "dealer";
                const string brokerRoleName = "broker";

                await EnsureRoleAsync(adminRoleName, "administrator");
                await EnsureRoleAsync(userRoleName, "user");
                await EnsureRoleAsync(dealerRoleName, "dealer");
                await EnsureRoleAsync(brokerRoleName, "broker");

                await CreateUserAsync("admin", "Password@123", "Administrator", "admin@dekhbikedekh.com", "9594460614",  adminRoleName );

                _logger.LogInformation("Inbuilt account generation completed");
            }
        }

        private async Task EnsureRoleAsync(string roleName, string description)
        {
            if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                _logger.LogInformation($"Generating default role: {roleName}");

                Role applicationRole = new Role(roleName);
                applicationRole.IsAllowDelete = false;
                applicationRole.CreatedBy = "system";
                applicationRole.UpdatedBy = "system";
                applicationRole.CreatedDate = DateTime.Now;
                applicationRole.UpdatedDate = DateTime.Now;
                var result = await this._accountManager.CreateRoleAsync(applicationRole);

                if (!result.Succeeded)
                    throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");
            }
        }

        private async Task<User> CreateUserAsync(string userName, string password, string fullName, string email, string phoneNumber, string roles)
        {
            _logger.LogInformation($"Generating default user: {userName}");

            User applicationUser = new User
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                CreatedBy= "system",
                UpdatedBy="system",
                CreatedDate= DateTime.Now,
                UpdatedDate=DateTime.Now,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed= true,
                EmailConfirmed = true,
                IsEnabled = true,
                IsSuperAdmin= true,
            };
            var result = await _accountManager.CreateUserAsync(applicationUser, roles, password);

            if (!result.Succeeded)
                throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");

            return applicationUser;
        }
    }
}
