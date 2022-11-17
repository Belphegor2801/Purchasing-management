using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using Purchasing_management.Common;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace Purchasing_management.Business.Services
{
    public class Department_Management
    {
        private readonly PurchasingDBContext _dbcontext;
        private readonly ILogger<Department_Management> _logger;

        public Department_Management(PurchasingDBContext DBContext, ILogger<Department_Management> logger)
        {
            _dbcontext = DBContext;
            _logger = logger;
        }

        public void AddDepartment(Department department)
        {
            _logger.LogInformation("Start adding department.");
            try
            {
                string Name = department.Name;
                _logger.LogInformation("Add department - Name: {@Name}", Name);
                _dbcontext.Departments.Add(department);
                _dbcontext.SaveChanges();
                _logger.LogInformation("End adding department: Success");

            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End adding department: Fail");
            }
        }

        public void EditDepartment(int id, Department department)
        {
            _logger.LogInformation("Start editing department.");
            if (id != department.Id)
            {
                _logger.LogError("Id not invalid");
            }
            try
            {
                _dbcontext.Entry(department).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbcontext.SaveChangesAsync();
                string Name = department.Name;
                _logger.LogInformation("Edit department - Name: {@Name}", Name);
                _logger.LogInformation("End editing department: Success");
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End editing department: Fail");
            }
        }

        public Department DeleteDepartment(int id)
        {
            _logger.LogInformation("Start deleting department");
            Department department = _dbcontext.Departments.Find(id);
            if (department == null)
            {
                _logger.LogError("Not found");
                _logger.LogInformation("End getting department by Id: Fail");
                return null;
            }

            _dbcontext.Departments.Remove(department);
            _dbcontext.SaveChangesAsync();
            _logger.LogInformation("Deleted department with Id: {@id}", id);
            _logger.LogInformation("End deleting department: Success");
            return department;
        }

        public IList<Department> GetDepartments(int page, int size)
        {
            _logger.LogInformation("Start getting department");
            var validFilter = new Pagination<Department>(page, size);

            IList<Department> departments = _dbcontext.Departments
                                            .Skip((validFilter.Page - 1) * validFilter.Size)
                                            .Take(validFilter.Size)
                                            .ToList<Department>();
            _logger.LogInformation("End getting department.");
            return departments;
        }

        public Department GetDepartment(int id)
        {
            _logger.LogInformation("Start getting department by Id");
            Department department;
            try
            {
                _logger.LogInformation("Getting department with Id: {@id}", id);
                department = _dbcontext.Departments.Find(id);
                if (department == null)
                {
                    _logger.LogError("Not found department with Id: {@id}", id);
                    _logger.LogInformation("End getting department by Id: Fail");
                    return null;
                }
                _logger.LogInformation("End getting department by Id: Success");
            }
            catch (Exception ex)
            {
                department = null;
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End getting department by Id: Fail");

            }
            return department;
        }
    }
}
