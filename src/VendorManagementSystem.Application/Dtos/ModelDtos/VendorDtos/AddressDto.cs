using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos
{
    public class AddressDto
    {
        // address
        public string Attention { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int StateId { get; set; }
        public string PinCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FaxNumber { get; set; } = string.Empty;
    }
}
