using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Common
{
    public class BikeUserRequestService : Repository<BikeUserRequest>, IBikeUserRequestService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public BikeUserRequestService(DbContext context) : base(context)
        {
        }

        public async Task<List<BikeUserRequest>> GetBikeUserRequest(int page, int pageSize,string dealerId)
        {
            IQueryable<BikeUserRequest> query = _appContext.BikeUserRequests
                .Include(m=>m.BikeVariants)
                               .Include(m => m.City)
                .OrderBy(u => u.Id);

            if (!string.IsNullOrEmpty(dealerId))
            {
                query = from request in query
                        join dealer in _appContext.ShowRooms
                        on request.ShowRoomId equals dealer.Id
                        where dealer.UserId == new Guid(dealerId)
                        select request;
            }
            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

    
    }
}
