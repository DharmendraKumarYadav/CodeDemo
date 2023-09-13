using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IAreaService : IRepository<Area>
    {
        Task<List<Area>> GetArea(int page, int pageSize);
    }
}
