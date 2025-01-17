﻿using System.ComponentModel.DataAnnotations.Schema;

namespace VendorManagementSystem.Models.Models
{
    public class VendorType
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = string.Empty;
    }
}
