using System.Collections.Generic;

namespace BDB.ViewModels
{
    public class BrandViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ImageId { get; set; }
        public ImageModel Images { get; set; }
    }
    public class BrandModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<FileUpload> Files { get; set; }
    }
}
