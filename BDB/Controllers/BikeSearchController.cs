using AutoMapper;
using BDB.Helpers;
using BDB.ViewModels;
using BDB.ViewModels.Bike;
using BDB.ViewModels.Home;
using Core;
using Core.Enums;
using Core.Extension;
using Core.Models;
using DAL;
using DAL.Core.Interfaces;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Services.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitechMedical.Helpers;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace BDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeSearchController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailer;
        private SmtpConfig _config;
        readonly ISmsService _smsService;
        private readonly IAccountManager _accountManager;
        public BikeSearchController(IMapper mapper, IAccountManager accountManager, IUnitOfWork unitOfWork, ILogger<BikeSearchController> logger, IOptions<AppSettings> config, IEmailService emailer, ISmsService smsService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailer = emailer;
            _smsService = smsService;
            _accountManager = accountManager;
            _config = config.Value.SmtpConfig;
        }

        [HttpPost("bikes")]
        [ProducesResponseType(200, Type = typeof(BikeModelPagedResult))]
        public async Task<IActionResult> GetBikesWithFilter([FromBody] SearchFilterModel model)
        {

            BikeModelPagedResult response = new BikeModelPagedResult();
            List<BikeModelList> list = new List<BikeModelList>();
            const string HeaderKeyName = "currnetcityid";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues cityId);
            var dataList = await _unitOfWork.BikeService.GetFilteredBikes(model, cityId);


            response.Filters = await GetFilterList();
            response.TotalCount = dataList.TotalCount;
            response.PageSize = dataList.PageSize;
            response.CurrentPage = dataList.CurrentPage;
            response.TotalPages = dataList.TotalPages;
            response.HasNext = dataList.HasNext;
            response.HasPrevious = dataList.HasPrevious;
            foreach (var item in dataList)
            {
                list.Add(new BikeModelList
                {
                    Id = item.Id,
                    Name = item.Name,
                    BasicSpecification = item.BikeVariants.FirstOrDefault()?.Specification,
                    Price = item.Price.ToString(),
                    ShowRoomName = "Ex-Show Room:" + item.BikeVariants?.FirstOrDefault()?.BikePrices.FirstOrDefault()?.City.Name,
                    Images = GetBiksImages(item.BikeImages),
                    Colours = Getcolours(item.BikeColours.ToList()),
                    BodyStyle = item.BodyStyle.Name,
                    Category = item.Category.Name,
                    Brand = item.Brand.Name,
                    Displacement = item.Displacement.ToString(),
                    TypeId=item.FeaturedBikes.FirstOrDefault()==null?9999999: item.FeaturedBikes.FirstOrDefault().FeatureType
                });
            }
            response.Result = list.OrderBy(m=>m.TypeId).ToList();
            return Ok(response);
        }

        private async Task<List<BikeFilterList>> GetFilterList()
        {
            List<BikeFilterList> bikeFilters = new List<BikeFilterList>();
            var brandList = await _unitOfWork.BrandService.GetAllAsync();
            var brandFilter = new BikeFilterList();
            brandFilter.Slug = "brand";
            brandFilter.Name = "Brand";
            brandFilter.Type = "check";
            brandFilter.Value = "";
            foreach (var item in brandList)
            {
                brandFilter.Items.Add(new FilterListItem
                {
                    Name = item.Name,
                    Slug = item.Id.ToString()
                });
            }
            bikeFilters.Add(brandFilter);


            var budgetList = await _unitOfWork.BudgetService.GetAllAsync();
            var budgetFilter = new BikeFilterList();
            budgetFilter.Slug = "budget";
            budgetFilter.Name = "Budget";
            budgetFilter.Type = "radio";
            budgetFilter.Value = "";
            foreach (var item in budgetList)
            {
                budgetFilter.Items.Add(new FilterListItem
                {
                    Name = item.Description,
                    Slug = item.Id.ToString()
                });
            }
            bikeFilters.Add(budgetFilter);

            var displacementList = await _unitOfWork.DisplacementService.GetAllAsync();
            var displacementFilter = new BikeFilterList();
            displacementFilter.Slug = "displacement";
            displacementFilter.Name = "Displacement";
            displacementFilter.Type = "radio";
            displacementFilter.Value = "";
            foreach (var item in displacementList)
            {
                displacementFilter.Items.Add(new FilterListItem
                {
                    Name = item.Name,
                    Slug = item.Id.ToString()
                });
            }
            bikeFilters.Add(displacementFilter);

            var bodyStyleList = await _unitOfWork.BodyStyleService.GetAllAsync();
            var bodyStyleFilter = new BikeFilterList();
            bodyStyleFilter.Slug = "bodyStyle";
            bodyStyleFilter.Name = "Body Style";
            bodyStyleFilter.Type = "check";
            bodyStyleFilter.Value = "";
            foreach (var item in bodyStyleList)
            {
                bodyStyleFilter.Items.Add(new FilterListItem
                {
                    Name = item.Name,
                    Slug = item.Id.ToString()
                });
            }
            bikeFilters.Add(bodyStyleFilter);


            var categoryList = await _unitOfWork.CategoryService.GetAllAsync();
            var categoryeFilter = new BikeFilterList();
            categoryeFilter.Slug = "category";
            categoryeFilter.Name = "Category";
            categoryeFilter.Type = "check";
            categoryeFilter.Value = "";
            foreach (var item in categoryList)
            {
                categoryeFilter.Items.Add(new FilterListItem
                {
                    Name = item.Name,
                    Slug = item.Id.ToString()
                });
            }
            //categoryeFilter.Items.Add(new FilterListItem
            //{
            //    Name = "Popular Bike",
            //    Slug = ((int)(BikeTypeEnum.Popular)).ToString()
            //});
            //categoryeFilter.Items.Add(new FilterListItem
            //{
            //    Name = "New Launch",
            //    Slug = ((int)(BikeTypeEnum.NewLaunch)).ToString()
            //});
            //categoryeFilter.Items.Add(new FilterListItem
            //{
            //    Name = "Upcoming Bike",
            //    Slug = ((int)(BikeTypeEnum.Upcoming)).ToString()
            //});
            //categoryeFilter.Items.Add(new FilterListItem
            //{
            //    Name = "Scooters",
            //    Slug = ((int)(BikeTypeEnum.Scooters)).ToString()
            //});
            //categoryeFilter.Items.Add(new FilterListItem
            //{
            //    Name = "Sports Bike",
            //    Slug = ((int)(BikeTypeEnum.Sports)).ToString()
            //});
            //categoryeFilter.Items.Add(new FilterListItem
            //{
            //    Name = "Cruiser Bike",
            //    Slug = ((int)(BikeTypeEnum.Cruiser)).ToString()
            //});
            //categoryeFilter.Items.Add(new FilterListItem
            //{
            //    Name = "Electric Bike",
            //    Slug = ((int)(BikeTypeEnum.ElectricBikes)).ToString()
            //});


            bikeFilters.Add(categoryeFilter);
            return bikeFilters;
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

        [HttpGet("bikedetails/{id:int}")]
        [ProducesResponseType(200, Type = typeof(BikeDetailModel))]
        public async Task<IActionResult> GetBikeDetails(int id)
        {
            var bike = await _unitOfWork.BikeService.GetBikeDetail(id);
            if (bike.DocumentId != 0) {
                bike.Document = await _unitOfWork.DocumentService.GetDocumentImage(bike.DocumentId);
            }
            
            return Ok(bike);
        }

        [HttpGet("bikerating/{id:int}")]
        [ProducesResponseType(200, Type = typeof(BikeUserRatingModel))]
        public async Task<IActionResult> GetBikeRatings(int id)
        {
            var bike = await _unitOfWork.BikeUserRatingService.GetBikeRatingsByBikeId(id);
            return Ok(bike);
        }

        [HttpPost("bikesubscribe")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CretaeBikeSubscribe([FromBody] BikeSubscribeModelRequest model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");
                var bikeEntity = _mapper.Map<BikeUserRequest>(model);
                _unitOfWork.BikeUserRequestService.Add(bikeEntity);
                await _unitOfWork.SaveChangesAsync();
                //Sent Email to Admin:
                var query = "";
                var data = _unitOfWork.BikeVariantsService.Find(m => m.Id == Convert.ToInt32(model.BikeVariantId)).FirstOrDefault();
                if (data != null)
                {
                    query = "User is looking for bike " + data.Name;
                    var city = _unitOfWork.CityService.Find(m => m.Id == Convert.ToInt32(model.CityId)).FirstOrDefault();
                    if (city != null)
                    {
                        query = query + " in " + city.Name;
                    }
                }

                //Sent Email to Admin
                string adminMessage = AccountTemplate.SendAvailbilityToAdminEmailAsync(_config.AdminName, model.Name, model.MobileNumber, model.Email.ToString(), model.Remarks, query);
                await _emailer.SendEmailAsync(_config.AdminName, _config.AdminEmail, "Bike Availbilty Request Recieved  ", adminMessage);

                //Sent Emailto Show Room Dealer
                if (model.DealerShowRoomId != 0)
                {
                    var userIdDealer = await _unitOfWork.ShowRoomService.GetAsync(model.DealerShowRoomId);
                    if (userIdDealer != null)
                    {
                        var user = await _accountManager.GetUserByIdAsync(userIdDealer.UserId.ToString());
                        if (user != null)
                        {
                            string dealerMessage = AccountTemplate.SendAvailbilityToAdminEmailAsync(_config.AdminName, model.Name, model.MobileNumber, model.Email.ToString(), model.Remarks, query);
                            await _emailer.SendEmailAsync(user.FullName, user.Email, "Bike Availbilty Request Recieved  ", dealerMessage);

                        }
                    }

                }


                //Sent Email to Customer:
                string customerMessage = AccountTemplate.SendBikeAvailbilityToCustomerEmailAsync(model.Name);
                await _emailer.SendEmailAsync(model.Name, model.Email, "Bike Availbilty Request Recieved confirmation  ", customerMessage);


                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPost("bikerating")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CretaeBikeRating([FromBody] BikeUserRatingModel model)
        {
            BikeUserRatingModel bookinResponse = new BikeUserRatingModel();
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                var bikes = await _unitOfWork.BikeService.GetBikeDetail(model.BikeId);
                if (bikes != null)
                {
                    var bikeEntity = _mapper.Map<BikeUserRating>(model);
                    bikeEntity.BikeId = model.BikeId;

                    bikeEntity.IsPublished = false;

                    _unitOfWork.BikeUserRatingService.Add(bikeEntity);
                    _unitOfWork.SaveChanges();


                    return Ok(bookinResponse);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }

            return BadRequest(ModelState);
        }
        private string GetDocumentFromByteArray(byte[] bytes)
        {
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }
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
    }
}
