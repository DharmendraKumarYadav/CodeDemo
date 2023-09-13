using BDB;
using Core.Enums;
using DAL.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.BikeManagement
{
    public class BikeSimilarService : Repository<BikeSimilar>, IBikeSimilarService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public BikeSimilarService(DbContext context) : base(context)
        {
        }

        public async Task<List<SimilarBikeModel>> GetSimilarBikes(int page, int pageSize,int bikeId)
        {
            List<SimilarBikeModel> featuredBikeTypeModels = new List<SimilarBikeModel>();
            IQueryable<BikeSimilar> query = _appContext.BikeSimilar.Where(m=>m.BikeId==bikeId).OrderBy(u => u.Id)
                   .Include(m => m.Bike).ThenInclude(m => m.Brand)
                   .Include(m => m.Bike).ThenInclude(m => m.Category)
                     .Include(m => m.Bike).ThenInclude(m => m.BodyStyle);
            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                featuredBikeTypeModels.Add(new SimilarBikeModel
                {
                    Id = item.Id,
                    Name = item.Bike.Name,
                    BodyStyle = item.Bike.BodyStyle.Name,
                    Category = item.Bike.Category.Name,
                    BrandName = item.Bike.Brand.Name,
                    BikeId = item.Bike.Id,
                });
            }

            return featuredBikeTypeModels;
        }
    }
}
