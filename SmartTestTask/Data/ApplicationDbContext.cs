using Microsoft.EntityFrameworkCore;
using SmartTestTask.Models;

namespace SmartTestTask.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ProductionFacility> ProductionFacilities { get; set; }
        public DbSet<ProcessEquipment> ProcessEquipments { get; set; }
        public DbSet<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; }
    }

}
