using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using System.Collections.Generic;
using System.Linq;

using Purchasing_management.Common;

namespace Purchasing_management.Business.Services
{
    public class Purchasing_Management
    {
        private readonly PurchasingDBContext _dbcontext;

        public Purchasing_Management(PurchasingDBContext DBContext)
        {
            _dbcontext = DBContext;
        }

        public void AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _dbcontext.PurchaseOrders.Add(purchaseOrder);
            _dbcontext.SaveChanges();
        }

        public void EditPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _dbcontext.Entry(purchaseOrder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbcontext.SaveChangesAsync();
        }

        public PurchaseOrder DeletePurchaseOrder(int Id)
        {
            PurchaseOrder purchaseOrder = _dbcontext.PurchaseOrders.Find(Id);
            if (purchaseOrder == null)
            {
                return null;
            }

            _dbcontext.PurchaseOrders.Remove(purchaseOrder);
            _dbcontext.SaveChangesAsync();
            return purchaseOrder;
        }

        public IList<PurchaseOrder> GetPurchaseOrders(int page, int size)
        {
            var validFilter = new Pagination<PurchaseOrder>(page, size);

            IList<PurchaseOrder> purchaseOrders = _dbcontext.PurchaseOrders
                                            .Skip((validFilter.Page - 1) * validFilter.Size)
                                            .Take(validFilter.Size)
                                            .ToList<PurchaseOrder>();
            return purchaseOrders;
        }

        public PurchaseOrder GetPurchaseOrder(int id)
        {
            PurchaseOrder purchaseOrder = _dbcontext.PurchaseOrders.Find(id);
            return purchaseOrder;
        }
    }
}
