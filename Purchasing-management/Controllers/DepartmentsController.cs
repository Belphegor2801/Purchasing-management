using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Purchasing_management.Data.Entity;
using Purchasing_management.Business;
using Purchasing_management.Common;

namespace Purchasing_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentHandler _departmentHandler;
        public DepartmentsController(DepartmentHandler departmentHandler)
        {
            _departmentHandler = departmentHandler;
        }
        
        // GET: api/Departments
        [HttpGet]
        public async Task<IActionResult> GetDepartments([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _departmentHandler.GetDepartments(paginationRequest);
            return Helper.TransformData(result);
        }

        // GET: api/Departments/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetDepartment(Guid id)
        {
            var result = await _departmentHandler.GetDepartment(id);
            return Helper.TransformData(result);
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditDepartment(Guid id, DepartmentUpdateModel department)
        {
            var result = await _departmentHandler.EditDepartment(id, department);
            return Helper.TransformData(result);
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add", Name="AddDepartment")]
        public async Task<IActionResult> AddDepartment(DepartmentCreateModel department)
        {
            var result = await _departmentHandler.AddDepartment(department);
            return Helper.TransformData(result);
        }

        // DELETE: api/Departments/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid departmentId)
        {
            var result = await _departmentHandler.DeleteDepartment(departmentId);
            return Helper.TransformData(result);
        }
    }
}
