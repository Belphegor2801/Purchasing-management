using Microsoft.EntityFrameworkCore;

namespace Purchasing_management.Data
{
    public class PurchasingDBContext: DbContext
    {
        public PurchasingDBContext(DbContextOptions<PurchasingDBContext> options) : base(options)
        {
        }

        private readonly string connectionString;

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
    }
}
