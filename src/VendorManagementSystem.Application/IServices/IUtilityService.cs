using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.DashBoard;
using VendorManagementSystem.Application.Dtos.UtilityDtos;

namespace VendorManagementSystem.Application.IServices
{
    public interface IUtilityService
    {
        public ApplicationResponseDto<CountDto> VendorCount();
        public ApplicationResponseDto<CountDto> InvoiceCount();
        public ApplicationResponseDto<Dictionary<string, List<string>>> ExtractPropertyNames(string type);
        public List<string> ExtractPropertyNames<T>(T objectToExtract);
        public T ParseEnum<T>(string value);
        public ApplicationResponseDto<string> SaveCanvasSignature(string signature);
        public ApplicationResponseDto<DashBoardResponseDto> DashBoardData();
        public string GenerateIdentifier(string prefix, int id, int length);
    }
}
