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
    public class DisplacementService : Repository<Displacement>, IDisplacementService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public DisplacementService(DbContext context) : base(context)
        {
        }

        public async Task<List<Displacement>> GetDisplacement(int page, int pageSize)
        {
            IQueryable<Displacement> query = _appContext.Displacements.OrderBy(u => u.Id);

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

    }
}
