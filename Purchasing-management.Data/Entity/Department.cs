using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Purchasing_management.Data.Entity
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OrdinalNumber { get; set; }
    }
}
