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
    public class PurchasingHandler
    {
        private readonly PurchasingDBContext _dbcontext;
        private readonly ILogger<PurchasingHandler> _logger;
        private readonly DbHandler<PurchaseOrder, PurchaseOrder, PaginationRequest> _dbHandler = DbHandler<PurchaseOrder, PurchaseOrder, PaginationRequest>.Instance;

        public PurchasingHandler(PurchasingDBContext DBContext, ILogger<PurchasingHandler> logger)
        {
            _dbcontext = DBContext;
            _logger = logger;
        }

        public async Task<Response> AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _logger.LogInformation("Start adding purchaseOrder.");
            try
            {
                Guid id = purchaseOrder.Id;
                _dbcontext.PurchaseOrders.Add(purchaseOrder);
                _dbcontext.SaveChanges();
                _logger.LogInformation("Add purchaseOrder - id: {@id}", id);
                _logger.LogInformation("End adding purchaseOrder: Success");
                return new Response(System.Net.HttpStatusCode.OK, "Add PurchaseOrder Success!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End adding purchaseOrder: Fail");
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        public async Task<Response> EditPurchaseOrder(Guid id, PurchaseOrder purchaseOrder)
        {
            _logger.LogInformation("Start editing purchaseOrder.");
            if (id != purchaseOrder.Id)
            {
                _logger.LogError("Id invalid");
                return new ResponseError(System.Net.HttpStatusCode.NotFound, "Id invalid!");
            }
            try
            {
                _dbcontext.Entry(purchaseOrder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbcontext.SaveChangesAsync();
                _logger.LogInformation("Edit purchaseOrder - id: {@id}", id);
                _logger.LogInformation("End editing purchaseOrder: Success");
                return new ResponseUpdate(System.Net.HttpStatusCode.OK, "Edit PurchaseOrder: Success", id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End editing purchaseOrder: Fail");
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        public async Task<Response> DeletePurchaseOrder(Guid id)
        {
            try
            {
                _logger.LogInformation("Start deleting purchaseOrder");
                PurchaseOrder purchaseOrder = _dbcontext.PurchaseOrders.Find(id);

                if (purchaseOrder == null)
                {
                    _logger.LogError("Not found");
                    _logger.LogInformation("End deleting purchaseOrder: Fail");
                    return new ResponseError(System.Net.HttpStatusCode.NotFound, "Id Not found");
                }

                _dbcontext.PurchaseOrders.Remove(purchaseOrder);
                _dbcontext.SaveChangesAsync();
                _logger.LogInformation("Deleted purchaseOrder with Id: {@id}", id);
                _logger.LogInformation("End deleting purchaseOrder: Success");

                return new ResponseDelete(System.Net.HttpStatusCode.OK, "Delete Success", id, "");
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End deleting purchaseOrder: Fail");
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        public async Task<Response> GetPurchaseOrders(PaginationRequest paginationRequest)
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
        public async Task<Response> GetPurchaseOrder(Guid id)
        {
            _logger.LogInformation("Start getting purchaseOrder by Id");

            PurchaseOrder purchaseOrder;
            try
            {
                _logger.LogInformation("Getting purchaseOrder with Id: {@id}", id);
                purchaseOrder = _dbcontext.PurchaseOrders.Include(CustomExtensions.GetIncludePaths(_dbcontext, typeof(PurchaseOrder))).SingleOrDefault(i => i.Id == id);
                if (purchaseOrder == null)
                {
                    _logger.LogError("Not found purchaseOrder with Id: {@id}", id);
                    _logger.LogInformation("End getting purchaseOrder by Id: Fail");
                    return new ResponseError(System.Net.HttpStatusCode.BadRequest, "PurchaseOrder not found! ");
                }
                _logger.LogInformation("End getting purchaseOrder by Id: Success");
                return new Response<PurchaseOrder>(System.Net.HttpStatusCode.OK, purchaseOrder);
            }
            catch (Exception ex)
            {
                purchaseOrder = null;
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End getting purchaseOrder by Id: Fail");
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        private Expression<Func<PurchaseOrder, bool>> BuildQuery(PaginationRequest query)
        {
            var predicate = PredicateBuilder.New<PurchaseOrder>(true);

            if (!string.IsNullOrEmpty(query.FullTextSearch))
            {
                predicate.And(s => s.RegistantName.Contains(query.FullTextSearch));
            }    

            return predicate;
        }
    }
}
