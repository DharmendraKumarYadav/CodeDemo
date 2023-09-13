using DAL.Models.Entity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Common
{
    public class DealerEmployeeService : Repository<DealerEmployee>, IDealerEmployeeService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public DealerEmployeeService(DbContext context) : base(context)
        {
        }

        public async Task<List<DealerEmployee>> GetDealerEmployee(int page, int pageSize)
        {
            IQueryable<DealerEmployee> query = _appContext.DealerEmployees.OrderBy(u => u.Id);

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

    }
}
