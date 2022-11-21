using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using Purchasing_management.Common;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace Purchasing_management.Business.Services
{
    public class Purchasing_Manager
    {
        private readonly PurchasingDBContext _dbcontext;
        private readonly ILogger<Purchasing_Manager> _logger;

        public Purchasing_Manager(PurchasingDBContext DBContext, ILogger<Purchasing_Manager> logger)
        {
            _dbcontext = DBContext;
            _logger = logger;
        }

        public Response AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _logger.LogInformation("Start adding purchaseOrder.");
            try
            {
                int Id = purchaseOrder.Id;
                _dbcontext.PurchaseOrders.Add(purchaseOrder);
                _dbcontext.SaveChanges();
                _logger.LogInformation("Add purchaseOrder - Id: {@Id}", Id);
                _logger.LogInformation("End adding purchaseOrder: Success");
                return new Response(System.Net.HttpStatusCode.OK, "Add PurchaseOrder Success!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End adding purchaseOrder: Fail");
                return new Response(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        public ResponseUpdate EditPurchaseOrder(int id, PurchaseOrder purchaseOrder)
        {
            _logger.LogInformation("Start editing purchaseOrder.");
            if (id != purchaseOrder.Id)
            {
                _logger.LogError("Id not invalid");
                return new ResponseUpdate(System.Net.HttpStatusCode.NotFound, "Id not found!!", ToGuid(id));
            }
            try
            {
                _dbcontext.Entry(purchaseOrder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbcontext.SaveChangesAsync();
                int Id = purchaseOrder.Id;
                _logger.LogInformation("Edit purchaseOrder - Id: {@Id}", Id);
                _logger.LogInformation("End editing purchaseOrder: Success");
                return new ResponseUpdate(System.Net.HttpStatusCode.OK, "Edit PurchaseOrder: Success", ToGuid(id));
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End editing purchaseOrder: Fail");
                return new ResponseUpdate(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex, ToGuid(id));
            }
        }

        public ResponseDelete DeletePurchaseOrder(int id)
        {
            _logger.LogInformation("Start deleting purchaseOrder");
            PurchaseOrder purchaseOrder = _dbcontext.PurchaseOrders.Find(id);

            if (purchaseOrder == null)
            {
                _logger.LogError("Not found");
                _logger.LogInformation("End getting purchaseOrder by Id: Fail");
                return new ResponseDelete(System.Net.HttpStatusCode.NotFound, "Id Not found", ToGuid(id), "");
            }

            _dbcontext.PurchaseOrders.Remove(purchaseOrder);
            _dbcontext.SaveChangesAsync();
            _logger.LogInformation("Deleted purchaseOrder with Id: {@id}", id);
            _logger.LogInformation("End deleting purchaseOrder: Success");

            return new ResponseDelete(System.Net.HttpStatusCode.OK, "Delete Success", ToGuid(id), "");
        }

        public ResponsePagination<PurchaseOrder> GetPurchaseOrders(int page, int size)
        {
            var validFilter = new Pagination<PurchaseOrder>(page, size);
            ResponsePagination<PurchaseOrder> responsePagination = new ResponsePagination<PurchaseOrder>(validFilter);
            try
            {
                _logger.LogInformation("Start getting purchaseOrder");
                IList<PurchaseOrder> purchaseOrders = _dbcontext.PurchaseOrders
                                                .Skip((responsePagination.Data.Page - 1) * responsePagination.Data.Size)
                                                .Take(responsePagination.Data.Size)
                                                .ToList<PurchaseOrder>();
                _logger.LogInformation("End getting purchaseOrder.");
                responsePagination.Code = System.Net.HttpStatusCode.OK;
                responsePagination.Message = "OK";
                responsePagination.Data.Content = purchaseOrders.ToList<PurchaseOrder>();
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

        public Response<PurchaseOrder> GetPurchaseOrder(int id)
        {
            _logger.LogInformation("Start getting purchaseOrder by Id");

            PurchaseOrder purchaseOrder;
            try
            {
                _logger.LogInformation("Getting purchaseOrder with Id: {@id}", id);
                purchaseOrder = _dbcontext.PurchaseOrders.Find(id);
                if (purchaseOrder == null)
                {
                    _logger.LogError("Not found purchaseOrder with Id: {@id}", id);
                    _logger.LogInformation("End getting purchaseOrder by Id: Fail");
                }
                _logger.LogInformation("End getting purchaseOrder by Id: Success");
                return new Response<PurchaseOrder>(System.Net.HttpStatusCode.OK, purchaseOrder);
            }
            catch (Exception ex)
            {
                purchaseOrder = null;
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End getting purchaseOrder by Id: Fail");
                return new Response<PurchaseOrder>(System.Net.HttpStatusCode.BadRequest, purchaseOrder, "Something went wrong!" + ex);
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
