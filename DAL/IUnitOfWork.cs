using DAL.Repositories;
using DAL.Repositories.Interfaces.BikeManagement;
using DAL.Repositories.Interfaces.BikeSpecification;
using DAL.Repositories.Interfaces.Common;
using DAL.Repositories.Services.BikeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork
    {
        IAttributeService AttributeService { get; }
        ISpecificationService SpecificationService { get; }
        IDisplacementService DisplacementService { get; }
        ICategoryService CategoryService { get; }
        IColourService ColourService { get; }
        IBudgetService BudgetService { get; }
        IBrandService BrandService { get; }
        IBodyStyleService BodyStyleService { get; }

        IBikeSimilarService BikeSimilarService { get; }

        IDocumentService DocumentService { get; }

        #region Bike Management
        IBikeColourService BikeColourService { get; }
        IBikeFeaturesService BikeFeaturesService { get; }
        IBikePriceService BikePriceService { get; }
        IBikeSpecificationsService BikeSpecificationsService { get; }
        IBikeVariantsService BikeVariantsService { get; }
        IBikeService BikeService { get; }

        IBikeImageService BikeImageService { get; }
        #endregion

        ICityService CityService { get; }
        IAreaService AreaService { get; }
        IShowRoomService ShowRoomService { get; }
        ISaleBikeService SaleBikeService { get; }

        IDealerBrokerService DealerBrokerService { get; }
        IDealerEmployeeService DealerEmployeeService { get; }
        IBikeBookingService  BikeBookingService { get; }
        IFeaturedBikeService FeaturedBikeService { get; }

        IBikeUserRatingService BikeUserRatingService { get; }
        IBikeUserRequestService BikeUserRequestService { get; }
        IBikeCityService BikeCityService { get; }

        INotificationService NotificationService { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
