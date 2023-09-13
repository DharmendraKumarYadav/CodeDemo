using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IBikeUserRatingService : IRepository<BikeUserRating>
    {
        Task<List<BikeUserRating>> GetBikeUserRating(int page, int pageSize);

        Task<List<BikeUserRating>> GetBikeRatingsByBikeId(int bikeId);
    }
}
