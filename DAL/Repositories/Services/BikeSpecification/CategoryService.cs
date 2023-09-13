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
    public class CategoryService : Repository<Category>, ICategoryService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public CategoryService(DbContext context) : base(context)
        {
        }

        public async Task<List<Category>> GetCategory(int page, int pageSize)
        {
            IQueryable<Category> query = _appContext.Category.OrderBy(u => u.Id);

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

    }
}
