using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.IRepository
{
    public interface IAddressRepository
    {
        public IEnumerable<object> RelatedAddresses(List<int> vendorId);
    }
}
