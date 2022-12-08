using Purchasing_management.Data;
using Purchasing_management.Data.Entity;
using Purchasing_management.Common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using LinqKit;
using System.Linq.Expressions;
using System.Linq;
using AutoMapper;

namespace Purchasing_management.Business
{
    public class PurchasingHandler
    {
        private readonly PurchasingDBContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly ILogger<PurchasingHandler> _logger;
        private readonly DbHandler<PurchaseOrder, PurchaseOrderDto, PaginationRequest> _dbHandler = DbHandler<PurchaseOrder, PurchaseOrderDto, PaginationRequest>.Instance;

        public PurchasingHandler(PurchasingDBContext DBContext, IMapper mapper, ILogger<PurchasingHandler> logger)
        {
            _dbcontext = DBContext ?? throw new ArgumentNullException(nameof(DBContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Response> GetPurchaseOrder(Guid id)
        {
            _logger.LogInformation("Start getting purchaseOrder by Id");

            PurchaseOrderDto purchaseOrder;
            try
            {
                _logger.LogInformation("Getting purchaseOrder with Id: {@id}", id);
                purchaseOrder = _mapper.Map<PurchaseOrderDto>(_dbcontext.PurchaseOrders.Include(CustomExtensions.GetIncludePaths(_dbcontext, typeof(PurchaseOrder))).SingleOrDefault(i => i.Id == id));
                if (purchaseOrder == null)
                {
                    _logger.LogError("Not found purchaseOrder with Id: {@id}", id);
                    _logger.LogInformation("End getting purchaseOrder by Id: Fail");
                    return new ResponseError(System.Net.HttpStatusCode.BadRequest, "PurchaseOrder not found! ");
                }
                _logger.LogInformation("End getting purchaseOrder by Id: Success");
                return new Response<PurchaseOrderDto>(System.Net.HttpStatusCode.OK, purchaseOrder);
            }
            catch (Exception ex)
            {
                purchaseOrder = null;
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End getting purchaseOrder by Id: Fail");
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }

        public async Task<Response> GetPurchaseOrders(PaginationRequest paginationRequest)
        {
            try
            {
                _logger.LogInformation("Start getting purchaseOrder");
                var predicate = BuildQuery(paginationRequest);
                var result = await _dbHandler.GetPageAsync(predicate, paginationRequest, _mapper);
                _logger.LogInformation("End getting purchaseOrder.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong! ", ex);
                _logger.LogInformation("End getting purchaseOrder: Fail");
                return new ResponseError(System.Net.HttpStatusCode.BadRequest, "Something went wrong! " + ex);
            }
        }


        public async Task<Response> AddPurchaseOrder(PurchaseOrderCreateModel purchaseOrder)
        {
            _logger.LogInformation("Start adding purchaseOrder.");
            if (purchaseOrder == null)
                return new ResponseError(System.Net.HttpStatusCode.NotFound, "PurchaseOrder Not found");
            try
            {
                var purchaseOrderEntity = _mapper.Map<PurchaseOrder>(purchaseOrder);
                purchaseOrderEntity.CreateByUserId = _dbcontext.UserTokens.FirstOrDefault().UserId;
                _dbcontext.SaveChanges();
                _logger.LogInformation("Add purchaseOrder - id: {@id}", purchaseOrderEntity.Id);
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

        public async Task<Response> EditPurchaseOrder(Guid purchaseOrderId, Guid departmentId, PurchaseOrderUpdateModel purchaseOrder)
        {
            _logger.LogInformation("Start editing purchaseOrder.");
            if (!this.PurchaseOrderExists(purchaseOrderId))
            {
                _logger.LogError("PurchaseOrderId not found");
                return new ResponseError(System.Net.HttpStatusCode.NotFound, "PurchaseOrderId not found");
            }
            if (!this.DepartmentExists(departmentId))
            {
                _logger.LogError("DepartmentId not found");
                return new ResponseError(System.Net.HttpStatusCode.NotFound, "DepartmentId not found");
            }
            try
            {
                var purchaseOrderFromRepo = _dbcontext.PurchaseOrders.Where(d => d.Id == purchaseOrderId && d.DepartmentId == departmentId).FirstOrDefault();

                if (purchaseOrderFromRepo == null)
                {
                    var purchaseOrderToAdd = _mapper.Map<PurchaseOrder>(purchaseOrder);
                    purchaseOrderToAdd.Id = purchaseOrderId;
                    _dbcontext.Add(purchaseOrderToAdd);
                    _dbcontext.SaveChanges();

                    var DepartmentToReturn = _mapper.Map<DepartmentDto>(purchaseOrderToAdd);
                    return new ResponseUpdate(System.Net.HttpStatusCode.OK, "Edit Department: Success", purchaseOrderId);
                }

                _mapper.Map(purchaseOrder, purchaseOrderFromRepo);
                _dbcontext.PurchaseOrders.Update(purchaseOrderFromRepo);
                _dbcontext.SaveChanges();

                _logger.LogInformation("Edit purchaseOrder - id: {@id}", purchaseOrderId);
                _logger.LogInformation("End editing purchaseOrder: Success");
                return new ResponseUpdate(System.Net.HttpStatusCode.OK, "Edit PurchaseOrder: Success", purchaseOrderId);
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
                _dbcontext.SaveChanges();
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

        bool PurchaseOrderExists(Guid purchaseOrderId)
        {
            if (purchaseOrderId == Guid.Empty)
                throw new ArgumentNullException(nameof(purchaseOrderId));

            return _dbcontext.PurchaseOrders.Any(a => a.Id == purchaseOrderId);
        }

        bool DepartmentExists(Guid departmentId)
        {
            if (departmentId == Guid.Empty)
                throw new ArgumentNullException(nameof(departmentId));

            return _dbcontext.Departments.Any(a => a.Id == departmentId);
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
