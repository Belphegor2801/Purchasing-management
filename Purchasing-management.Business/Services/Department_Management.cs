using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using System.Collections.Generic;
using System.Linq;

using Purchasing_management.Common;

namespace Purchasing_management.Business.Services
{
    public class Department_Management
    {
        private readonly PurchasingDBContext _dbcontext;

        public Department_Management(PurchasingDBContext DBContext)
        {
            _dbcontext = DBContext;
        }

        public void AddDepartment(Department department)
        {
            _dbcontext.Departments.Add(department);
            _dbcontext.SaveChanges();
        }

        public void EditDepartment(Department department)
        {
            _dbcontext.Entry(department).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbcontext.SaveChangesAsync();
        }

        public Department DeleteDepartment(int Id)
        {
            Department department = _dbcontext.Departments.Find(Id);
            if (department == null)
            {
                return null;
            }

            _dbcontext.Departments.Remove(department);
            _dbcontext.SaveChangesAsync();
            return department;
        }

        public IList<Department> GetDepartments(int page, int size)
        {
            var validFilter = new Pagination<Department>(page, size);

            IList<Department> departments = _dbcontext.Departments
                                            .Skip((validFilter.Page - 1) * validFilter.Size)
                                            .Take(validFilter.Size)
                                            .ToList<Department>();
            return departments;
        }

        public Department GetDepartment(int id)
        {
            Department department = _dbcontext.Departments.Find(id);
            return department;
        }
    }
}
