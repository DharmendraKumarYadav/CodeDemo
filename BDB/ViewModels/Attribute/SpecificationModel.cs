using System.Collections.Generic;

namespace BDB.ViewModels.Attribute
{
    public class SpecificationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ImageId { get; set; }
        public ImageModel Images { get; set; }
    }
    public class SpecificationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ImageId { get; set; }
        public List<FileUpload> Files { get; set; }
    }
}
