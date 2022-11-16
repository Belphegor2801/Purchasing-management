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
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly Purchasing_Management _purchaseOrderManagement;
        public PurchaseOrdersController(Purchasing_Management purchaseOrderManagement)
        {
            _purchaseOrderManagement = purchaseOrderManagement;
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrders([FromQuery] Pagination<PurchaseOrder> filter)
        {
            IList<PurchaseOrder> purchaseOrders = _purchaseOrderManagement.GetPurchaseOrders(filter.Page, filter.Size);
            return Ok(purchaseOrders);
        }

        // GET: api/PurchaseOrders/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrder(int id)
        {
            var purchaseOrder = _purchaseOrderManagement.GetPurchaseOrder(id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return purchaseOrder;
        }

        // PUT: api/PurchaseOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditPurchaseOrder(int id, PurchaseOrder purchaseOrder)
        {
            _purchaseOrderManagement.EditPurchaseOrder(purchaseOrder);
            return Ok();
        }

        // POST: api/PurchaseOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add", Name = "AddPurchaseOrder")]
        public async Task<ActionResult<PurchaseOrder>> AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _purchaseOrderManagement.AddPurchaseOrder(purchaseOrder);

            return CreatedAtRoute("AddPurchaseOrder", new { id = purchaseOrder.Id }, purchaseOrder);
        }

        // DELETE: api/PurchaseOrders/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(int id)
        {
            PurchaseOrder purchaseOrder = _purchaseOrderManagement.DeletePurchaseOrder(id);
            if (purchaseOrder == null) return BadRequest();
            else return Ok(purchaseOrder);
        }
    }
}
