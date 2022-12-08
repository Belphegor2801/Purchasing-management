using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Purchasing_management.Data.Entity
{
    public class PurchaseOrder
    {
        [Key]
        public Guid Id { get; set; }
        
        public Guid DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public string RegistantName { get; set; }

        public Guid CreateByUserId { get; set; }

        public virtual ICollection<Supply> SupplyList { get; set; }
    }
}
