using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VendorManagementSystem.Application.Dtos.ModelDtos.DashBoard;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos;
using VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application.Services
{
    public class UtilityService : IUtilityService
    { 
        private readonly IVendorRepository _vendorRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        public UtilityService(IVendorRepository vendorRepository, IInvoiceRepository invoiceRepository)
        {
            _vendorRepository = vendorRepository;
            _invoiceRepository = invoiceRepository; 
        }

        public ApplicationResponseDto<CountDto> VendorCount()
        {
           var response =  _vendorRepository.GetVendorCount();
            return new ApplicationResponseDto<CountDto>
            {
                Data = response,
            };
        }
        public ApplicationResponseDto<CountDto> InvoiceCount()
        {
            var response = _invoiceRepository.GetInvoiceCount();
            return new ApplicationResponseDto<CountDto>
            {
                Data = response,
            };
        }
        public T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, false);
        }
        public ApplicationResponseDto<Dictionary<string, List<string>>> ExtractPropertyNames(string type)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            if(string.Equals(type, "vendor", StringComparison.OrdinalIgnoreCase))
            {
                dict["vendor"] = ExtractPropertyNames(new CreateVendorNewDto());
                dict["address"] = ExtractPropertyNames(new Address());
                dict["primaryContact"] = ExtractPropertyNames(new PrimaryContact());

            }

            return new ApplicationResponseDto<Dictionary<string, List<string>>>
            {
                Data = dict,
            };
        }

        public List<string> ExtractPropertyNames<T>(T objectToExtract)
        {
            List<string> result = new List<string>();
            if(!object.Equals(objectToExtract, default(T)))
            {
                Type type = objectToExtract!.GetType();

                PropertyInfo[] propertyInfos = type.GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    result.Add(propertyInfo.Name);
                }
                return result;
            }
            return result;
        }

        public ApplicationResponseDto<string> SaveCanvasSignature(string signature)
        {
            try
            {
                string outPutPath = "signature.png";
                if (signature.Contains(','))
                {
                    signature = signature.Substring(signature.IndexOf(',') + 1);
                }
                byte[] imageBytes = Convert.FromBase64String(signature);
                File.WriteAllBytes(outPutPath, imageBytes);
                return new ApplicationResponseDto<string>
                {
                    Message = "Image Saved Successfully",
                };
            }
            catch(Exception ex)
            {
                return new ApplicationResponseDto<string>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.AzureError,
                        Message = new List<string> { ex.Message }
                    },
                    Message = "Error while saving signature"
                };
            }
        }

        public ApplicationResponseDto<DashBoardResponseDto> DashBoardData()
        {
           try
            {
                VendorDashBoardDetailsDto vendor = _vendorRepository.GetVendorDashBoradData();
                ExpenditureDto expenditure = _invoiceRepository.GetTopCategories(4);
                return new ApplicationResponseDto<DashBoardResponseDto>
                {
                    Data = new DashBoardResponseDto
                    {
                        VendorDetails = vendor,
                        Expenditure = expenditure,
                    },
                };
            }catch(Exception ex)
            {
                return new ApplicationResponseDto<DashBoardResponseDto>
                {
                    Error = new()
                    {
                        Code = (int)ErrorCodes.AzureError,
                        Message = new List<string> { ex.Message }
                    },
                    Message = "Error while saving signature"
                };
            }
        }

        public string GenerateIdentifier(string prefix, int id, int length)
        {
            string numericPart = id.ToString().PadLeft(length, '0'); //convert id to 9 digit
            return prefix + numericPart;
        }

    }
}
