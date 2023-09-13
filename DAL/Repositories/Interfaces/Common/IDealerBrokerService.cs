using Core.Models;
using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IDealerBrokerService : IRepository<DealerBroker>
    {
        Task<List<DealerBrokerModel>> GetDealers(string userId);
        Task<List<DealerBrokerModel>> GetBrokers(string userId);
    }
}
