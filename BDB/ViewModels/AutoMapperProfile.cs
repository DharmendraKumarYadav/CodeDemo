using AutoMapper;
using BDB.ViewModels.Account;
using BDB.ViewModels.Attribute;
using BDB.ViewModels.Bike;
using BDB.ViewModels.BikeAttribute;
using BDB.ViewModels.Common;
using Core.Enums;
using Core.Models;
using DAL.Core;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using System.Linq;

namespace BDB.ViewModels
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserViewModel>()
                   .ForMember(d => d.Role, map => map.Ignore())
                     .ForMember(d => d.DealerIds, map => map.MapFrom(src => src.Brokers.Select(r => r.DealerId.ToString())));
            CreateMap<UserViewModel, User>()

                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            //Mine Start 

            CreateMap<User, UserRegisterModel>()
           .ForMember(d => d.Role, map => map.Ignore());
            CreateMap<UserRegisterModel, User>();

            CreateMap<User, UserUpdateModel>()
           .ForMember(d => d.Role, map => map.Ignore());
            CreateMap<UserUpdateModel, User>()
           .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<User, UserCurrentUpdateModel>();
            CreateMap<UserCurrentUpdateModel, User>()
           .ForMember(d => d.Id, map => map.Ignore());


            CreateMap<Specification, SpecificationViewModel>();
            CreateMap<Specification, SpecificationModel>();
            CreateMap<SpecificationModel, Specification>();

            CreateMap<DAL.Models.Entity.Attributes, AttributeModel>();
            CreateMap<AttributeModel, DAL.Models.Entity.Attributes>();

            CreateMap<Colour, ColourModel>();
            CreateMap<ColourModel, Colour>();

            CreateMap<Brand, BrandModel>();
            CreateMap<BrandModel, Brand>();

            CreateMap<Brand, BrandViewModel>();

            CreateMap<Budget, BudgetModel>();
            CreateMap<BudgetModel, Budget>();


            CreateMap<Displacement, DisplacementModel>();
            CreateMap<DisplacementModel, Displacement>();

            CreateMap<Category, CategoryModel>();
            CreateMap<CategoryModel, Category>();

            CreateMap<BodyStyle, BodyStyleModel>();
            CreateMap<BodyStyleModel, BodyStyle>();

            //Mine End



            CreateMap<User, UserEditViewModel>()
                .ForMember(d => d.Role, map => map.Ignore());
            CreateMap<UserEditViewModel, User>()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));





            CreateMap<User, UserPatchViewModel>()
                .ReverseMap();

            CreateMap<Role, RoleViewModel>()
                .ForMember(d => d.UsersCount, map => map.MapFrom(s => s.UserRoles != null ? s.UserRoles.Count : 0))
                .ReverseMap();
            CreateMap<RoleViewModel, Role>()
                .ForMember(d => d.Id, map => map.Condition(src => src.Id != null));

            CreateMap<RoleClaim, ClaimViewModel>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
                .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
                .ReverseMap();


            CreateMap<DAL.Models.Entity.Bike, BikeViewModel>()
                .ForMember(des => des.BodyStyle, opt => opt.MapFrom(src => src.BodyStyle.Name))
                   .ForMember(des => des.Category, opt => opt.MapFrom(src => src.Category.Name))
                   .ForMember(m => m.IsElectricBike, opt => opt.MapFrom(m => m.IsElectricBike ? "1" : "0"));
            CreateMap<BikeModel, DAL.Models.Entity.Bike>()
                 .ForMember(m => m.IsElectricBike, opt => opt.MapFrom(m => m.IsElectricBike=="1" ? true : false));
            CreateMap<BikeVariantModel, DAL.Models.Entity.BikeVariant>();
            CreateMap<BikeVariant, BikeVariantModel>();
            CreateMap<BikeColour, DAL.Models.Entity.BikeColour>();

            CreateMap<DAL.Models.Entity.Bike, BikeModel>()
                .ForMember(m => m.IsElectricBike, opt => opt.MapFrom(m => m.IsElectricBike ? "1" : "0"));


            CreateMap<ShowRoomModel, ShowRoom>();


            CreateMap<ShowRoom, ShowRoomViewModel>();

            CreateMap<DealerBikeModel, SaleBike>();
            CreateMap<SaleBike, DealerBikeModel>();



            CreateMap<SimilarBikeRequest, BikeSimilar>();

            CreateMap<SaleBike, DealerBikeViewModel>().ForMember(m => m.VariantName, opt => opt.MapFrom(m => m.BikeVariants.Name));

            //CreateMap<Dealer, DealerViewModel>().ForMember(des => des.Area, opt => opt.MapFrom(src => src.Area.Name + "(" + src.Area.City.Name + ")"))
            //    .ForMember(des => des.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.FirstName + " " + src.LastName)); 



            CreateMap<City, CityModel>();
            CreateMap<CityModel, City>();


            CreateMap<BikeSubscribeModelRequest, BikeUserRequest>();
            CreateMap<BikeUserRequest, BikeSubscribeModel>().ForMember(m => m.BikeName, opt => opt.MapFrom(m => m.BikeVariants.Name))
                .ForMember(m => m.City, opt => opt.MapFrom(m => m.City.Name));

            CreateMap<BikeUserRating, BikeUserRatingModel>().ForMember(m => m.BikeName, opt => opt.MapFrom(m => m.Bike.Name));
            CreateMap<BikeUserRatingModel, BikeUserRating>();



            CreateMap<Area, AreaModel>().ForMember(des => des.City, opt => opt.MapFrom(src => src.City.Name));
            CreateMap<AreaModel, Area>();

            CreateMap<BikeBooking, BikeBookingModel>()
                .ForMember(m => m.Name, opt => opt.MapFrom(src => (src.FullName )))
                .ForMember(m => m.BookingDate, opt => opt.MapFrom(src => (src.CreatedDate.ToShortDateString())))
                .ForMember(m => m.PaymentStatus, opt => opt.MapFrom(src => ((PaymentStatus)src.PaymentStatusId).ToString()))
                      .ForMember(m => m.BookingStatus, opt => opt.MapFrom(src => ((BookingStatus)src.BookingStatusId).ToString()));
            CreateMap<BikeBookingModel, BikeBooking>();
        }
    }
}

