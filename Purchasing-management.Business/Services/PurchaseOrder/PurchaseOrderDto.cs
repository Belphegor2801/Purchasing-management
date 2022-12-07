using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing_management.Business
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }

        public string DepartmentName { get; set; }

        public string RegistantName { get; set; }

        public virtual ICollection<SupplyDto> SupplyList { get; set; }
    }

    public class PurchaseOrderCreateModel
    {
        public string RegistantName { get; set; }

        public virtual ICollection<SupplyDto> SupplyList { get; set; }
    }

    public class PurchaseOrderUpdateModel
    {
        public string RegistantName { get; set; }

        public virtual ICollection<SupplyDto> SupplyList { get; set; }
    }

    public class SupplyDto
    {
        public string Name { get; set; }
        public string Amount { get; set; }
    }

}
