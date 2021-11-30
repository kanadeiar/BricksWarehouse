namespace BricksWarehouse.Dal.Data;

public class WarehouseContext : DbContext
{
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Place> Places { get; set; }

    public WarehouseContext(DbContextOptions<WarehouseContext> options) : base( options )
    { 
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

