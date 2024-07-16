﻿using System.ComponentModel.DataAnnotations.Schema;

namespace VendorManagementSystem.Models.Models
{
    public class PurchasedItem
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ItemId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Account { get; set; } = string.Empty;
        [Column(TypeName = "decimal(10,3)")]
        public decimal Rate { get; set; }
        [Column(TypeName = "decimal(6,2)")]
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(6,3)")]
        public int Tax { get; set; }
        [Column(TypeName = "decimal(10,3)")]
        public decimal Amount { get; set; }
        [ForeignKey(nameof(PurchaseOrderId))]
        public PurchaseOrder? PurchaseOrder { get; set; }
        [ForeignKey(nameof(ItemId))]
        public Item? Item { get; set; }
    }
}
