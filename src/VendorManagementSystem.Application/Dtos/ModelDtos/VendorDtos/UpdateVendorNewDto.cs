using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos
{
    public class UpdateVendorNewDto
    {
        public int Id { get; set; }
        public List<int> Categories { get; set; } = [];
        public List<UpdateColumnDto> VendorColumns { get; set; } = [];
        public List<UpdateColumnDto> ShippingAddressColumns { get; set; } = [];
        public List<UpdateColumnDto> BillingAddressColumns { get; set; } = [];
        public List<UpdateColumnDto> PrimaryContacts { get; set; } = [];
    }
}
