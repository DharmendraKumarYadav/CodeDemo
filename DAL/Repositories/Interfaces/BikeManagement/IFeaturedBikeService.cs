﻿using BDB;
using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IFeaturedBikeService : IRepository<FeaturedBike>
    {
        Task<List<FeaturedBikeTypeModel>> GetFeaturedBikes(int page, int pageSize);
    }
}
