using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VendorManagementSystem.Models.Enums;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos
{
    public class CreateVendorNewDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string GSTIN { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string PaymentTerms { get; set; } = string.Empty;
        public int TDSId { get; set; }
        public int TypeId { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();



        // primary contact
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string WorkPhone { get; set; } = string.Empty;
        public string MobilePhone { get; set; } = string.Empty;

        // address
        public AddressDto? ShippingDto { get; set; }
        public AddressDto? BillingDto { get; set; }
    }
}
