using AutoMapper;
using BDB.Helpers;
using BDB.ViewModels;
using BDB.ViewModels.Account;
using BDB.ViewModels.Attribute;
using BDB.ViewModels.BikeAttribute;
using BDB.ViewModels.Common;
using DAL;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;


        public LocationController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<LocationController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }

        #region City

        [HttpGet("city/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<CityModel>))]
        public async Task<IActionResult> GetCity(int pageNumber, int pageSize)
        {

            var dataList = await _unitOfWork.CityService.GetCity(pageNumber, pageSize);
            var mappedObject = _mapper.Map<List<CityModel>>(dataList);
            return Ok(mappedObject);
        }


        [HttpPost("city")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCity([FromBody] CityModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");


                var data = _mapper.Map<City>(model);
                _unitOfWork.CityService.Add(data);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("city/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] CityModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting city id in parameter and model data");



                var dataObject = await _unitOfWork.CityService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);


                _mapper.Map<CityModel, City>(model, dataObject);

                _unitOfWork.CityService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("city/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var dataObject = await _unitOfWork.CityService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.CityService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }

        #endregion

        #region Area
        [HttpGet("area/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<AreaModel>))]
        public async Task<IActionResult> GetArea(int pageNumber, int pageSize)
        {

            var dataList = await _unitOfWork.AreaService.GetArea(pageNumber, pageSize);
            var mappedObject = _mapper.Map<List<AreaModel>>(dataList);
            return Ok(mappedObject);
        }

        [HttpPost("area")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AreaCity([FromBody] AreaModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");


                var data = _mapper.Map<Area>(model);
                _unitOfWork.AreaService.Add(data);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("area/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateArea(int id, [FromBody] AreaModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting city id in parameter and model data");



                var dataObject = await _unitOfWork.AreaService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);


                _mapper.Map<AreaModel, Area>(model, dataObject);

                _unitOfWork.AreaService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("area/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteArea(int id)
        {
            var dataObject = await _unitOfWork.AreaService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.AreaService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }

        #endregion
    }
}
