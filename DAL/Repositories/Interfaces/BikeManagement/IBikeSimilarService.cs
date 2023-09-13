using BDB;
using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IBikeSimilarService : IRepository<BikeSimilar>
    {
        Task<List<SimilarBikeModel>> GetSimilarBikes(int page, int pageSize, int bikeId);
    }
}
