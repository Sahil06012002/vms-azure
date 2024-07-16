using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DataContext _db;
        public AddressRepository(DataContext db)
        {
            _db = db;
        }

        public IEnumerable<object> RelatedAddresses(List<int> vendorId)
        {
            var addresses = _db.Addresses
                                .Where(a => vendorId.Contains(a.VendorId))
                                .Select(a => new
                                {
                                    a.Id,
                                    a.VendorId,
                                    AddressType = a.AddressType.ToString(),
                                    a.Attention,
                                    Country = a.Country.ToString(),
                                    a.AddressLine1,
                                    a.AddressLine2,
                                    a.City,
                                    State = a.state!=null?a.state.Name:"Unknown State",
                                    a.PinCode,
                                    a.Phone,
                                    a.FaxNumber
                                })
                                .ToList();
            if (!addresses.Any() )
            {
                return [];
            }
            return addresses;
        }
    }
}
