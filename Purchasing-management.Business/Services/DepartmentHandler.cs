using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using Purchasing_management.Common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using LinqKit;
using System.Linq.Expressions;
using System.Linq;

namespace Purchasing_management.Services
{
    public class DepartmentHandler
    {
        private readonly PurchasingDBContext _dbcontext;
        private readonly ILogger<DepartmentHandler> _logger;
        private readonly DbHandler<Department, Department, PaginationRequest> _dbHandler = DbHandler<Department, Department, PaginationRequest>.Instance;

        public DepartmentHandler(PurchasingDBContext DBContext, ILogger<DepartmentHandler> logger)
        {
            _dbcontext = DBContext ?? throw new ArgumentNullException(nameof(DBContext)); ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        public async Task<Response> AddDepartment(Department department)
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
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        public async Task<Response> EditDepartment(Guid departmentId, Department department)
        {
            _logger.LogInformation("Start editing department.");
            if (departmentId != department.Id)
            {
                _logger.LogError("DepartmentId not invalid");
                return new ResponseError(System.Net.HttpStatusCode.NotFound, "DepartmentId not found!");
            }
            try
            {
                _dbcontext.Entry(department).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbcontext.SaveChanges();
                _logger.LogInformation("Edit department - Name: {@Name}", department.Name);
                _logger.LogInformation("End editing department: Success");
                return new ResponseUpdate(System.Net.HttpStatusCode.OK, "Edit Department: Success", departmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End editing department: Fail");
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        public async Task<Response> DeleteDepartment(Guid departmentId)
        {
            try
            {
                _logger.LogInformation("Start deleting department");
                Department department = _dbcontext.Departments.Find(departmentId);

                if (department == null)
                {
                    _logger.LogError("DepartmentId Not found");
                    _logger.LogInformation("End deleting department: Fail");
                    return new ResponseError(System.Net.HttpStatusCode.NotFound, "DepartmentId Not found");
                }

                _dbcontext.Departments.Remove(department);
                _dbcontext.SaveChanges();
                _logger.LogInformation("Deleted department with Id: {@id}", departmentId);
                _logger.LogInformation("End deleting department: Success");

                return new ResponseDelete(System.Net.HttpStatusCode.OK, "Delete Success", departmentId, "");
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End deleting department: Fail");
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        public async Task<Response> GetDepartments(PaginationRequest paginationRequest)
        {
            try
            {
                _logger.LogInformation("Start getting department");
                var predicate = BuildQuery(paginationRequest);
                var result = await _dbHandler.GetPageAsync(predicate, paginationRequest);
                _logger.LogInformation("End getting department.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End getting department: Fail");
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        public async Task<Response> GetDepartment(Guid departmentId)
        {
            _logger.LogInformation("Start getting department by Id");
            Department department;
            try
            {
                _logger.LogInformation("Getting department with Id: {@id}", departmentId);
                department = _dbcontext.Departments.Find(departmentId);
                if (department == null)
                {
                    _logger.LogError("Not found department with Id: {@id}", departmentId);
                    _logger.LogInformation("End getting department by Id: Fail");
                    return new ResponseError(System.Net.HttpStatusCode.BadRequest, "DepartmentId not found! ");
                }
                _logger.LogInformation("End getting department by Id: Success");
                return new ResponseObject<Department>(department);
            }
            catch (Exception ex)
            {
                department = null;
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End getting department by Id: Fail");
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        bool DepartmentExists(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
                throw new ArgumentNullException(nameof(departmentId));

            return _dbcontext.Departments.Any(a => a.Id == departmentId);
        }


        private Expression<Func<Department, bool>> BuildQuery(PaginationRequest query)
        {
            var predicate = PredicateBuilder.New<Department>(true);

            if (!string.IsNullOrEmpty(query.FullTextSearch))
                predicate.And(s => s.Name.Contains(query.FullTextSearch)
                || s.Manager.Contains(query.FullTextSearch.ToLower()));

            return predicate;
        }
    }
}
