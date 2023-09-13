using AutoMapper;
using BDB.Authorization;
using BDB.Helpers;
using BDB.Helpers.Common;
using BDB.ViewModels;
using BDB.ViewModels.Bike;
using Core.Models;
using DAL;
using DAL.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BikeController : ControllerBase
    {
   
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IEmailService _emailSender;

        public BikeController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<BikeController> logger, IEmailService emailSender)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;

        }

        [HttpGet("bikes/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeViewModel>))]
        public async Task<IActionResult> GetBikes(int pageNumber, int pageSize)
        {

            var dataList = await _unitOfWork.BikeService.GetBikes(pageNumber, pageSize);
            var mappedObject = _mapper.Map<List<BikeViewModel>>(dataList);
            return Ok(mappedObject);
        }



        [HttpGet("bike/{id:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeModel>))]
        public async Task<IActionResult> GetGeneralDetails(int id)
        {
            var bike = await _unitOfWork.BikeService.GetAsync(id);
            var mappedObject = _mapper.Map<BikeModel>(bike);
            mappedObject.ColourIds = _unitOfWork.BikeColourService.Find(m => m.BikeId == id).Select(m => m.ColourId).ToList();
            mappedObject.CityIds = _unitOfWork.BikeCityService.Find(m => m.BikeId == id).Select(m => m.CityId).ToList();
            var variants = _unitOfWork.BikeVariantsService.Find(m => m.BikeId == id).ToList();
            var mappedVariant = _mapper.Map<List<BikeVariantModel>>(variants);
            mappedObject.Document = await _unitOfWork.DocumentService.GetDocumentImage(mappedObject.DocumentId);
            mappedObject.Variants = mappedVariant;
            return Ok(mappedObject);
        }

        [HttpPost("bike")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateGeneralDetail([FromBody] BikeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");


                if (model.Id == 0)
                {
                    var bikeEntity = _mapper.Map<Bike>(model);

                    if (model.Files.Count > 0) {
                        bikeEntity.DocumentId = _unitOfWork.DocumentService.UploadDocument(model.Files[0]);
                    }
                    _unitOfWork.BikeService.Add(bikeEntity);
                    await _unitOfWork.SaveChangesAsync();
                    var bikeColourEntities = new List<BikeColour>();
                    foreach (var item in model.ColourIds)
                    {
                        bikeColourEntities.Add(new BikeColour { ColourId = item, BikeId = bikeEntity.Id });
                    }
                    _unitOfWork.BikeColourService.AddRange(bikeColourEntities);

                    var bikeCityEntity = new List<BikeCity>();
                    foreach (var item in model.CityIds)
                    {
                        bikeCityEntity.Add(new BikeCity { CityId = item, BikeId = bikeEntity.Id });
                    }
                    _unitOfWork.BikeCityService.AddRange(bikeCityEntity);


                    var bikeVariantEntity = new List<BikeVariant>();
                    foreach (var item in model.Variants)
                    {
                        bikeVariantEntity.Add(new BikeVariant { Name = item.Name, Specification = item.Specification, BikeId = bikeEntity.Id });
                    }
                    _unitOfWork.BikeVariantsService.AddRange(bikeVariantEntity);
                    await _unitOfWork.SaveChangesAsync();
                    return Ok(bikeEntity.Id);
                }
                else
                {
                    var bikeEntity = await _unitOfWork.BikeService.GetAsync(model.Id);
                    bikeEntity.ShortDescription = model.ShortDescription;
                    bikeEntity.LongDescription = model.LongDescription;
                    bikeEntity.BrandId = model.BrandId;
                    bikeEntity.BodyStyleId = model.BodyStyleId;
                    bikeEntity.Displacement = model.Displacement;
                    bikeEntity.CategoryId = model.CategoryId;
                    bikeEntity.Price = model.Price;
                    bikeEntity.IsElectricBike = model.IsElectricBike=="1"?true:false;

                    if (model.Files.Count > 0)
                    {
                        if (bikeEntity.DocumentId != 0) {
                           _unitOfWork.DocumentService.UpdateDocument(model.Files[0],bikeEntity.DocumentId);
                        }
                        else
                        {
                            bikeEntity.DocumentId = _unitOfWork.DocumentService.UploadDocument(model.Files[0]);

                        }
                       
                    }

                    _unitOfWork.BikeService.Update(bikeEntity);
                    await _unitOfWork.SaveChangesAsync();

                    var existingColours = _unitOfWork.BikeColourService.Find(m => m.BikeId == bikeEntity.Id).ToList();
                    _unitOfWork.BikeColourService.RemoveRange(existingColours);


                    var existingCity = _unitOfWork.BikeCityService.Find(m => m.BikeId == bikeEntity.Id).ToList();
                    _unitOfWork.BikeCityService.RemoveRange(existingCity);



                    //var existingVaraints = _unitOfWork.BikeVariantsService.Find(m => m.BikeId == bikeEntity.Id).ToList();
                    //_unitOfWork.BikeVariantsService.RemoveRange(existingVaraints);

                    var bikeColourEntities = new List<BikeColour>();
                    foreach (var item in model.ColourIds)
                    {
                        bikeColourEntities.Add(new BikeColour { ColourId = item, BikeId = bikeEntity.Id });
                    }


                    var bikeCityEntity = new List<BikeCity>();
                    foreach (var item in model.CityIds)
                    {
                        bikeCityEntity.Add(new BikeCity { CityId = item, BikeId = bikeEntity.Id });
                    }
                    _unitOfWork.BikeCityService.AddRange(bikeCityEntity);


                    _unitOfWork.BikeColourService.AddRange(bikeColourEntities);

                    foreach (var item in model.Variants)
                    {
                        var varinatsData = await _unitOfWork.BikeVariantsService.GetAsync(item.Id);
                        if (varinatsData != null)
                        {
                            varinatsData.Name = item.Name;
                            varinatsData.Specification = item.Specification;
                            _unitOfWork.BikeVariantsService.Update(varinatsData);
                        }
                        else
                        {
                            var bikeVariantEntity = new BikeVariant();
                            bikeVariantEntity.Name = item.Name;
                            bikeVariantEntity.Specification = item.Specification;
                            bikeVariantEntity.BikeId = bikeEntity.Id;
                            _unitOfWork.BikeVariantsService.Update(bikeVariantEntity);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    return Ok(bikeEntity.Id);

                }

            }

            return BadRequest(ModelState);
        }



        [HttpDelete("bike-broucher/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBikeBroucher(int id)
        {
            var dataObject = await _unitOfWork.BikeService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);


            var documnt =await _unitOfWork.DocumentService.GetAsync(dataObject.DocumentId);
            if (documnt != null) {
                _unitOfWork.DocumentService.Remove(documnt);
            }
            dataObject.DocumentId = 0;
            _unitOfWork.BikeService.Update(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }

        [HttpDelete("bike-variants/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBikeVariants(int id)
        {
            var dataObject = await _unitOfWork.BikeVariantsService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.BikeVariantsService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }

        [HttpGet("bike-image/{id:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeModel>))]
        public async Task<IActionResult> GetBikePhoto(int id)
        {
            BikePhoto bikePhotos = new BikePhoto();
            var bike = await _unitOfWork.BikeService.GetAsync(id);
            if (bike == null)
                return BadRequest(ModelState);

            bikePhotos.BikeId = bike.Id;
            var bikePhotoDocId = _unitOfWork.BikeImageService.Find(m => m.BikeId == id).ToList();
            var image = new List<BikeImageModel>();
            var imageList = new List<ImageModel>();
            foreach (var item in bikePhotoDocId)
            {
                var imageObject = await _unitOfWork.DocumentService.GetDocumentImage(item.DocumentId);
                image.Add(new BikeImageModel { BikeImageId = item.Id, Images = imageObject });
            }
            bikePhotos.Images = image;
            return Ok(bikePhotos);
        }

        [HttpPost("bike-image")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBikePhoto([FromBody] BrandPhotoModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                var bike = await _unitOfWork.BikeService.GetAsync(model.BikeId);
                if (bike == null)
                    return BadRequest(ModelState);


                var documentIds = _unitOfWork.DocumentService.UploadDocument(model.Files);
                var bikePhotoEntities = new List<BikeImage>();
                foreach (var documentId in documentIds)
                {
                    bikePhotoEntities.Add(new BikeImage { DocumentId = documentId, BikeId = bike.Id });
                }
                _unitOfWork.BikeImageService.AddRange(bikePhotoEntities);

                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("bike-image/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBikePhoto(int id)
        {
            var dataObject = await _unitOfWork.BikeImageService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            var dataDocument = _unitOfWork.DocumentService.Find(m => m.Id == dataObject.DocumentId).FirstOrDefault();

            if (dataDocument == null)
                return NotFound(id);

            _unitOfWork.BikeImageService.Remove(dataObject);
            _unitOfWork.DocumentService.Remove(dataDocument);
            _unitOfWork.SaveChanges();
            return NoContent();
        }


        [HttpGet("bike-price/{id:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeVarianPricetModel>))]
        public async Task<IActionResult> GetBikePrice(int id)
        {
            var bikeVariantPrice = new List<BikeVarianPricetModel>();
            var bike = await _unitOfWork.BikeService.GetAsync(id);
            if (bike == null)
                return NotFound();

            var variantList = _unitOfWork.BikeVariantsService.Find(m => m.BikeId == id).ToList();
            foreach (var item in variantList)
            {
                bikeVariantPrice.Add(new BikeVarianPricetModel { VariantId = item.Id, Name = item.Name, Prices = GetBikePriceByVariantId(item.Id) });
            }
            return Ok(bikeVariantPrice);
        }

        [HttpPost("bike-price")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBikePrice([FromBody] List<BikeVarianPricetModel> model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");



                foreach (var itemVar in model)
                {
                    var variantPrice = _unitOfWork.BikePriceService.Find(m => m.BikeVariantId == itemVar.VariantId).ToList();
                    if (variantPrice.Count > 0)
                    {
                        _unitOfWork.BikePriceService.RemoveRange(variantPrice);
                    }
                    var bikePriceEntities = new List<BikePrice>();
                    foreach (var item in itemVar.Prices)
                    {
                        bikePriceEntities.Add(new BikePrice
                        {
                            RTOPrice = item.RtoAmount,
                            Price = item.TotalAmount,
                            InsurancePrice = item.InsuranceAmount,
                            ExShowRoomPrice = item.ExShowRoomAmount,
                            BikeVariantId = itemVar.VariantId,
                            BookingAmount = item.BookingAmount,
                            CityId = item.CityId,
                            isMinPrice=item.isMinPrice

                        });
                    }
                    _unitOfWork.BikePriceService.AddRange(bikePriceEntities);
                    await _unitOfWork.SaveChangesAsync();

                }
                return NoContent();
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("bike-price/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBikePrice(int id)
        {
            var dataObject = await _unitOfWork.BikePriceService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.BikePriceService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }


        [HttpGet("bike-specification/{id:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeModel>))]
        public async Task<IActionResult> GetBikeSpecification(int id)
        {
            var bikeVariantSpecification = new List<BikeVarianSpecificationModel>();
            var bike = await _unitOfWork.BikeService.GetAsync(id);
            if (bike == null)
                return NotFound();

            var variantList = _unitOfWork.BikeVariantsService.Find(m => m.BikeId == id).ToList();
            foreach (var item in variantList)
            {
                bikeVariantSpecification.Add(new BikeVarianSpecificationModel { VariantId = item.Id, Name = item.Name, Specifications = GetBikeSpecificationByVariantId(item.Id) });
            }
            return Ok(bikeVariantSpecification);
        }

        [HttpPost("bike-specification")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBikeSpecification([FromBody] List<BikeVarianSpecificationModel> model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");



                foreach (var itemVar in model)
                {
                    var variantSpec = _unitOfWork.BikeSpecificationsService.Find(m => m.BikeVariantId == itemVar.VariantId).ToList();
                    if (variantSpec != null)
                    {
                        _unitOfWork.BikeSpecificationsService.RemoveRange(variantSpec);
                    }
                    var bikeSpecEntities = new List<BikeSpecifications>();
                    foreach (var item in itemVar.Specifications)
                    {
                        bikeSpecEntities.Add(new BikeSpecifications
                        {
                            BikeVariantId = itemVar.VariantId,
                            AttributeId = item.AttributeId,
                            AttributeValue = item.AttributeValue,
                            SpecificationId = item.SpecificationId,

                        }); ;
                    }
                    _unitOfWork.BikeSpecificationsService.AddRange(bikeSpecEntities);
                    await _unitOfWork.SaveChangesAsync();
                }
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("bike-specification/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBikeSpecification(int id)
        {
            var dataObject = await _unitOfWork.BikeSpecificationsService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.BikeSpecificationsService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }

        [HttpGet("bike-features/{id:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeVarianFeatureModel>))]
        public async Task<IActionResult> GetBikeFeatures(int id)
        {
            var bikeVariantFeature = new List<BikeVarianFeatureModel>();
            var bike = await _unitOfWork.BikeService.GetAsync(id);
            if (bike == null)
                return NotFound();

            var variantList = _unitOfWork.BikeVariantsService.Find(m => m.BikeId == id).ToList();
            foreach (var item in variantList)
            {
                bikeVariantFeature.Add(new BikeVarianFeatureModel { VariantId = item.Id, Name = item.Name, Features = GetBikeFeatureByVariantId(item.Id) });
            }
            return Ok(bikeVariantFeature);
        }

        [HttpPost("bike-features")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBikeFeatures([FromBody] List<BikeVarianFeatureModel> model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");


                foreach (var itemVar in model)
                {
                    var variantFeature = _unitOfWork.BikeFeaturesService.Find(m => m.BikeVariantId == itemVar.VariantId).ToList();
                    if (variantFeature != null)
                    {
                        _unitOfWork.BikeFeaturesService.RemoveRange(variantFeature);
                    }

                    var bikeFeatureEntities = new List<BikeFeatures>();
                    foreach (var item in itemVar.Features)
                    {
                        bikeFeatureEntities.Add(new BikeFeatures
                        {
                            BikeVariantId = itemVar.VariantId,
                            AttributeId = item.AttributeId,
                            AttributeValue = item.AttributeValue,

                        });
                    }
                    _unitOfWork.BikeFeaturesService.AddRange(bikeFeatureEntities);
                    await _unitOfWork.SaveChangesAsync();

                }
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("bike-features/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBikeFeature(int id)
        {
            var dataObject = await _unitOfWork.BikeFeaturesService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.BikeFeaturesService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }


        #region Bike Similar
        [HttpGet("bike-related/{pageNumber:int}/{pageSize:int}/{bikeId:int}")]
        [ProducesResponseType(200, Type = typeof(List<SimilarBikeModel>))]
        public async Task<IActionResult> GetBikeRelated(int pageNumber,int pageSize,int bikeId)
        {
            var bike = await _unitOfWork.BikeSimilarService.GetSimilarBikes(pageNumber, pageSize, bikeId);
           
            return Ok(bike);
        }

        [HttpPost("bike-related")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBikeRelated([FromBody] SimilarBikeRequest model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if(model.BikeId == model.SimilarBikeId)
                    return BadRequest($"{nameof(model)} same bike can not be realted");


                var data = _mapper.Map<BikeSimilar>(model);

                _unitOfWork.BikeSimilarService.Add(data);
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("bike-related/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBikeRelated(int id)
        {
            var dataObject = await _unitOfWork.BikeSimilarService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.BikeSimilarService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }

        #endregion




        [HttpGet("feature-bike-type/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<FeaturedBikeTypeModel>))]
        public async Task<IActionResult> GetFeatureBikeType(int pageNumber, int pageSize)
        {
            var dataList = await _unitOfWork.FeaturedBikeService.GetFeaturedBikes(pageNumber, pageSize);
            return Ok(dataList);
        }

        [HttpPost("feature-bike-type")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateBikeFeaturesType([FromBody] FeatureBikeTypeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                var featureJob=_unitOfWork.FeaturedBikeService.Find(m=>m.BikeId== model.BikeId && m.FeatureType== model.TypeId).FirstOrDefault();
                if (featureJob == null) {
                    var featureBikeEntities = new FeaturedBike();
                    featureBikeEntities.BikeId = model.BikeId;
                    featureBikeEntities.FeatureType = model.TypeId;
                    _unitOfWork.FeaturedBikeService.Add(featureBikeEntities);
                    await _unitOfWork.SaveChangesAsync();
                }
                return NoContent();
            }
            return BadRequest(ModelState);
        }


        [HttpPut("feature-bike-type/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateDealer(int id, [FromBody] FeatureBikeTypeModel model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                    return BadRequest($"{nameof(model)} cannot be null");

                if (id != model.Id)
                    return BadRequest("Conflicting  id in parameter and model data");



                var dataObject = await _unitOfWork.FeaturedBikeService.GetAsync(id);

                if (dataObject == null)
                    return NotFound(id);

                dataObject.BikeId = model.BikeId;
                dataObject.FeatureType = model.TypeId;
                _unitOfWork.FeaturedBikeService.Update(dataObject);
                _unitOfWork.SaveChanges();
                return NoContent();

            }

            return BadRequest(ModelState);
        }

        [HttpDelete("feature-bike-type/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBikeFeaturesType(int id)
        {
            var dataObject = await _unitOfWork.FeaturedBikeService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);

            _unitOfWork.FeaturedBikeService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }

        [HttpDelete("bike/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBike(int id)
        {
            var dataObject = await _unitOfWork.BikeService.GetAsync(id);

            if (dataObject == null)
                return NotFound(id);


            var existingColours = _unitOfWork.BikeColourService.Find(m => m.BikeId == dataObject.Id).ToList();
            _unitOfWork.BikeColourService.RemoveRange(existingColours);

            var existingVaraints = _unitOfWork.BikeVariantsService.Find(m => m.BikeId == dataObject.Id).ToList();
            _unitOfWork.BikeVariantsService.RemoveRange(existingVaraints);

            _unitOfWork.BikeService.Remove(dataObject);
            _unitOfWork.SaveChanges();
            return NoContent();
        }


        [AllowAnonymous]
        [HttpGet("availableshowroom/{cityId:int}/{variantid:int}")]
        [ProducesResponseType(200, Type = typeof(List<AvailableShowRoomBikes>))]
        public async Task<IActionResult> GetAvailbleShowRoom(int cityId, int variantid )
        {
            var showRoomWithPriceBike =await _unitOfWork.SaleBikeService.GetAllBikeByCityAndVariant(cityId, variantid);
            return Ok(showRoomWithPriceBike);
        }

        [HttpGet("bikebycity/{showroomid:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeVariantPriceModel>))]
        public async Task<IActionResult> GetBikesByCity(int showroomid)
        {
            var bikeVairaintList = new List<BikeVariantPriceModel>();
            var showRoom = await _unitOfWork.ShowRoomService.GetAsync(showroomid);
            if (showRoom != null) {
                var area = await _unitOfWork.AreaService.GetAsync(showRoom.AreaId);
                if (area != null) {
                    var bikeVariantPrice = _unitOfWork.BikePriceService.Find(m => m.CityId == area.CityId).Distinct().ToList();
                    foreach (var item in bikeVariantPrice)
                    {
                        var variantData = GetBikePriceByVariantAndCityId(item.BikeVariantId, area.CityId);
                        var variant = await _unitOfWork.BikeVariantsService.GetAsync(item.BikeVariantId);
                        if (variant != null) {
                            variantData.Name=variant.Name;
                            variantData.Id=variant.Id;
                            variantData.Specification=variant.Specification;
                        }
                        bikeVairaintList.Add(variantData);
                    }
                }
            }

 

            return Ok(bikeVairaintList);
        }

        [HttpGet("bike-details/{id:int}/{cityId:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeVariantPriceModel>))]
        public async Task<IActionResult> GetBikeDetailsByCity(int id, int cityId)
        {
            var bikeVariantSpecification = new List<BikeVariantPriceModel>();
            var bike = await _unitOfWork.BikeService.GetAsync(id);
            if (bike == null)
                return NotFound();

            var variantList = _unitOfWork.BikeVariantsService.Find(m => m.BikeId == id).ToList();
            foreach (var item in variantList)
            {
                var data = GetBikePriceByVariantAndCityId(item.Id, cityId);
                data.Name = item.Name;
                bikeVariantSpecification.Add(data);
            }
            return Ok(bikeVariantSpecification);
        }


        #region Ratings & Request

        [HttpGet("bikerating/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeUserRatingModel>))]
        public async Task<IActionResult> GetBikeUserRating(int pageNumber, int pageSize)
        {

            var dataList = await _unitOfWork.BikeUserRatingService.GetBikeUserRating(pageNumber, pageSize);
            var mappedObject = _mapper.Map<List<BikeUserRatingModel>>(dataList);
            return Ok(mappedObject);
        }

        [HttpGet("bikeratingpublished/{id}")]
        [ProducesResponseType(200, Type = typeof(BikeUserRatingModel))]
        public async Task<IActionResult> PublishBikeUseRating(int id)
        {

            var bikeRating = await _unitOfWork.BikeUserRatingService.GetAsync(id);
            if (bikeRating != null) {
                if (bikeRating.IsPublished)
                {
                    bikeRating.IsPublished = false;
                }
                else {
                    bikeRating.IsPublished = true;
                }
                _unitOfWork.BikeUserRatingService.Update(bikeRating);
                _unitOfWork.SaveChanges();
            }

            var mappedObject = _mapper.Map<BikeUserRatingModel>(bikeRating);
            return Ok(mappedObject);
        }


        [HttpGet("bikerequest/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<BikeSubscribeModel>))]
        public async Task<IActionResult> GetBikeUseRating(int pageNumber, int pageSize)
        {
            string dealerId = null;
            var IsDealer = this.User.IsInRole(AppUserRoles.Dealer);
            if (IsDealer)
            {
                dealerId = Utilities.GetUserId(this.User);
            }
            var dataList = await _unitOfWork.BikeUserRequestService.GetBikeUserRequest(pageNumber, pageSize, dealerId);
            var mappedObject = _mapper.Map<List<BikeSubscribeModel>>(dataList);
            return Ok(mappedObject);
        }

        #endregion

        private BikeVariantPriceModel GetBikePriceByVariantAndCityId(int variantId, int cityId)
        {
            BikeVariantPriceModel dataObject = new BikeVariantPriceModel();
            var result = _unitOfWork.BikePriceService.Find(m => m.BikeVariantId == variantId && m.CityId == cityId).FirstOrDefault();

            if (result != null)
            {
                dataObject.Id = result.Id;
                dataObject.BookingAmount = result.BookingAmount;
                dataObject.ExShowRoomAmount = result.ExShowRoomPrice;
                dataObject.InsuranceAmount = result.InsurancePrice;
                dataObject.TotalAmount = result.Price;
                dataObject.RtoAmount = result.RTOPrice;
                dataObject.IsMinPrice = result.isMinPrice;
            }

            return dataObject;

        }
        private List<BikePriceModel> GetBikePriceByVariantId(int variantId)
        {
            List<BikePriceModel> bikePriceModels = new List<BikePriceModel>();
            var dataObject = _unitOfWork.BikePriceService.Find(m => m.BikeVariantId == variantId).ToList();
            foreach (var item in dataObject)
            {

                bikePriceModels.Add(new BikePriceModel
                {
                    Id = item.Id,
                    VariantId = variantId,
                    BookingAmount = item.BookingAmount,
                    CityId = item.CityId,
                    ExShowRoomAmount = item.ExShowRoomPrice,
                    InsuranceAmount = item.InsurancePrice,
                    TotalAmount = item.Price,
                    RtoAmount = item.RTOPrice,
                    isMinPrice=item.isMinPrice
                });

            }
            return bikePriceModels;

        }
        private List<BikeSpecificationModel> GetBikeSpecificationByVariantId(int variantId)
        {
            List<BikeSpecificationModel> bikeModels = new List<BikeSpecificationModel>();
            var dataObject = _unitOfWork.BikeSpecificationsService.Find(m => m.BikeVariantId == variantId).ToList();
            foreach (var item in dataObject)
            {

                bikeModels.Add(new BikeSpecificationModel
                {
                    Id = item.Id,
                    BikeVariantId = variantId,
                    AttributeId = item.AttributeId,
                    SpecificationId = item.SpecificationId,
                    AttributeValue = item.AttributeValue

                });

            }
            return bikeModels;

        }
        private List<BikeFeatureModel> GetBikeFeatureByVariantId(int variantId)
        {
            List<BikeFeatureModel> bikeModels = new List<BikeFeatureModel>();
            var dataObject = _unitOfWork.BikeFeaturesService.Find(m => m.BikeVariantId == variantId).ToList();
            foreach (var item in dataObject)
            {

                bikeModels.Add(new BikeFeatureModel
                {
                    Id = item.Id,
                    BikeVariantId = variantId,
                    AttributeId = item.AttributeId,
                    AttributeValue = item.AttributeValue

                });

            }
            return bikeModels;

        }

    }
}
