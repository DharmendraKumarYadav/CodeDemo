using Core.Models;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Common
{
    public class DealerBrokerService : Repository<DealerBroker>, IDealerBrokerService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public DealerBrokerService(DbContext context) : base(context)
        {
        }

        public async Task<List<DealerBrokerModel>> GetDealers(string userId)
        {
            var listOfDealer=new List<DealerBrokerModel>();
            if (userId != null)
            {
                var dealerBroker = await _appContext.DealerBrokers.Where(m => m.BrokerId == new Guid(userId)).Include(m => m.Dealer).ToListAsync();

                foreach (var item in dealerBroker)
                {
                    listOfDealer.Add(new DealerBrokerModel { DealerId = item.DealerId.ToString(),BrokerId= userId, Name = item.Dealer.FullName, Id = item.Id, Email = item.Dealer.Email, Mobile = item.Dealer.PhoneNumber });
                }
            }
            else {
                var dealerList = await (from user in _appContext.Users
                                        join userRole in _appContext.UserRoles on user.Id equals userRole.UserId
                                        join roles in _appContext.Roles on userRole.RoleId equals roles.Id
                                        where user.IsEnabled == true && roles.Name == "dealer"
                                        select user).ToListAsync();
                foreach (var item in dealerList)
                {
                    listOfDealer.Add(new DealerBrokerModel { BrokerId = null, DealerId = item.Id.ToString(), Name = item.FullName, Email = item.Email, Mobile = item.PhoneNumber });
                }
            }
            return listOfDealer;

        }
        public async Task<List<DealerBrokerModel>> GetBrokers(string userId)
        {
            var listOfDealer = new List<DealerBrokerModel>();
            if (userId != null)
            {
                var dealerBroker = await _appContext.DealerBrokers.Where(m => m.DealerId == new Guid(userId)).Include(m => m.Broker).ToListAsync();

                foreach (var item in dealerBroker)
                {
                    listOfDealer.Add(new DealerBrokerModel { BrokerId = item.BrokerId.ToString(), DealerId = item.DealerId.ToString(), Name = item.Broker.FullName, Id = item.Id, Email = item.Broker.Email, Mobile = item.Broker.PhoneNumber });
                }
            }
            else {
                var brokerList = await (from user in _appContext.Users
                                        join userRole in _appContext.UserRoles on user.Id equals userRole.UserId
                                        join roles in _appContext.Roles on userRole.RoleId equals roles.Id
                                        where user.IsEnabled == true && roles.Name == "broker"
                                        select user).ToListAsync();
                foreach (var item in brokerList)
                {
                    listOfDealer.Add(new DealerBrokerModel { BrokerId = item.Id.ToString(), DealerId = null, Name = item.FullName, Email = item.Email, Mobile = item.PhoneNumber });
                }
            }
            return listOfDealer;

        }
    }
}
