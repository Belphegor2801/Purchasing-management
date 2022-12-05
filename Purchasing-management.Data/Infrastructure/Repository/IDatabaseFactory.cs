using Microsoft.EntityFrameworkCore;

namespace Purchasing_management.Data
{
    public interface IDatabaseFactory
    {
        DbContext GetDbContext();
    }
}