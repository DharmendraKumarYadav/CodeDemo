using BDB;
using DAL.Models.Entity;
using DAL.Models.Idenity;
using DAL.Repositories.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Common
{
    public class DocumentService : Repository<Document>, IDocumentService

    {
        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
        public DocumentService(DbContext context) : base(context)
        {
        }

        public List<int> UploadDocument(List<FileUpload> files)
        {
            var listData = new List<Document>();
            var listDocId = new List<int>();
            foreach (var item in files)
            {
                var dataEntity = new Document();
                dataEntity.Data = GetByteFromBase64(item.FileAsBase64);// Helper.Compressimage(GetByteFromBase64(item.FileAsBase64),96,96);
                dataEntity.Name = item.FileName;
                dataEntity.Name = item.FileName;
                dataEntity.FileType = item.FileType;
                listData.Add(dataEntity);
                _appContext.Documents.Add(dataEntity);
                _appContext.SaveChanges();
                listDocId.Add(dataEntity.Id);
            }
            return listDocId;
        }

        public int UploadDocument(FileUpload item)
        {
                var dataEntity = new Document();
                dataEntity.Data = GetByteFromBase64(item.FileAsBase64);// Helper.Compressimage(GetByteFromBase64(item.FileAsBase64),96,96);
                dataEntity.Name = item.FileName;
                dataEntity.Name = item.FileName;
                dataEntity.FileType = item.FileType;
                _appContext.Documents.Add(dataEntity);
                _appContext.SaveChanges();
                return dataEntity.Id;
        }
        public void UpdateDocument(FileUpload item, int documentId)
        {
            var dataEntity = _appContext.Documents.Where(m => m.Id == documentId).FirstOrDefault();
            if (dataEntity != null)
            {
                dataEntity.Data = GetByteFromBase64(item.FileAsBase64);
                dataEntity.Name = item.FileName;
                dataEntity.Name = item.FileName;
                dataEntity.FileType = item.FileType;
                _appContext.Documents.Update(dataEntity);
                _appContext.SaveChanges();
            }
        }

        public async Task<ImageModel> GetDocumentImage(int id)
        {

            ImageModel imageObject = new ImageModel();
            var dataObject = await _appContext.Documents.FirstOrDefaultAsync(m => m.Id == id);
            if (dataObject != null)
            {
                imageObject.Id = id;
                imageObject.FileName = dataObject.Name;
                imageObject.FileType = dataObject.FileType;
                imageObject.Base64Content = GetDocumentFromByteArray(dataObject.Data);
            }
            else
            {
                imageObject = null;
            }

            return imageObject;
        }

        private byte[] GetByteFromBase64(string data)
        {
            var base64data = data;
            if (base64data.Contains(","))
            {
                base64data = base64data.Substring(base64data.IndexOf(",") + 1);
            }
            return Convert.FromBase64String(base64data);
        }

        private  Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private string GetDocumentFromByteArray(byte[] bytes)
        {
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }
    }
}
