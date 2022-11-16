using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Purchasing_management.Data.Entity
{
    public class Supply
    {
        [Key]
        public int Id { get; set; }
        public int OrdinalNumber { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
    }
}
