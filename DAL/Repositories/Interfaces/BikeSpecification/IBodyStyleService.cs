using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IBodyStyleService : IRepository<BodyStyle>
    {
        Task<List<BodyStyle>> GetBodyStyle(int page, int pageSize);
    }
}
