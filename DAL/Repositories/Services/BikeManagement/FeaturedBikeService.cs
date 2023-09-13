using BDB;
using Core.Enums;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.BikeManagement
{
    public class FeaturedBikeService : Repository<FeaturedBike>, IFeaturedBikeService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public FeaturedBikeService(DbContext context) : base(context)
        {
        }

        public async Task<List<FeaturedBikeTypeModel>> GetFeaturedBikes(int page, int pageSize)
        {
            List<FeaturedBikeTypeModel> featuredBikeTypeModels = new List<FeaturedBikeTypeModel>();
            IQueryable<FeaturedBike> query = _appContext.FeaturedBikes.OrderBy(u => u.Id).
                    Include(m => m.Bike).ThenInclude(m => m.Brand)
                   .Include(m => m.Bike).ThenInclude(m => m.Category)
                     .Include(m => m.Bike).ThenInclude(m => m.BodyStyle);
            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                featuredBikeTypeModels.Add(new FeaturedBikeTypeModel
                {
                    Id = item.Id,
                    Type = ((BikeTypeEnum)item.FeatureType).ToString(),
                    Name = item.Bike.Name,
                    BodyStyle = item.Bike.BodyStyle.Name,
                    Category = item.Bike.Category.Name,
                    BrandName = item.Bike.Brand.Name,
                    BikeId=item.Bike.Id,
                    TypeId=item.FeatureType
                });
            }

            return featuredBikeTypeModels;
        }
    }
}
