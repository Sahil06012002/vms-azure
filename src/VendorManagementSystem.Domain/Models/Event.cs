using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorManagementSystem.Models.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "varchar(255)")]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(12,3)")]
        public decimal Budget { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string? Link { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public User? Creator { get; set; }

        [ForeignKey("UpdatedBy")]
        public User? Updater { get; set; }

    }
}
