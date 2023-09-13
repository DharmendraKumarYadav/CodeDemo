using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface ICityService : IRepository<City>
    {
        Task<List<City>> GetCity(int page, int pageSize);
    }
}
