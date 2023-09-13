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
    public class BodyStyleService : Repository<BodyStyle>, IBodyStyleService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public BodyStyleService(DbContext context) : base(context)
        {
        }

        public async Task<List<BodyStyle>> GetBodyStyle(int page, int pageSize)
        {
            IQueryable<BodyStyle> query = _appContext.BodyStyles.OrderBy(u => u.Id);

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

    }
}
