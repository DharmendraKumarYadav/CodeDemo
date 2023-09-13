using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.BikeSpecification
{
    public class BrandService : Repository<Brand>, IBrandService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public BrandService(DbContext context) : base(context)
        {
        }

        public async Task<List<Brand>> GetBrand(int page, int pageSize)
        {
            IQueryable<Brand> query = _appContext.Brands.OrderBy(u => u.Id);

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

    }
}
