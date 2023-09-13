using AutoMapper;
using BDB.Helpers;
using BDB.ViewModels.Bike;
using BDB.ViewModels.Common;
using BDB.ViewModels.Home;
using Core;
using Core.Enums;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;


        public HomeController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<HomeController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
         
        }

        [HttpGet("brands")]
        [ProducesResponseType(200, Type = typeof(List<BrandListModel>))]
        public async Task<IActionResult> GetBrands()
        {
            List<BrandListModel> brandListModels = new List<BrandListModel>();
            var dataList = await _unitOfWork.BrandService.GetAllAsync();
            foreach (var item in dataList)
            {
                var imageData = await _unitOfWork.DocumentService.GetDocumentImage(item.ImageId);
                brandListModels.Add(new BrandListModel
                {
                    Image = imageData?.Base64Content,
                    Id = item.Id,
                    Name = item.Name

                });
            }
            return Ok(brandListModels);
        }

        [HttpGet("budgets")]
        [ProducesResponseType(200, Type = typeof(List<BudgetListModel>))]
        public async Task<IActionResult> GetBudgets()
        {
            var list = new List<BudgetListModel>();
            var dataList = await _unitOfWork.BudgetService.GetAllAsync();
            foreach (var item in dataList)
            {
                list.Add(new BudgetListModel
                {
                    Id = item.Id,
                    Name = item.Description
                });
            }
            return Ok(list);
        }

        [HttpGet("displacements")]
        [ProducesResponseType(200, Type = typeof(List<DisplacementModelList>))]
        public async Task<IActionResult> GetDisplacements()
        {
            var list = new List<DisplacementModelList>();
            var dataList = await _unitOfWork.DisplacementService.GetAllAsync();
            foreach (var item in dataList)
            {
                list.Add(new DisplacementModelList
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }
            return Ok(list);
        }

        [HttpGet("bodystyles")]
        [ProducesResponseType(200, Type = typeof(List<BodyStyleModelList>))]
        public async Task<IActionResult> GetBodyStyles()
        {
            var list = new List<BodyStyleModelList>();
            var dataList = await _unitOfWork.BodyStyleService.GetAllAsync();
            foreach (var item in dataList)
            {
                var imageData = await _unitOfWork.DocumentService.GetDocumentImage(item.ImageId);
                list.Add(new BodyStyleModelList
                {
                    Id = item.Id,
                    Image=imageData?.Base64Content,
                    Name = item.Name
                });
            }
            return Ok(list);
        }

        [HttpGet("featured")]
        [ProducesResponseType(200, Type = typeof(BikeFeaturedList))]
        public async Task<IActionResult> GetFeaturedBikes ()
        {
            const string HeaderKeyName = "currnetcityid";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues cityId);
            var dataList = await _unitOfWork.BikeService.GetFeaturedBikes(cityId);
            return Ok(dataList);
        }
        [HttpGet("search")]
        [ProducesResponseType(200, Type = typeof(List<BikeSearchData>))]
        public async Task<IActionResult> GetSearchData([FromQuery] string query)
        {
            var dataList = await _unitOfWork.BikeService.GetSearchData(query );
            return Ok(dataList);
        }
        [HttpGet("city")]
        [ProducesResponseType(200, Type = typeof(List<CityModel>))]
        public async Task<IActionResult> GetCity()
        {
            var dataList = await _unitOfWork.CityService.GetCity(-1, -1);
            var mappedObject = _mapper.Map<List<CityModel>>(dataList);
            return Ok(mappedObject);
        }
    }
}
