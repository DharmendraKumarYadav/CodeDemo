﻿using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.BikeManagement
{
    public class BikeFeaturesService : Repository<BikeFeatures>, IBikeFeaturesService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public BikeFeaturesService(DbContext context) : base(context)
        {
        }
    }
}
