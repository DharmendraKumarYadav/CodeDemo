using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeSpecification
{
    public interface IShowRoomService : IRepository<ShowRoom>
    {
        Task<List<ShowRoom>> GetShowRoom(int page, int pageSize,string dealerId);

        Task<List<ShowRoom>> GetShowRoomAdmin(int page, int pageSize, string dealerId);
    }
}
