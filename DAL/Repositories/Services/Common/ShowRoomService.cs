using Core.Enums;
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
    public class ShowRoomService : Repository<ShowRoom>, IShowRoomService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public ShowRoomService(DbContext context) : base(context)
        {
        }

        public async Task<List<ShowRoom>> GetShowRoom(int page, int pageSize,string dealerId)
        {
            IQueryable<ShowRoom> query = _appContext.ShowRooms.Where(m => m.IsActive && m.Status== Convert.ToInt32(ShowRoomStatusEnum.Approved));

            if (!string.IsNullOrEmpty(dealerId))
            {
                query = query.Where(m => m.UserId == new Guid(dealerId));
            }

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }
        public async Task<List<ShowRoom>> GetShowRoomAdmin(int page, int pageSize, string dealerId)
        {
            IQueryable<ShowRoom> query = _appContext.ShowRooms.Where(m => m.IsActive).OrderByDescending(m=>m.Status== (int)ShowRoomStatusEnum.Pending);

            if (!string.IsNullOrEmpty(dealerId))
            {
                query = query.Where(m => m.UserId == new Guid(dealerId));
            }

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

    }
}
