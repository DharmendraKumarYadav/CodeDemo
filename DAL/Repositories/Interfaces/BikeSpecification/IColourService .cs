using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IColourService : IRepository<Colour>
    {
        Task<List<Colour>> GetColour(int page, int pageSize);
    }
}
