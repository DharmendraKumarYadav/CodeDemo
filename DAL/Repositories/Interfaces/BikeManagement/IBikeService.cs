using BDB;
using Core;
using Core.Enums;
using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.BikeManagement
{
    public interface IBikeService : IRepository<Bike>
    {
        Task<List<Bike>> GetBikes(int page, int pageSize);
        Task<BikeFeaturedList> GetFeaturedBikes(string? cityId);

        Task<PagedList<Bike>> GetFilteredBikes(SearchFilterModel searchFilter, string? cityId);

        Task<BikeDetailModel> GetBikeDetail(int bikeId);
        Task<List<BikeSearchData>> GetSearchData(string query);


        Task<int> GetBikeCount();
        Task<int> GetSalesBikeCount(string dealerId);

    }
}
