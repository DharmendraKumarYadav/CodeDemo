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
    public class AttributeService : Repository<Attributes>, IAttributeService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public AttributeService(DbContext context) : base(context)
        {
        }

        public async Task<List<Attributes>> GetAttributes(int page, int pageSize)
        {
            IQueryable<Attributes> query = _appContext.Attributes.OrderBy(u => u.Id);

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

    }
}
