using System;

namespace BDB
{
    public class FileUpload
    {
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public string FileAsBase64 { get; set; }
    }

    public class ImageModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Base64Content { get; set; }
        public string FileType { get; set; }
    }
}
