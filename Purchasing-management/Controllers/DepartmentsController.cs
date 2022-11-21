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
        private readonly Department_Manager _departmentManagement;
        public DepartmentsController(Department_Manager departmentManagement)
        {
            _departmentManagement = departmentManagement;
        }

        // GET: api/Departments
        [HttpGet]
        public ActionResult<IList<Department>> GetDepartments([FromQuery] Pagination<Department> filter)
        {
            ResponsePagination<Department> response = _departmentManagement.GetDepartments(filter.Page, filter.Size);
            return response.Data.Content;
        }

        // GET: api/Departments/5
        [HttpGet("get/{id}")]
        public ActionResult<Department> GetDepartment(int id)
        {
            var response = _departmentManagement.GetDepartment(id);
            return response.Data;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("edit/{id}")]
        public ActionResult EditDepartment(int id, Department department)
        {
            var response = _departmentManagement.EditDepartment(id, department);
            return Ok();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add", Name="AddDepartment")]
        public ActionResult<Department> AddDepartment(Department department)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response =  _departmentManagement.AddDepartment(department);

            return CreatedAtRoute("AddDepartment", new { id = department.Id }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var reponse = _departmentManagement.DeleteDepartment(id);
            return Ok(reponse);
        }
    }
}
