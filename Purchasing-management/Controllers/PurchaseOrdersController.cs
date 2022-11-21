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
        private readonly Purchasing_Manager _purchaseOrderManagement;
        public PurchaseOrdersController(Purchasing_Manager purchaseOrderManagement)
        {
            _purchaseOrderManagement = purchaseOrderManagement;
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public ActionResult<IEnumerable<PurchaseOrder>> GetPurchaseOrders([FromQuery] Pagination<PurchaseOrder> filter)
        {
            ResponsePagination<PurchaseOrder> response = _purchaseOrderManagement.GetPurchaseOrders(filter.Page, filter.Size);
            return response.Data.Content;
        }

        // GET: api/PurchaseOrders/5
        [HttpGet("get/{id}")]
        public ActionResult<PurchaseOrder> GetPurchaseOrder(int id)
        {
            Response<PurchaseOrder> response = _purchaseOrderManagement.GetPurchaseOrder(id);

            if (response.Data == null)
            {
                return NotFound();
            }

            return response.Data;
        }

        // PUT: api/PurchaseOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("edit/{id}")]
        public ActionResult EditPurchaseOrder(int id, PurchaseOrder purchaseOrder)
        {
            Response response = _purchaseOrderManagement.EditPurchaseOrder(id, purchaseOrder);
            return Ok(response.Code);
        }

        // POST: api/PurchaseOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add", Name = "AddPurchaseOrder")]
        public ActionResult<PurchaseOrder> AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Response response = _purchaseOrderManagement.AddPurchaseOrder(purchaseOrder);

            return CreatedAtRoute("AddPurchaseOrder", new { id = purchaseOrder.Id }, purchaseOrder);
        }

        // DELETE: api/PurchaseOrders/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeletePurchaseOrder(int id)
        {
            Response reponse = _purchaseOrderManagement.DeletePurchaseOrder(id);
            return Ok(reponse.Code);
        }
    }
}
