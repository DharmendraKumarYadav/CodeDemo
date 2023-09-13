using BDB;
using Core;
using Core.Enums;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.Repositories.Services.BikeManagement
{
    public class BikeService : Repository<Bike>, IBikeService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public BikeService(DbContext context) : base(context)
        {
        }

        #region Getting Bikes
        public async Task<BikeFeaturedList> GetFeaturedBikes(string cityId)
        {
            List<FeaturedBike> featuredBikesList;
            BikeFeaturedList featuredBikes = new BikeFeaturedList();
            List<BikeModelList> list = new List<BikeModelList>();

            var query = _appContext.FeaturedBikes
                         .Include(m => m.Bike).ThenInclude(m => m.BikeColours).ThenInclude(m => m.Colour).ThenInclude(m => m.Document)
                 .Include(m => m.Bike).ThenInclude(m => m.BodyStyle)
                  .Include(m => m.Bike).ThenInclude(m => m.Brand)
                      .Include(m => m.Bike).ThenInclude(m => m.Category)

                             .Include(m => m.Bike).ThenInclude(m => m.BikeVariants).ThenInclude(m => m.BikePrices).ThenInclude(m => m.City)
                             .Include(m => m.Bike).ThenInclude(m => m.BikeImages).ThenInclude(m => m.Documents).AsQueryable();

            if (cityId !=null)
            {
                var cityIds = cityId.Split(',').ToList();
                featuredBikesList = await ((from queryData in query
                                            join bikeCity in _appContext.BikeCity on queryData.BikeId equals bikeCity.BikeId
                                            where cityIds.Contains(bikeCity.CityId.ToString())
                                            select queryData)
                          ).ToListAsync();
            }
            else
            {
                featuredBikesList = await query.ToListAsync();
            }

            foreach (var item in featuredBikesList)
            {
                list.Add(new BikeModelList
                {
                    Id = item.Bike.Id,
                    Name = item.Bike.Name,
                    BasicSpecification = item.Bike.BikeVariants.FirstOrDefault()?.Specification,
                    Price = item.Bike.BikeVariants.FirstOrDefault()?.BikePrices?.FirstOrDefault()?.ExShowRoomPrice,
                    ShowRoomName = "Ex-Show Room:" + item.Bike.BikeVariants?.FirstOrDefault()?.BikePrices?.FirstOrDefault()?.City?.Name,
                    Images = GetBiksImages(item.Bike.BikeImages),
                    Colours=Getcolours(item.Bike.BikeColours.ToList()),
                    BodyStyle=item.Bike.BodyStyle.Name,
                    Category=item.Bike.Category.Name,
                    Brand=item.Bike.Brand.Name,
                    Displacement=item.Bike.Displacement.ToString(),
                    TypeId = item.FeatureType,
                    IsElectricBike=item.Bike.IsElectricBike
                });
            }
            featuredBikes.PopularBikes = list.Where(m => m.TypeId == (int)(BikeTypeEnum.Popular)).ToList();
            featuredBikes.NewLaunches = list.Where(m => m.TypeId == (int)(BikeTypeEnum.NewLaunch)).ToList();
            featuredBikes.Upcoming = list.Where(m => m.TypeId == (int)(BikeTypeEnum.Upcoming)).ToList();
            featuredBikes.ElectricBike = list.Where(m => m.IsElectricBike==true).ToList();

            return featuredBikes;
        }
        public async Task<PagedList<Bike>> GetFilteredBikes(SearchFilterModel searchFilter, string? cityId)
        {
            List<BikeModelList> list = new List<BikeModelList>();
            var query = _appContext.Bikes
                .Include(m => m.BikeColours).ThenInclude(m => m.Colour).ThenInclude(m => m.Document)
                .Include(m => m.BodyStyle)
                .Include(m=>m.BikeCity)
                  .Include(m => m.Brand)
                      .Include(m => m.Category)
                           .Include(m => m.FeaturedBikes)
                .Include(m => m.BikeVariants).ThenInclude(m => m.BikePrices).ThenInclude(m => m.City)
                .Include(m => m.BikeImages).ThenInclude(m => m.Documents).AsQueryable();


            if (cityId !=null)
            {
                var cityIds = cityId.Split(',').ToList();
                 query = query.Where(m => m.BikeCity.Any(m => cityIds.Contains(m.CityId.ToString())));
                //query = ((from queryData in query
                //          join bikeCity in _appContext.BikeCity on queryData.Id equals bikeCity.BikeId
                //          where cityIds.Contains(bikeCity.CityId.ToString()) 
                //          select queryData)
                //          ).AsQueryable();
            }
            if (searchFilter != null)
            {

                if (!string.IsNullOrEmpty(searchFilter?.filters.category))
                {
                    query = (from queryData in query where searchFilter.filters.category.Contains(queryData.CategoryId.ToString())  select queryData);
                }
                if (!string.IsNullOrEmpty(searchFilter?.type.ToString()))
                {
                  
                    if (searchFilter.type == (int)BikeTypeEnum.ElectricBikes)//For Electric Type
                    {
                        query = (from queryData in query where queryData.IsElectricBike select queryData);
                    }
                    else if(searchFilter.type != (int)BikeTypeEnum.All)
                    {
                        query = (from queryData in query
                                 join fb in _appContext.FeaturedBikes on queryData.Id equals fb.BikeId
                                 into studentInfo
                                 from students in studentInfo.DefaultIfEmpty()
                                 where students.FeatureType.ToString() == searchFilter.type.ToString()
                                 select queryData);
                    }

                }
                //else {
                   
                //    query = (from queryData in query
                //             join fb in _appContext.FeaturedBikes on queryData.Id equals fb.BikeId
                //              into studentInfo
                //             from students in studentInfo.DefaultIfEmpty()
                //             orderby students.FeatureType.ToString()
                //             select queryData);
                //}

                if (!string.IsNullOrEmpty(searchFilter?.filters.brand))
                {
                    query = query.Where(m => searchFilter.filters.brand.Contains(m.BrandId.ToString()));
                }
                if (!string.IsNullOrEmpty(searchFilter?.filters.bodyStyle))
                {
                    query = query.Where(m => searchFilter.filters.bodyStyle.Contains(m.BodyStyleId.ToString()));
                }
                if (!string.IsNullOrEmpty(searchFilter?.filters.budget))
                {
                    var data = await _appContext.Budgets.Where(m => m.Id == Convert.ToInt32(searchFilter.filters.budget)).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        if (data.Operator == "=")
                        {
                            query = query.Where(m => Convert.ToDecimal(m.Price) == Convert.ToDecimal(data.Amount));
                        }
                        else if (data.Operator == ">")
                        {
                            query = query.Where(m => Convert.ToDecimal(m.Price) > Convert.ToDecimal(data.Amount));
                        }
                        else if (data.Operator == "<")
                        {
                            query = query.Where(m => Convert.ToDecimal(m.Price) < Convert.ToDecimal(data.Amount));
                        }
                    }

                }
                if (!string.IsNullOrEmpty(searchFilter?.filters.displacement))
                {
                    var data = await _appContext.Displacements.Where(m => m.Id == Convert.ToInt32(searchFilter.filters.displacement)).FirstOrDefaultAsync();
                    if (data != null)
                    {
                        if (data.Operator == "=")
                        {
                            query = query.Where(m => Convert.ToDecimal(m.Displacement) == Convert.ToDecimal(data.Value));
                        }
                        else if (data.Operator == ">")
                        {
                            query = query.Where(m => Convert.ToDecimal(m.Displacement) > Convert.ToDecimal(data.Value));
                        }
                        else if (data.Operator == "<")
                        {
                            query = query.Where(m => Convert.ToDecimal(m.Displacement) < Convert.ToDecimal(data.Value));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(searchFilter?.sort))
                {
                    query = searchFilter.sort == "name_desc" ? query.OrderByDescending(m => m.Name) : query.OrderBy(m => m.Name);
                }
            }
            //query = query.Distinct(new UserEqualityComparer()).AsQueryable();

            return await PagedList<Bike>.ToPagedList(query, searchFilter.page, searchFilter.limit);

        }

        public async Task<List<BikeSearchData>> GetSearchData(string query)
        {
            List<BikeSearchData> searchList = new List<BikeSearchData>();
            if (!string.IsNullOrEmpty(query))
            {
                var bike = await _appContext.Bikes.Where(m => m.Name.Contains(query)).ToListAsync();
                if (bike.Count > 0)
                {
                    foreach (var item in bike)
                    {
                        searchList.Add(new BikeSearchData
                        {
                            Id = item.Id,
                            Name = item.Name,
                            SearchType = "In Bikes"
                        });
                    }
                }
                var brands = await _appContext.Brands.Where(m => m.Name.Contains(query)).ToListAsync();
                if (brands.Count > 0)
                {
                    foreach (var item in brands)
                    {
                        searchList.Add(new BikeSearchData
                        {
                            Id = item.Id,
                            Name = item.Name,
                            SearchType = "In Brand"
                        });
                    }
                }
                var bodyStyles = await _appContext.BodyStyles.Where(m => m.Name.Contains(query)).ToListAsync();
                if (bodyStyles.Count > 0)
                {
                    foreach (var item in bodyStyles)
                    {
                        searchList.Add(new BikeSearchData
                        {
                            Id = item.Id,
                            Name = item.Name,
                            SearchType = "In Body Style"
                        });
                    }
                }
            }
            else
            {
                var bike = await _appContext.Bikes.OrderByDescending(m => m.Id).Take(10).ToListAsync();
                if (bike.Count > 0)
                {
                    foreach (var item in bike)
                    {
                        searchList.Add(new BikeSearchData
                        {
                            Id = item.Id,
                            Name = item.Name,
                            SearchType = "In Bikes"
                        });
                    }
                }
            }

            return searchList;
        }

        #endregion

        public async Task<List<Bike>> GetBikes(int page, int pageSize)
        {
            IQueryable<Bike> query = _appContext.Bikes.OrderBy(u => u.Id)
                .Include(m => m.Brand)
                .Include(m => m.BodyStyle)
                .Include(m => m.Category);
            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<BikeDetailModel> GetBikeDetail(int bikeId)
        {
            var result = new BikeDetailModel();
            var bike = await _appContext.Bikes.Where(m => m.Id == bikeId)
                           .Include(m => m.Brand)
                           .Include(m => m.Category)
                           .Include(m => m.BodyStyle)
                           .Include(m => m.BikeColours).ThenInclude(m => m.Colour).ThenInclude(m => m.Document)
                           .Include(m => m.BikeImages).ThenInclude(m => m.Documents)
                           //.Include(m => m.Dealer).ThenInclude(m => m.Area).ThenInclude(m => m.City)
                           .Include(m => m.BikeVariants).ThenInclude(m => m.BikePrices).ThenInclude(m => m.City)
                           .Include(m => m.BikeVariants).ThenInclude(m => m.BikeSpecifications).ThenInclude(m => m.Attributes)
                           .Include(m => m.BikeVariants).ThenInclude(m => m.BikeSpecifications).ThenInclude(m => m.Specification)
                           .Include(m => m.BikeVariants).ThenInclude(m => m.BikeFeatures).ThenInclude(m => m.Attributes)
                           .FirstOrDefaultAsync();


            result.Id = bikeId;
            result.BikeName = bike.Name;
            result.ShortDescription = bike.ShortDescription;
            result.LongDescription = bike.LongDescription;
            result.BodyStyle = bike.BodyStyle.Name;
            result.Category = bike.Category.Name;
            result.Brand = bike.Brand.Name;
            result.Displacement = bike.Displacement.ToString();
            result.IsElectricBike = bike.IsElectricBike;
            result.DocumentId = bike.DocumentId;
            //Bike Coulure
            List<BikeColourModelList> colourModelLists = new List<BikeColourModelList>();
            foreach (var item in bike.BikeColours)
            {
                colourModelLists.Add(new BikeColourModelList
                {
                    Id = item.ColourId,
                    Name = item.Colour.Name,
                    Image = item.Colour?.Document?.Data != null ? GetDocumentFromByteArray(item.Colour.Document.Data) : null,
                });
            }
            result.Colours = colourModelLists;

            //Bike Images:
            List<BikeImagesList> imageList = new List<BikeImagesList>();
            foreach (var item in bike.BikeImages)
            {
                imageList.Add(new BikeImagesList
                {
                    Id = item.DocumentId,
                    Image = item.Documents?.Data != null ? GetDocumentFromByteArray(item.Documents.Data) : null,
                });
            }
            result.BikeImages = imageList;


            //Bike Variants:
            List<BikeVariantsModelList> bikeVariants = new List<BikeVariantsModelList>();
            foreach (var item in bike.BikeVariants)
            {
                bikeVariants.Add(new BikeVariantsModelList
                {
                    VariantId = item.Id,
                    Name = item.Name,
                    Specification = item.Specification,
                    BikePrices = GetPriceList(item.BikePrices),
                    BikeFeatures = GetFeaturesData(item.BikeFeatures),
                    BikeSpecifications =await GetBikeSepecification(item.BikeSpecifications)
                }); 
            }
            result.BikeVariants = bikeVariants;

            //Bike Dealer:
            //List<BikeDealerModelList> bikeDealers = new List<BikeDealerModelList>();
            //foreach (var item in bike.BikeDealers)
            //{
            //    bikeDealers.Add(new BikeDealerModelList
            //    {
            //        Id = item.Id,
            //        FirstName = item.Dealer.FirstName,
            //        MiddleName = item.Dealer.MiddleName,
            //        LastName = item.Dealer.LastName,
            //        AddressLine1 = item.Dealer.AddressLine1,
            //        AddressLine2 = item.Dealer.AddressLine2,
            //        Area = item.Dealer.Area.Name,
            //        PhoneNumber = item.Dealer.PhoneNumber,
            //        EmailId = item.Dealer.EmailId,
            //        Latitude = item.Dealer.Latitude,
            //        Longitude = item.Dealer.Longitude,
            //        MobileNumber = item.Dealer.MobileNumber,
            //        City = item.Dealer.Area.City.Name,
            //        PinCode = item.Dealer.Area.PinCode,
            //        CityId = item.Dealer.Area.City.Id
            //    });
            //}
            result.BikeDealers = null;
            return result;
        }


        #region Private Method
        private string GetDocumentFromByteArray(byte[] bytes)
        {
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        private string[] GetBiksImages(ICollection<BikeImage> bikeImages)
        {
            var bikeImage = new List<string>();
            foreach (var image in bikeImages)
            {
                bikeImage.Add(GetDocumentFromByteArray(image.Documents.Data));
            }
            return bikeImage.ToArray();
        }

        private List<BikeAttributeModelList> GetFeaturesData(ICollection<BikeFeatures> bikeFeatures)
        {

            List<BikeAttributeModelList> list = new List<BikeAttributeModelList>();
            foreach (var item in bikeFeatures)
            {
                list.Add(new BikeAttributeModelList
                {
                    Id = item.Id,
                    Name = item.Attributes.Name,
                    Value = item.AttributeValue

                });
            }
            return list;
        }

        private async Task<List<BikeSpecificationModelList>> GetBikeSepecification(ICollection<BikeSpecifications> bikeSpecifications)
        {

            List<BikeSpecificationModelList> specifcationList = new List<BikeSpecificationModelList>();
            foreach (var item in bikeSpecifications)
            {
                specifcationList.Add(new BikeSpecificationModelList
                {
                    Id = item.Id,
                    SpecifcaitionName = item.Specification.Name,
                    Name = item.Attributes.Name,
                    Value = item.AttributeValue,
                    Image = await GetDocumentImage(item.Specification.ImageId) 
                });
            }
            return specifcationList;
        }

        private List<BikePriceList> GetPriceList(ICollection<BikePrice> bikePrices)
        {
            List<BikePriceList> priceList = new List<BikePriceList>();
            foreach (var item in bikePrices)
            {
                priceList.Add(new BikePriceList
                {
                    Price = item.Price,
                    BookingAmount = item.BookingAmount,
                    CityId = item.CityId,
                    City = item.City.Name,
                    ExShowRoomPrice = item.ExShowRoomPrice,
                    Id = item.Id,
                    InsurancePrice = item.InsurancePrice,
                    RTOPrice = item.RTOPrice

                });
            }
            return priceList;
        }

        #endregion

        private List<BikeColourModelList> Getcolours(List<BikeColour> bikeColours)
        {
            List<BikeColourModelList> colourModelLists = new List<BikeColourModelList>();
            foreach (var item in bikeColours)
            {
                colourModelLists.Add(new BikeColourModelList
                {
                    Id = item.ColourId,
                    Name = item.Colour.Name,
                    Image = item.Colour?.Document?.Data != null ? GetDocumentFromByteArray(item.Colour.Document.Data) : null,
                });
            }
            return colourModelLists;
        }

        public async Task<int> GetBikeCount()
        {
            return await _appContext.Bikes.Where(m => m.IsActive && !m.IsDeleted).CountAsync();
        }

        public async Task<int> GetSalesBikeCount(string dealerId)
        {
            var query = _appContext.SaleBikes.Include(m => m.ShowRoom).Where(m => m.IsActive && !m.IsDeleted);
            if (!string.IsNullOrEmpty(dealerId)) {
                query = query.Where(m => m.ShowRoom.UserId.ToString() == dealerId);
            }
            return await query.CountAsync();
        }
        public async Task<string> GetDocumentImage(int id)
        {

           
            var dataObject = await _appContext.Documents.FirstOrDefaultAsync(m => m.Id == id);
            if (dataObject != null)
            {
                return GetDocumentFromByteArray(dataObject.Data);
            }
         

            return ""; 
        }
    }
    public class UserEqualityComparer : IEqualityComparer<Bike>
    {
        public bool Equals(Bike x, Bike y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Bike obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
