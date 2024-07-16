using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.VendorDtos.Response
{
    public class PrimaryContactResponseDto
    {
        public int Id { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string WorkPhone { get; set; } = string.Empty;
        public string MobilePhone { get; set; } = string.Empty;
    }
}
