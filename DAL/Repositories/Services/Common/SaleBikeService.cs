using Core.Enums;
using Core.Models;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.BikeSpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAL.Repositories.Services.Common
{
    public class SaleBikeService : Repository<SaleBike>, ISaleBikeService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public SaleBikeService(DbContext context) : base(context)
        {
        }


        public async Task<List<SaleBike>> GetSaleBike(int page, int pageSize, string userId)
        {
            IQueryable<SaleBike> query = _appContext.SaleBikes
                .Include(m => m.BikeVariants).ThenInclude(m => m.Bike)
                .Include(m => m.ShowRoom).ThenInclude(m => m.Area).ThenInclude(m => m.City)
                .Include(m => m.ShowRoom).ThenInclude(m => m.User);

            query = query.Where(m => m.ShowRoom.Status == Convert.ToInt32(ShowRoomStatusEnum.Approved));

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(m => m.ShowRoom.UserId == new Guid(userId));
            }

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<List<SaleBike>> GetBookedBike(int page, int pageSize, string userId)
        {
            IQueryable<SaleBike> query = _appContext.SaleBikes.Where(m => m.Status == 2)
                .Include(m => m.BikeVariants).ThenInclude(m => m.Bike)
                .Include(m => m.ShowRoom).ThenInclude(m => m.Area).ThenInclude(m => m.City)
                .Include(m => m.ShowRoom).ThenInclude(m => m.User);

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(m => m.ShowRoom.UserId == new Guid(userId));
            }

            if (page != -1)
                query = query.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                query = query.Take(pageSize);

            return await query.ToListAsync();
        }


        public async Task<List<SaleBike>> GetBrokerRequestedBike(int page, int pageSize, string dealerId)
        {
            IQueryable<SaleBike> query = _appContext.SaleBikes.Where(m => m.TransferStatus == (int)(TransferStatus.Requested))
                .Include(m => m.BikeVariants).ThenInclude(m => m.Bike)
                .Include(m => m.ShowRoom).ThenInclude(m => m.Area).ThenInclude(m => m.City)
                .Include(m => m.ShowRoom).ThenInclude(m => m.User);


            query = query.Where(m => m.ShowRoom.Status == Convert.ToInt32(ShowRoomStatusEnum.Approved));
            var bikeRequest = (from dataBike in query
                               join dataUser in _appContext.Users on dataBike.TransferRequestedBy equals dataUser.Id.ToString().ToLower()
                               select new SaleBike
                               {
                                   Id = dataBike.Id,
                                   ExShowRoomPrice = dataBike.ExShowRoomPrice,
                                   TransferRequestedBy = dataBike.TransferRequestedBy,
                                   Availbility = dataBike.Availbility,
                                   BikeVariants = dataBike.BikeVariants,
                                   ShowRoom = dataBike.ShowRoom,
                                   BikeColurId = dataBike.BikeColurId,
                                   BikeVariantsId = dataBike.BikeVariantsId,
                                   BookingAmount = dataBike.BookingAmount,
                                   ChesisNumber = dataBike.ChesisNumber,
                                   CreatedBy = dataBike.CreatedBy,
                                   CreatedDate = dataBike.CreatedDate,
                                   EngineNumber = dataBike.EngineNumber,
                                   InsurancePrice = dataBike.InsurancePrice,
                                   IsActive = dataBike.IsActive,
                                   IsDeleted = dataBike.IsDeleted,
                                   IsTransferable = dataBike.IsTransferable,
                                   NoOfDay = dataBike.NoOfDay,
                                   IsTransferred = dataBike.IsTransferred,
                                   Price = dataBike.Price,
                                   Remarks = dataBike.Remarks,
                                   ReturnBy = dataBike.ReturnBy,
                                   ReturnDate = dataBike.ReturnDate,
                                   RTOPrice = dataBike.RTOPrice,
                                   ShowRoomId = dataBike.ShowRoomId,
                                   TransferAuthorizeBy = dataBike.TransferAuthorizeBy,
                                   TransferAuthorizeDate = dataBike.TransferAuthorizeDate,
                                   UpdatedDate = dataBike.UpdatedDate,
                                   UpdatedBy = dataBike.UpdatedBy,
                                   TransferRequestedDate = dataBike.TransferRequestedDate,
                                   TransferStatus = dataBike.TransferStatus,
                                   RequestorName = dataUser.FullName
                               });

            if (!string.IsNullOrEmpty(dealerId))
            {
                bikeRequest = bikeRequest.Where(m => m.ShowRoom.UserId == new Guid(dealerId));
            }

            if (page != -1)
                bikeRequest = bikeRequest.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                bikeRequest = bikeRequest.Take(pageSize);

            return await bikeRequest.ToListAsync();
        }



        public async Task<List<SaleBike>> GetDealerBikeAvailableForSale(string dealerId)
        {
            IQueryable<SaleBike> query = _appContext.SaleBikes.Where(m => m.IsTransferable && m.Status == 1 && (m.TransferStatus == (int)TransferStatus.Nothing || m.TransferStatus == (int)TransferStatus.Requested || m.TransferStatus == (int)TransferStatus.Returned))
                .Include(m => m.BikeVariants).ThenInclude(m => m.Bike)
                .Include(m => m.ShowRoom).ThenInclude(m => m.Area).ThenInclude(m => m.City)
                .Include(m => m.ShowRoom).ThenInclude(m => m.User);



            query = query.Where(m => m.ShowRoom.Status == Convert.ToInt32(ShowRoomStatusEnum.Approved));
            if (!string.IsNullOrEmpty(dealerId))
            {
                query = query.Where(m => m.ShowRoom.UserId == new Guid(dealerId));
            }

            return await query.ToListAsync();
        }


        public async Task<List<AvailableShowRoomBikes>> GetAllBikeByCityAndVariant(int cityId, int variantId)
        {
            var availableList = new List<AvailableShowRoomBikes>();

            var dealerBikes = _appContext.SaleBikes.Where(m => m.IsActive && m.BikeVariantsId == variantId && m.Status == 1)
                            .Include(m => m.BikeVariants)
                            .Include(m => m.ShowRoom).ThenInclude(m=>m.User)
                            .Include(m => m.ShowRoom).ThenInclude(m => m.Area).ThenInclude(m=>m.City).AsQueryable();


            if (cityId != 0)
            {
                dealerBikes = dealerBikes.Where(m => m.ShowRoom.Area.CityId == cityId);
            }
          var  query = dealerBikes.OrderByDescending(m => m.ShowRoom.User.LocalOrder).ToList();


            foreach (var item in query)
            {
                availableList.Add(new AvailableShowRoomBikes
                {
                    
                    DealerBikeId = item.Id,
                    ChesisNumber = item.ChesisNumber,
                    BookingAmount = item.BookingAmount,
                    EngineNumber = item.EngineNumber,
                    ExShowRoomAmount = item.ExShowRoomPrice,
                    RtoAmount = item.RTOPrice,
                    InsuranceAmount = item.InsurancePrice,
                    VariantName = item.BikeVariants.Name,
                    Specification = item.BikeVariants.Specification,
                    ShowRoomId = item.ShowRoomId,
                    ShowRoomName = item.ShowRoom.Name,
                    Colour = GetColour(item.BikeColurId),
                    Available = item.NoOfDay,
                    CityId = cityId,
                    VaraintId = variantId,
                    TotalAmount = item.Price,
                    AddressLine1=item.ShowRoom.AddressLine1,
                    AddressLine2 = item.ShowRoom.AddressLine2,
                    MobileNumber = item.ShowRoom.MobileNumber,
                    EmailId = item.ShowRoom.EmailId,
                    PhoneNumber = item.ShowRoom.PhoneNumber,
                    Availibility=item.Availbility,
                    AreaName=item.ShowRoom.Area.Name,
                    CityName=item.ShowRoom.Area.City.Name,
                    PinCode=item.ShowRoom.Area.PinCode,
                    Url= IsValidUrl(item.ShowRoom.UrlLink)
                });
            }

            return availableList;
        }
        private string GetColour(int colourId)
        {

            string colourImage = "";
            var coulrImage = _appContext.Colours.FirstOrDefault(m => m.Id == colourId);
            if (coulrImage != null)
            {
                var bikeImage = _appContext.Documents.FirstOrDefault(m => m.Id == coulrImage.ImageId);
                if (bikeImage != null)
                {
                    colourImage = GetDocumentFromByteArray(bikeImage.Data);
                }

            }
            return colourImage;
        }
        private string GetDocumentFromByteArray(byte[] bytes)
        {
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }
        public  string IsValidUrl(string url)
        {
            if (url == null) {
                return "";
            }
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)|| !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "http://" + url;
            }
            return url;
        }
    }
}
