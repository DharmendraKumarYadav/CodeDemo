using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Entity
{
    public class Document: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Data { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }
        public string DownlaodUrl { get; set; }
    }
}
