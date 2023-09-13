using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IBudgetService : IRepository<Budget>
    {
        Task<List<Budget>> GetBudget(int page, int pageSize);
    }
}
