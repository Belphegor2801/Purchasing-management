using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Purchasing_management.Data.Entity
{
    public class Supply
    {
        [Key]
        public Guid Id { get; set; }
        public int OrdinalNumber { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }

        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
    }
}
