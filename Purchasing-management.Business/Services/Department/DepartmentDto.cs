using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing_management.Business
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OrdinalNumber { get; set; }
    }

    public class DepartmentUpdateModel
    {
        public string Name { get; set; }
        public string Manager { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OrdinalNumber { get; set; }
    }

    public class DepartmentCreateModel
    {
        public string Name { get; set; }
        public string Manager { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OrdinalNumber { get; set; }
    }
}
