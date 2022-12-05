using Microsoft.EntityFrameworkCore;
using Purchasing_management.Common;
using Purchasing_management.Data.Entity;
using System;

namespace Purchasing_management.Data
{
    public class PurchasingDBContext: DbContext
    {
        public PurchasingDBContext(DbContextOptions<PurchasingDBContext> options) : base(options)
        {
        }

        private readonly string connectionString;

        public PurchasingDBContext()
        {
            connectionString = Utils.GetConfig("ConnectionStrings:PurchasingDBContext");
        }

        public virtual DbSet<Purchasing_management.Data.Entity.Department> Departments { get; set; }
        public virtual DbSet<Purchasing_management.Data.Entity.PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<Purchasing_management.Data.Entity.Supply> Supplies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(new Department()
            {
                Id = Guid.Parse("6b1eea43-5597-45a6-bdea-e68c60564247"),
                Name = "Ban Điều Hành",
                Manager = "Nguyễn Tiến Sơn",
                CreatedDate = new DateTime(2021, 2, 1),
                OrdinalNumber = 1
            },
            new Department
            {
                Id = Guid.Parse("a052a63d-fa53-44d5-a197-83089818a676"),
                Name = "Ban Đào Tạo",
                Manager = "Ngô Xuân Hinh",
                CreatedDate = new DateTime(2021, 2, 1),
                OrdinalNumber = 2
            },
            new Department
            {
                Id = Guid.Parse("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"),
                Name = "Ban Nhân Sự",
                Manager = "Lê Tuấn Việt",
                CreatedDate = new DateTime(2021, 2, 1),
                OrdinalNumber = 3
            },
            new Department
            {
                Id = Guid.Parse("8e2f0a16-4c09-44c7-ba56-8dc62dfd792d"),
                Name = "Ban Truyền Thông",
                Manager = "Đỗ Tài Linh",
                CreatedDate = new DateTime(2021, 2, 1),
                OrdinalNumber = 4
            },
            new Department
            {
                Id = Guid.Parse("cab51058-0996-4221-ba63-b841004e89dd"),
                Name = "Ban Giám Sát",
                Manager = "Hoàng Minh Chí",
                CreatedDate = new DateTime(2021, 2, 1),
                OrdinalNumber = 5
            });

            modelBuilder.Entity<PurchaseOrder>().HasData(new PurchaseOrder
            {
                Id = Guid.Parse("6b1eea43-5597-45a6-bdea-e68c60564247"),
                DepartmentId = Guid.Parse("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"),
                RegistantName = "Lê Tuấn Việt"
            },
            new PurchaseOrder
            {
                Id = Guid.Parse("a052a63d-fa53-44d5-a197-83089818a676"),
                DepartmentId = Guid.Parse("8e2f0a16-4c09-44c7-ba56-8dc62dfd792d"),
                RegistantName = "Đỗ Tài Linh"
            },
            new PurchaseOrder
            {
                Id = Guid.Parse("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"),
                DepartmentId = Guid.Parse("a052a63d-fa53-44d5-a197-83089818a676"),
                RegistantName = "Bùi Gia Huy"
            });

            modelBuilder.Entity<Supply>().HasData(new Supply
            {
                Id = Guid.Parse("6b1eea43-5597-45a6-bdea-e68c60564247"),
                OrderId = Guid.Parse("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"),
                OrdinalNumber = 1,
                Name = "Máy tính",
                Amount = "3"
            },
            new Supply
            {
                Id = Guid.Parse("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"),
                OrderId = Guid.Parse("cb554ed6-8fa7-4b8d-8d90-55cc6a3e0074"),
                OrdinalNumber = 2,
                Name = "Chậu hoa",
                Amount = "2"
            },
            new Supply
            {
                Id = Guid.Parse("8e2f0a16-4c09-44c7-ba56-8dc62dfd792d"),
                OrderId = Guid.Parse("a052a63d-fa53-44d5-a197-83089818a676"),
                OrdinalNumber = 3,
                Name = "Bàn",
                Amount = "2"
            },

            new Supply
            {
                Id = Guid.Parse("a052a63d-fa53-44d5-a197-83089818a676"),
                OrderId = Guid.Parse("a052a63d-fa53-44d5-a197-83089818a676"),
                OrdinalNumber = 4,
                Name = "Ghế",
                Amount = "6"
            },
            new Supply
            {
                Id = Guid.Parse("cab51058-0996-4221-ba63-b841004e89dd"),
                OrderId = Guid.Parse("6b1eea43-5597-45a6-bdea-e68c60564247"),
                OrdinalNumber = 3,
                Name = "Bàn",
                Amount = "2"
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
