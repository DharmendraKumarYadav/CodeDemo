using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IAttributeService : IRepository<Attributes>
    {
        Task<List<Attributes>> GetAttributes(int page, int pageSize);
    }
}
