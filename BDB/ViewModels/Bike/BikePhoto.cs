using System.Collections.Generic;

namespace BDB.ViewModels.Bike
{
    public class BikePhoto
    {
        public int BikeId { get; set; }
        public List<BikeImageModel> Images { get; set; }
    }
    public class BikeImageModel
    {
        public int BikeImageId { get; set; }
        public ImageModel Images { get; set; }
    }
    public class BrandPhotoModel
    {
        public int BikeId { get; set; }
        public List<FileUpload> Files { get; set; }
    }
}
