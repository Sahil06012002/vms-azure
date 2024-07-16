using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Dynamic.Core;
using VendorManagementSystem.Application.Dtos.ModelDtos;
using VendorManagementSystem.Application.Dtos.ModelDtos.DashBoard;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Dtos.UtilityDtos.Vendor;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Models.Enums;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Repository
{
    public class VendorRepository : IVendorRepository
    {

        private readonly DataContext _db;

        public VendorRepository(DataContext db)
        {
            _db = db;
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _db.Database.BeginTransaction();
        }


        public int AddVendor(VendorNew vendor)
        {
            _db.VendorsNew.Add(vendor);
            _db.SaveChanges();
            return vendor.Id;
        }

        public bool AddVendorAddress(List<Address> addresses)
        {
            _db.Addresses.AddRange(addresses);
            _db.SaveChanges();
            return true;
        }

        public int AddVendorAddress(Address address)
        {
            _db.Addresses.Add(address);
            _db.SaveChanges();
            return address.Id;
        }

        public int AddVendorPrimaryContact(PrimaryContact primaryContact)
        {
            _db.PrimaryContact.Add(primaryContact);
            _db.SaveChanges();
            return primaryContact.Id;
        }

        private IQueryable<VendorNewResponseDto> GetVendorsNew(IQueryable<VendorNew> query)
        {
            var currencyType = typeof(Currency);
            var paymentTermsType = typeof(PaymentTerms);
            var countryType = typeof(Country);
            var salutionType = typeof(Salutation);

            IQueryable<VendorNewResponseDto> query2 = query.Select(v => new VendorNewResponseDto
            {
                VendorId = v.Id,
                CompanyName = v.CompanyName,
                GSTIN = v.GSTIN,
                Currency = Enum.GetName(currencyType, v.Currency) ?? "NA",
                PaymentTerms = Enum.GetName(paymentTermsType, v.PaymentTerms) ?? "NA",
                TDS = string.Format("{0} ({1})", v.TDS!=null?v.TDS.Name:string.Empty, v.TDS != null ? v.TDS.Percentage:string.Empty),
                Type = v.VendorType!=null?v.VendorType.Name:string.Empty,
                Status = v.Status,
                Categories = _db.VendorCategoryMappings.Where(vcm => vcm.VendorId == v.Id).Select(vcm => vcm.Category!.Name).ToList(),
                PrimaryContact = v.Contacts!= null? new PrimaryContactResponseDto
                {
                    Id =v.Contacts.Id,
                    Salutation =  Enum.GetName(salutionType, v.Contacts.Salutation!) ?? "NA",
                    FirstName = v.Contacts.FirstName??"",
                    LastName = v.Contacts.LastName??"",
                    Email = v.Contacts.Email ?? "",
                    WorkPhone = v.Contacts.WorkPhone ?? "",
                    MobilePhone =  v.Contacts.MobilePhone ?? "",
                } : new PrimaryContactResponseDto(),
                ShippingAddress = _db.Addresses.Where(a => v.Id == a.VendorId && a.AddressType == AddressTypes.Shipping).Select(a => new AddressResponseDto
                {
                    AddressId = a.Id,
                    Attention = a.Attention ?? "",
                    Country = a.Country!=null?Enum.GetName(countryType, a.Country) ?? "NA":"NA",
                    AddressLine1 = a.AddressLine1??"",
                    AddressLine2 = a.AddressLine2??"",
                    City = a.City ?? "",
                    State = a.state!=null?a.state.Name:"",
                    PinCode = a.PinCode ?? "",
                    Phone = a.Phone??"",
                    FaxNumber = a.FaxNumber ?? "",
                }).FirstOrDefault()??new AddressResponseDto(),
                BillingAddress = _db.Addresses.Where(a => v.Id == a.VendorId && a.AddressType == AddressTypes.Billing).Select(a => new AddressResponseDto
                {
                    AddressId = a.Id,
                    Attention = a.Attention??"",
                    Country = a.Country != null ? Enum.GetName(countryType, a.Country) ?? "NA" : "NA",
                    AddressLine1 = a.AddressLine1 ?? "",
                    AddressLine2 = a.AddressLine2 ?? "",
                    City = a.City??"",
                    State = a.state != null ? a.state.Name : "",
                    PinCode = a.PinCode ?? "",
                    Phone = a.Phone??"",
                    FaxNumber = a.FaxNumber??"",
                }).FirstOrDefault()??new AddressResponseDto(),
            });
            return query2;
            }
        public VendorNewResponseDto? GetVendorById(int vendorId)
        {
            IQueryable<VendorNew> vendorQuery = _db.VendorsNew.Where(v => v.Id == vendorId);
            var vendor = GetVendorsNew(vendorQuery).FirstOrDefault();
            return vendor;
        }
       
        public IEnumerable<VendorNewResponseDto> GetVendorsNew(string? filter, int cursor, int size, bool next)
        {
            IQueryable<VendorNew> query;
            if(next)
            {
                if (cursor == 0)
                {
                    query = _db.VendorsNew.OrderByDescending(v => v.Id);
                }
                else
                {
                    query = _db.VendorsNew.OrderByDescending(v => v.Id).Where(v => v.Id < cursor);
                }
            }else
            {
                query = _db.VendorsNew.OrderByDescending(v => v.Id).Where(v => v.Id > cursor);
            }

            if(!string.IsNullOrEmpty(filter))
            {
                query = query.Where(vendor => vendor.CompanyName.Contains(filter) || vendor.Contacts!=null && vendor.Contacts.FirstName!=null && vendor.Contacts.FirstName.Contains(filter));
            }
            IQueryable<VendorNew> VendorNewQuery = query.Take(size);
            var result = GetVendorsNew(VendorNewQuery);
            return result.ToList();
        }

        public bool UpdateVendor(int vendorId, List<UpdateColumnDto> vendorColumns, string currentUser)
        {
            var vendor = _db.VendorsNew.Where(v => v.Id == vendorId).FirstOrDefault();
            if(vendor == null)
            {
                throw new ArgumentException($"Vendor with id ${vendorId} not found");
            }
            InfrastructureUtility.UpdateModel(vendor, vendorColumns);
            vendor. UpdatedBy = int.Parse(currentUser);
            vendor.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();
            return true;
        }

        public bool UpdateAddress(int vendorId, List<UpdateColumnDto> addressColumns, AddressTypes addressType, int currentUser)
        {
           
            var address =  _db.Addresses.Where(address => address.VendorId == vendorId && address.AddressType == addressType).FirstOrDefault();
            if (address == null)
            {
                throw new ArgumentException($"Address associated with vendor of id {vendorId} not found");
            }
            InfrastructureUtility.UpdateModel(address, addressColumns);
            address.UpdatedBy = currentUser;
            address.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();
            return true;
        }
        public bool UpdatePrimaryContact(int vendorId, List<UpdateColumnDto> primaryContactColumns, int currentUser)
        {
            var primaryContact = _db.PrimaryContact.Where(pc => pc.VendorId == vendorId).FirstOrDefault();
            if (primaryContact == null)
            {
                throw new ArgumentException($"PrimaryContact associated with vendor of id {vendorId} not found");
            }
            InfrastructureUtility.UpdateModel(primaryContact, primaryContactColumns);
            primaryContact.UpdatedBy = currentUser;
            primaryContact.UpdatedAt = DateTime.UtcNow;
            _db.SaveChanges();
            return true;
        }
        public bool ToggleStatus(int vendorId,int currentUser)
        {
            VendorNew? vendor = _db.VendorsNew.Where(v => v.Id == vendorId).FirstOrDefault();
            if(vendor != null)
            {
                vendor.Status = !vendor.Status;
                vendor.UpdatedAt = DateTime.UtcNow;
                vendor.UpdatedBy = currentUser;
                int change = _db.SaveChanges();
                return change > 0;
            }
            else
            {
                throw new ArgumentNullException($"Vendor with id {vendorId} didn't found in db");
            } 
        }

        public CountDto GetVendorCount()
        {
            var statusCount = _db.VendorsNew.GroupBy(v => v.Status).Select(g => new { Status = g.Key, StatusCount = g.Count() }).ToList();
            int activeCount = 0;
            int inactiveCount = 0;
            foreach (var item in statusCount)
            {
                if (item.Status)
                {
                    activeCount = item.StatusCount;
                }
                else
                {
                    inactiveCount = item.StatusCount;
                }
            }
            CountDto result = new CountDto()
            {
                active = activeCount,
                inactive = inactiveCount,
            };
            return result;
        }
        public IEnumerable<VendorFormTypesDto> GetTypesForForm()
        {
            IEnumerable<VendorFormTypesDto> types =
                    _db.VendorTypes.Select(t => new VendorFormTypesDto { Id = t.Id, Name = t.Name, }).ToList();
            return types;
        }

        public Address? GetVendorAddress(int vendorId, AddressTypes type)
        {
            return _db.Addresses.Where(a => a.VendorId == vendorId && a.AddressType == type).FirstOrDefault();
        }

        public IEnumerable<object> VednorFormDetails()
        {

            var vendorDetails = _db.VendorsNew.OrderByDescending(v => v.Id).Where(v => v.Status).Select(v => new
            {
                v.Id,
                v.CompanyName,
                Email=v.Contacts!=null?v.Contacts.Email:string.Empty,
                FirstName= v.Contacts != null ? v.Contacts.FirstName:string.Empty,
                MobilePhone = v.Contacts != null ? v.Contacts.MobilePhone:string.Empty
            }).ToList();
            return vendorDetails;
        }
        public bool HasNeighbour(string filter,int id, bool next)
        {
            IQueryable<VendorNew> query = _db.VendorsNew;
            if(!filter.IsNullOrEmpty())
            {
                query = query.Where(v => v.CompanyName.Contains(filter) || v.Contacts!=null && v.Contacts.FirstName!=null && v.Contacts.FirstName.Contains(filter));
            }
            query = query.OrderByDescending(v => v.Id);
            var result  = next ? query.Any(v => v.Id < id) : 
            query.Any(v => v.Id > id);
            return result;
        }
        public VendorDashBoardDetailsDto GetVendorDashBoradData()
        {
            int activeVendorsCount = GetVendorCount().active;
            var vendorTypeCount = _db.VendorsNew.Where(vendor => vendor.Status).GroupBy(v => v.VendorType, v => v.Id, (key, vendors) => new DashBoardVendorCountDto { Type = key!=null?key.Name:"Unknown-Type", Count=vendors.Count()}).ToList();
            return new VendorDashBoardDetailsDto
            {
                VendorCount = activeVendorsCount,
                TypeCount = vendorTypeCount,
            };
        }
    }
}
