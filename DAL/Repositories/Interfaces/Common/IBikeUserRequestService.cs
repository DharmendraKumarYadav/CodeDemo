using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IBikeUserRequestService : IRepository<BikeUserRequest>
    {
        Task<List<BikeUserRequest>> GetBikeUserRequest(int page, int pageSize,string dealerId);
    }
}
