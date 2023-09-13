using System.Collections.Generic;

namespace BDB.ViewModels.Attribute
{
    public class BodyStyleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<FileUpload> Files { get; set; }
        public ImageModel Images { get; set; }
        public int ImageId { get; set; }
    }
}
