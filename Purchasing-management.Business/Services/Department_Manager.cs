using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using Purchasing_management.Common;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace Purchasing_management.Business.Services
{
    public class Department_Manager
    {
        private readonly PurchasingDBContext _dbcontext;
        private readonly ILogger<Department_Manager> _logger;

        public Department_Manager(PurchasingDBContext DBContext, ILogger<Department_Manager> logger)
        {
            _dbcontext = DBContext;
            _logger = logger;
        }

        public Response AddDepartment(Department department)
        {
            _logger.LogInformation("Start adding department.");
            try
            {
                string Name = department.Name;
                _dbcontext.Departments.Add(department);
                _dbcontext.SaveChanges();
                _logger.LogInformation("Add department - Name: {@Name}", Name);
                _logger.LogInformation("End adding department: Success");
                return new Response(System.Net.HttpStatusCode.OK, "Add Department Success!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End adding department: Fail");
                return new Response(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        public ResponseUpdate EditDepartment(int id, Department department)
        {
            _logger.LogInformation("Start editing department.");
            if (id != department.Id)
            {
                _logger.LogError("Id not invalid");
                return new ResponseUpdate(System.Net.HttpStatusCode.NotFound, "Id not found!!", ToGuid(id));
            }
            try
            {
                _dbcontext.Entry(department).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbcontext.SaveChangesAsync();
                string Name = department.Name;
                _logger.LogInformation("Edit department - Name: {@Name}", Name);
                _logger.LogInformation("End editing department: Success");
                return new ResponseUpdate(System.Net.HttpStatusCode.OK, "Edit Department: Success", ToGuid(id));
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End editing department: Fail");
                return new ResponseUpdate(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex, ToGuid(id));
            }
        }

        public ResponseDelete DeleteDepartment(int id)
        {
            _logger.LogInformation("Start deleting department");
            Department department = _dbcontext.Departments.Find(id);
            
            if (department == null)
            {
                _logger.LogError("Not found");
                _logger.LogInformation("End getting department by Id: Fail");
                return new ResponseDelete(System.Net.HttpStatusCode.NotFound, "Id Not found", ToGuid(id), "");
            }

            _dbcontext.Departments.Remove(department);
            _dbcontext.SaveChangesAsync();
            _logger.LogInformation("Deleted department with Id: {@id}", id);
            _logger.LogInformation("End deleting department: Success");

            return new ResponseDelete(System.Net.HttpStatusCode.OK, "Delete Success", ToGuid(id), "");
        }

        public ResponsePagination<Department> GetDepartments(int page, int size)
        {
            var validFilter = new Pagination<Department>(page, size);
            ResponsePagination<Department> responsePagination = new ResponsePagination<Department>(validFilter);
            try
            {
                _logger.LogInformation("Start getting department");
                IList<Department> departments = _dbcontext.Departments
                                                .Skip((responsePagination.Data.Page - 1) * responsePagination.Data.Size)
                                                .Take(responsePagination.Data.Size)
                                                .ToList<Department>();
                _logger.LogInformation("End getting department.");
                responsePagination.Code = System.Net.HttpStatusCode.OK;
                responsePagination.Message = "OK";
                responsePagination.Data.Content = departments.ToList<Department>();
                return responsePagination;
            }
            catch (Exception ex)
            {
                responsePagination.Code = System.Net.HttpStatusCode.BadRequest;
                responsePagination.Message = "Something went wrong! " + ex;
                responsePagination.Data.Content = null;
                return responsePagination;
            }
        }

        public Response<Department> GetDepartment(int id)
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
                }
                _logger.LogInformation("End getting department by Id: Success");
                return new Response<Department>(System.Net.HttpStatusCode.OK, department);
            }
            catch (Exception ex)
            {
                department = null;
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End getting department by Id: Fail");
                return new Response<Department>(System.Net.HttpStatusCode.BadRequest, department, "Something went wrong!" + ex);
            }
        }

        private static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}
