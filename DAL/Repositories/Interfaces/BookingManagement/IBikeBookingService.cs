using BDB;
using Core.Models;
using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IBikeBookingService : IRepository<BikeBooking>
    {
        Task<List<BikeBooking>> GetBikeBookings(int page, int pageSize,string dealerId);
        Task<List<BikeBookingCustomerModel>> GetMyBikeBookings(string userId);

        Task<BikeBookingCustomerDetailsModel> GetBikeBookingDetail(string bookingId);

        Task<List<DashboardMonthlyModel>> GetMonthlyBooking(int year,string dealerId);
        Task<int> GetCountBooking(int year,string dealerId);
    }
}
