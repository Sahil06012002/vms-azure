using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public FileStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task<BlobContainerClient> CreateContainerIfNotExist(string name)
        {
            Console.WriteLine("trying to create container");
            BlobContainerClient _genericblobContainerClient = _blobServiceClient.GetBlobContainerClient(name);
            Console.WriteLine($"{_genericblobContainerClient.Name}");
            Console.WriteLine("got the container instanse");

            await _genericblobContainerClient.CreateIfNotExistsAsync();
            Console.WriteLine("container created");

            return _genericblobContainerClient;
        }
        public async Task<string> UploadFile(IFormFile file, string fileName, BlobContainerClient container)
        {
            try
            {
                if(fileName.IsNullOrEmpty())
                {
                    Guid id = Guid.NewGuid();
                    fileName = $"{id}";
                }
                var blobClient = container.GetBlobClient(fileName);
                await blobClient.UploadAsync(file.OpenReadStream(), true);
                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<FileContentDto> GetFile(string fileName,BlobContainerClient _container)
        {
            try
            {
                var blobClient = _container.GetBlobClient(fileName);
                if (!await blobClient.ExistsAsync())
                {
                    throw new Exception($"File {fileName} not found.");
                }
                var blobDownloadInfo = await blobClient.DownloadAsync();
                using var stream = blobDownloadInfo.Value.Content;
                return new FileContentDto
                {
                    Content = await ReadStreamAsync(stream),
                    ContentType = blobDownloadInfo.Value.ContentType,
                    Name = fileName,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task<byte[]> ReadStreamAsync(Stream stream)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }



        public async Task<List<string>> GetAllContainers()
        {
            List<string> containers = null;

            await foreach (BlobContainerItem container in _blobServiceClient.GetBlobContainersAsync())
            {
                containers.Add(container.Name);
            }
            return containers;
        }
    }
}
