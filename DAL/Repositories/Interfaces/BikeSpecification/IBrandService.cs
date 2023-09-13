using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IBrandService : IRepository<Brand>
    {
        Task<List<Brand>> GetBrand(int page, int pageSize);
    }
}
