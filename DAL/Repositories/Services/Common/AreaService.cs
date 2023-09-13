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
    public class AreaService : Repository<Area>, IAreaService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public AreaService(DbContext context) : base(context)
        {
        }

        public async Task<List<Area>> GetArea(int page, int pageSize)
        {
            IQueryable<Area> query = _appContext.Area.OrderBy(u => u.Id).Include(m=>m.City);

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

    }
}
