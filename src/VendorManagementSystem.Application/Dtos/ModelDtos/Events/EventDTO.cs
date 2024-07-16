using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Application.Dtos.ModelDtos.Events
{
    public class EventDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        [Required]
        public int Budget { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string Link { get; set; } = string.Empty;
    }
}
