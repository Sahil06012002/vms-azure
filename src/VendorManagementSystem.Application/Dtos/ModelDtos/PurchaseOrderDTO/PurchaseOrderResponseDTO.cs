using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.PurchaseOrderDTO
{
    public  class PurchaseOrderResponseDto
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public int CreatorId { get; set; }//hardcoded branches in the frontend: would go as a id only
        public int VendorId { get; set; }//name or whole detail?
        public int CustomerId { get; set; }
        public int SourceStateId { get; set; }
        public int DestinationStateId { get; set; }
        public string? Reference { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string PaymentTerms { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PurchaseStatus { get; set; } = string.Empty;

        public List<SelectedItemsDto> Item { get; set; } = [];
    }
}
