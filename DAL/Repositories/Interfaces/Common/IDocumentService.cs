using BDB;
using DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces.Common
{
    public interface IDocumentService : IRepository<Document>
    {
        List<int> UploadDocument(List<FileUpload> files);
        Task<ImageModel> GetDocumentImage(int id);
        int UploadDocument(FileUpload item);
        void UpdateDocument(FileUpload item, int documentId);
    }
}
