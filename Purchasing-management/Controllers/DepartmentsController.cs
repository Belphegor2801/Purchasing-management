using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using Purchasing_management.Business.Services;
using Purchasing_management.Common;

namespace Purchasing_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly Department_Management _departmentManagement;
        public DepartmentsController(Department_Management departmentManagement)
        {
            _departmentManagement = departmentManagement;
        }

        // GET: api/Departments
        [HttpGet]
        public ActionResult<IEnumerable<Department>> GetDepartments([FromQuery] Pagination<Department> filter)
        {
            IList<Department> departments = _departmentManagement.GetDepartments(filter.Page, filter.Size);
            return Ok(departments);
        }

        // GET: api/Departments/5
        [HttpGet("get/{id}")]
        public ActionResult<Department> GetDepartment(int id)
        {
            var department = _departmentManagement.GetDepartment(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("edit/{id}")]
        public ActionResult EditDepartment(int id, Department department)
        {
            _departmentManagement.EditDepartment(id, department);
            return Ok();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add", Name="AddDepartment")]
        public ActionResult<Department> AddDepartment(Department department)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _departmentManagement.AddDepartment(department);

            return CreatedAtRoute("AddDepartment", new { id = department.Id }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            Department department = _departmentManagement.DeleteDepartment(id);
            if (department == null) return BadRequest();
            else return Ok(department);
        }
    }
}
