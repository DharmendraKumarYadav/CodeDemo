using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface ISpecificationService : IRepository<Specification>
    {
        Task<List<Specification>> GetSpecification(int page, int pageSize);
    }
}
