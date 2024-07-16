using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using VendorManagementSystem.Application.Dtos.UtilityDtos;

namespace VendorManagementSystem.Application.IServices
{
    public interface IDigitalSign
    {

        public byte[] SignDocument(byte[] documentContent);

        public byte[] SignDocument(string documentContent);

        public Task<ApplicationResponseDto<bool>> VerifySignature(IFormFile document);
    }
}