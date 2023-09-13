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
    public class BikeUserRatingService : Repository<BikeUserRating>, IBikeUserRatingService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public BikeUserRatingService(DbContext context) : base(context)
        {
        }

        public async Task<List<BikeUserRating>> GetBikeUserRating(int page, int pageSize)
        {
            IQueryable<BikeUserRating> query = _appContext.BikeUserRatings
                .Include(m=>m.Bike)
                .OrderBy(u => u.Id);

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }
        public async Task<List<BikeUserRating>> GetBikeRatingsByBikeId(int bikeId)
        {
            IQueryable<BikeUserRating> query = _appContext.BikeUserRatings.Where(m=>m.BikeId==bikeId && m.IsPublished).OrderByDescending(u => u.Id);

            return await query.ToListAsync();
        }

    }
}
