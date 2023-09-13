using DAL.Models.Entity;
using DAL.Repositories;
using DAL.Repositories.Interfaces.BikeManagement;
using DAL.Repositories.Interfaces.BikeSpecification;
using DAL.Repositories.Interfaces.Common;
using DAL.Repositories.Services.BikeManagement;
using DAL.Repositories.Services.BikeSpecification;
using DAL.Repositories.Services.Common;
using DAL.Repositories.Services.SignalIR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;


        public IDocumentService _documentService;

        public IAttributeService _attributeService;
        public ISpecificationService _specificationService;
        public IDisplacementService _displacementService;
        public ICategoryService _categoryService;
        public IColourService _colourService;
        public IBudgetService _budgetService;
        public IBrandService _brandService;
        public IBodyStyleService _bodyStyleService;

        public IBikeFeaturesService _bikeFeaturesService;

        public IBikePriceService _bikePriceService;
        public IBikeSpecificationsService _bikeSpecificationsService;
        public IBikeVariantsService _bikeVariantsService;
        public IBikeColourService _bikeColourService;
        public IBikeService _bikesService;
        public IBikeImageService _bikeImageService;

        public ICityService _cityService;
        public IAreaService _areaService;


        public IShowRoomService _dealerShowRoomService;
        public ISaleBikeService _dealerBikeService;
        public IDealerBrokerService _dealerBrokerService;
        public IDealerEmployeeService _dealerEmployeeService;


        public IBikeBookingService _bikeBookingService;
        public IFeaturedBikeService _featuredBikeService;


        public IBikeSimilarService _bikeSimilarService;

        public IBikeUserRatingService _bikeUserRatingService;
        public IBikeUserRequestService _bikeUserRequestService;
        public IBikeCityService _bikeCityService;

        public INotificationService _notificationService;

        public UnitOfWork(ApplicationDbContext context, IHubContext<BroadcastHub, IHubClient> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public IAttributeService AttributeService
        {
            get
            {
                if (_attributeService == null)
                    _attributeService = new AttributeService(_context);

                return _attributeService;
            }
        }

        public IDocumentService DocumentService
        {
            get
            {
                if (_documentService == null)
                    _documentService = new DocumentService(_context);

                return _documentService;
            }
        }

        public ISpecificationService SpecificationService
        {
            get
            {
                if (_specificationService == null)
                    _specificationService = new SpecificationService(_context);

                return _specificationService;
            }
        }

        public IDisplacementService DisplacementService
        {
            get
            {
                if (_displacementService == null)
                    _displacementService = new DisplacementService(_context);

                return _displacementService;
            }
        }

        public ICategoryService CategoryService
        {
            get
            {
                if (_categoryService == null)
                    _categoryService = new CategoryService(_context);

                return _categoryService;
            }
        }

        public IColourService ColourService
        {
            get
            {
                if (_colourService == null)
                    _colourService = new ColourService(_context);

                return _colourService;
            }
        }

        public IBudgetService BudgetService
        {
            get
            {
                if (_budgetService == null)
                    _budgetService = new BudgetService(_context);

                return _budgetService;
            }
        }

        public IBrandService BrandService
        {
            get
            {
                if (_brandService == null)
                    _brandService = new BrandService(_context);

                return _brandService;
            }
        }

        public IBodyStyleService BodyStyleService
        {
            get
            {
                if (_bodyStyleService == null)
                    _bodyStyleService = new BodyStyleService(_context);

                return _bodyStyleService;
            }
        }


        #region BIke Management
        public IBikeColourService BikeColourService
        {
            get
            {
                if (_bikeColourService == null)
                    _bikeColourService = new BikeColourService(_context);

                return _bikeColourService;
            }
        }
        public IBikeFeaturesService BikeFeaturesService
        {
            get
            {
                if (_bikeFeaturesService == null)
                    _bikeFeaturesService = new BikeFeaturesService(_context);

                return _bikeFeaturesService;
            }
        }

        public IBikePriceService BikePriceService
        {
            get
            {
                if (_bikePriceService == null)
                    _bikePriceService = new BikePriceService(_context);

                return _bikePriceService;
            }
        }
        public IBikeImageService BikeImageService
        {
            get
            {
                if (_bikeImageService == null)
                    _bikeImageService = new BikeImageService(_context);

                return _bikeImageService;
            }
        }

        public IBikeSpecificationsService BikeSpecificationsService
        {
            get
            {
                if (_bikeSpecificationsService == null)
                    _bikeSpecificationsService = new BikeSpecificationsService(_context);

                return _bikeSpecificationsService;
            }
        }
        public IBikeService BikeService
        {
            get
            {
                if (_bikesService == null)
                    _bikesService = new BikeService(_context);

                return _bikesService;
            }
        }
        public IBikeVariantsService BikeVariantsService
        {
            get
            {
                if (_bikeVariantsService == null)
                    _bikeVariantsService = new BikeVariantsService(_context);

                return _bikeVariantsService;
            }
        }


        #endregion

        public ICityService CityService
        {
            get
            {
                if (_cityService == null)
                    _cityService = new CityService(_context);

                return _cityService;
            }
        }

        public IAreaService AreaService
        {
            get
            {
                if (_areaService == null)
                    _areaService = new AreaService(_context);

                return _areaService;
            }
        }

        public IShowRoomService ShowRoomService
        {
            get
            {
                if (_dealerShowRoomService == null)
                    _dealerShowRoomService = new ShowRoomService(_context);

                return _dealerShowRoomService;
            }
        }
        public ISaleBikeService SaleBikeService
        {
            get
            {
                if (_dealerBikeService == null)
                    _dealerBikeService = new SaleBikeService(_context);

                return _dealerBikeService;
            }
        }
        public IDealerEmployeeService DealerEmployeeService
        {
            get
            {
                if (_dealerEmployeeService == null)
                    _dealerEmployeeService = new DealerEmployeeService(_context);

                return _dealerEmployeeService;
            }
        }


        public IFeaturedBikeService FeaturedBikeService
        {
            get
            {
                if (_featuredBikeService == null)
                    _featuredBikeService = new FeaturedBikeService(_context);

                return _featuredBikeService;
            }
        }


        public IBikeBookingService BikeBookingService
        {
            get
            {
                if (_bikeBookingService == null)
                    _bikeBookingService = new BikeBookingService(_context);

                return _bikeBookingService;
            }
        }


        public IBikeSimilarService BikeSimilarService
        {
            get
            {
                if (_bikeSimilarService == null)
                    _bikeSimilarService = new BikeSimilarService(_context);

                return _bikeSimilarService;
            }
        }

        public IBikeCityService BikeCityService
        {
            get
            {
                if (_bikeCityService == null)
                    _bikeCityService = new BikeCityService(_context);

                return _bikeCityService;
            }
        }

        public IBikeUserRequestService BikeUserRequestService
        {
            get
            {
                if (_bikeUserRequestService == null)
                    _bikeUserRequestService = new BikeUserRequestService(_context);

                return _bikeUserRequestService;
            }
        }
        public IBikeUserRatingService BikeUserRatingService
        {
            get
            {
                if (_bikeUserRatingService == null)
                    _bikeUserRatingService = new BikeUserRatingService(_context);

                return _bikeUserRatingService;
            }
        }

        public IDealerBrokerService DealerBrokerService
        {
            get
            {
                if (_dealerBrokerService == null)
                    _dealerBrokerService = new DealerBrokerService(_context);
                return _dealerBrokerService;
            }
        }
        public INotificationService NotificationService
        {
            get
            {
                if (_notificationService == null)
                    _notificationService = new NotificationService(_context,_hubContext);
                return _notificationService;
            }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
