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
    public class BudgetService : Repository<Budget>, IBudgetService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public BudgetService(DbContext context) : base(context)
        {
        }

        public async Task<List<Budget>> GetBudget(int page, int pageSize)
        {
            IQueryable<Budget> query = _appContext.Budgets.OrderBy(u => u.Id);

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

    }
}
