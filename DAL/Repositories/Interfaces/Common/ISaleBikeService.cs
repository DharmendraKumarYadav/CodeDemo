using Core.Models;
using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface ISaleBikeService : IRepository<SaleBike>
    {
        Task<List<SaleBike>> GetBookedBike(int page, int pageSize, string userId);

        Task<List<SaleBike>> GetSaleBike(int page, int pageSize,string userId);

        Task<List<SaleBike>> GetBrokerRequestedBike(int page, int pageSize, string dealerId);


        Task<List<SaleBike>> GetDealerBikeAvailableForSale(string dealerId);

        Task<List<AvailableShowRoomBikes>> GetAllBikeByCityAndVariant(int cityId, int variantId);
    }
}
