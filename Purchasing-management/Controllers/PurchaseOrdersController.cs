using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using Purchasing_management.Business;
using Purchasing_management.Common;

namespace Purchasing_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly PurchasingHandler _purchaseOrderHandler;
        public PurchaseOrdersController(PurchasingHandler purchaseOrderManagement)
        {
            _purchaseOrderHandler = purchaseOrderManagement;
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders([FromQuery] PaginationRequest paginationRequest)
        {
            var result = await _purchaseOrderHandler.GetPurchaseOrders(paginationRequest);
            return Helper.TransformData(result);
        }

        // GET: api/PurchaseOrders/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetPurchaseOrder(Guid id)
        {
            var result = await _purchaseOrderHandler.GetPurchaseOrder(id);
            return Helper.TransformData(result);
        }

        // PUT: api/PurchaseOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditPurchaseOrder(Guid id, Guid departmentId, PurchaseOrderUpdateModel purchaseOrder)
        {
            var result = await _purchaseOrderHandler.EditPurchaseOrder(id, departmentId, purchaseOrder);
            return Helper.TransformData(result);
        }

        // POST: api/PurchaseOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add", Name="AddPurchaseOrder")]
        public async Task<IActionResult> AddPurchaseOrder(PurchaseOrderCreateModel purchaseOrder)
        {
            var result = await _purchaseOrderHandler.AddPurchaseOrder(purchaseOrder);
            return Helper.TransformData(result);
        }

        // DELETE: api/PurchaseOrders/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(Guid purchaseOrderId)
        {
            var result = await _purchaseOrderHandler.DeletePurchaseOrder(purchaseOrderId);
            return Helper.TransformData(result);
        }
    }
}
