using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface ICategoryService : IRepository<Category>
    {
        Task<List<Category>> GetCategory(int page, int pageSize);
    }
}
