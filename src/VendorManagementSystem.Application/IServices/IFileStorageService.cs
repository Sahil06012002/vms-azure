using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
namespace VendorManagementSystem.Application.IServices
{
    public interface IFileStorageService
    {
        public Task<BlobContainerClient> CreateContainerIfNotExist(string name);
        public Task<string> UploadFile(IFormFile file, string fileName,BlobContainerClient container);
        public Task<FileContentDto> GetFile(string fileName, BlobContainerClient container);
        public Task<List<string>> GetAllContainers();
    }
}
