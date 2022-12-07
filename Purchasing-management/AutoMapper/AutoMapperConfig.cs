using Purchasing_management.Common;
using Purchasing_management.Business;
using Purchasing_management.Data.Entity;
using AutoMapper;

namespace Purchasing_management.Api
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DatabaseTableToViewModelMapping());
            });
        }
    }

    public class DatabaseTableToViewModelMapping : Profile
    {
        public static string StaticHost = Utils.GetConfig("StaticFiles:Host");

        public DatabaseTableToViewModelMapping()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentCreateModel, Department>();
            CreateMap<DepartmentUpdateModel, Department>().ReverseMap();

            CreateMap<PurchaseOrder, PurchaseOrderDto>();
            CreateMap<PurchaseOrder, PurchaseOrderDto>().ForMember(dest => dest.DepartmentName, x => x.MapFrom(src => src.Department.Name.ToString()));
            CreateMap<PurchaseOrderCreateModel, PurchaseOrder>();
            CreateMap<PurchaseOrderUpdateModel, PurchaseOrder>().ReverseMap();

            CreateMap<Supply, SupplyDto>();
        }
    }
}
