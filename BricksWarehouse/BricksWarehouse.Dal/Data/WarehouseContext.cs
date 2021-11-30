using BricksWarehouse.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksWarehouse.Dal.Data
{
    public class WarehouseContext : DbContext
    {
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Place> Places { get; set; }

        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base( options )
        { }
    }
}
