using DAL.Models.Entity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.BikeManagement
{
    public class BikeDealerService : Repository<SaleBike>, IBikeDealerService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public BikeDealerService(DbContext context) : base(context)
        {
        }

    }
}
