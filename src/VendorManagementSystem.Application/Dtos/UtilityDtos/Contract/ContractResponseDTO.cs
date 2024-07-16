using Microsoft.AspNetCore.Http;

namespace VendorManagementSystem.Application.Dtos.UtilityDtos.Contract
{
    public class ContractResponseDto
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string ContactPersonEmail { get; set; } = string.Empty;
        public string ContactPersonName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}