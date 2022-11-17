using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using Purchasing_management.Common;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Purchasing_management.Business.Services
{
    public class Purchasing_Management
    {
        private readonly PurchasingDBContext _dbcontext;
        private readonly ILogger<Purchasing_Management> _logger;

        public Purchasing_Management(PurchasingDBContext DBContext, ILogger<Purchasing_Management> logger)
        {
            _dbcontext = DBContext;
            _logger = logger;
        }

        public void AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _logger.LogInformation("Start adding purchaseOrder.");
            try
            {
                int Id = purchaseOrder.Id;
                _logger.LogInformation("Add purchaseOrder - Id: {@Id}", Id);
                _dbcontext.PurchaseOrders.Add(purchaseOrder);
                _dbcontext.SaveChanges();
                _logger.LogInformation("End adding purchaseOrder: Success");

            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End adding purchaseOrder: Fail");
            }
        }

        public void EditPurchaseOrder(int id, PurchaseOrder purchaseOrder)
        {
            _logger.LogInformation("Start editing purchaseOrder.");
            if (id != purchaseOrder.Id)
            {
                _logger.LogError("Id not invalid");
            }
            try
            {
                _dbcontext.Entry(purchaseOrder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbcontext.SaveChangesAsync();
                int Id = purchaseOrder.Id;
                _logger.LogInformation("Edit purchaseOrder - Id: {@Id}", purchaseOrder.Id);
                _logger.LogInformation("End editing purchaseOrder: Success");
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End editing purchaseOrder: Fail");
            }
        }

        public PurchaseOrder DeletePurchaseOrder(int id)
        {
            _logger.LogInformation("Start deleting purchaseOrder");
            PurchaseOrder purchaseOrder = _dbcontext.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                _logger.LogError("Not found");
                _logger.LogInformation("End getting purchaseOrder by Id: Fail");
                return null;
            }

            _dbcontext.PurchaseOrders.Remove(purchaseOrder);
            _dbcontext.SaveChangesAsync();
            _logger.LogInformation("Deleted purchaseOrder with Id: {@id}", id);
            _logger.LogInformation("End deleting purchaseOrder: Success");
            return purchaseOrder;
        }

        public IList<PurchaseOrder> GetPurchaseOrders(int page, int size)
        {
            _logger.LogInformation("Start getting purchaseOrder");
            var validFilter = new Pagination<PurchaseOrder>(page, size);

            IList<PurchaseOrder> purchaseOrders = _dbcontext.PurchaseOrders
                                            .Skip((validFilter.Page - 1) * validFilter.Size)
                                            .Take(validFilter.Size)
                                            .ToList<PurchaseOrder>();
            _logger.LogInformation("End getting purchaseOrder.");
            return purchaseOrders;
        }

        public PurchaseOrder GetPurchaseOrder(int id)
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
                    return null;
                }
                _logger.LogInformation("End getting purchaseOrder by Id: Success");
            }
            catch (Exception ex)
            {
                purchaseOrder = null;
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End getting purchaseOrder by Id: Fail");

            }
            return purchaseOrder;
        }
    }
}
