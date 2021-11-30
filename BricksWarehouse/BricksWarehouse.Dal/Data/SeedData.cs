using BricksWarehouse.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BricksWarehouse.Dal.Data
{
    public static class SeedData
    {
        public async static void SeedWarehouseContextTestData(IServiceProvider serviceProvider)
        {
            using (var context = new WarehouseContext(serviceProvider.GetRequiredService<DbContextOptions<WarehouseContext>>()))
            {
                var logger = serviceProvider.GetRequiredService<ILogger<WarehouseContext>>();
                if (context == null || context.ProductTypes == null)
                {
                    logger.LogError("Null WarehouseContext");
                    throw new ArgumentNullException("Null WarehouseContext");
                }
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation($"Applying migrations: {string.Join(",", pendingMigrations)}");
                    await context.Database.MigrateAsync();
                }
                if (context.ProductTypes.Any() || context.Places.Any())
                {
                    logger.LogInformation("Database contains data - database init with test data is not required");
                    return;
                }

                logger.LogInformation("Begin writing data to database ...");

                ProductType pt1 = new ProductType { Name = "Кирпич полуторный 250x120x65", FormatNumber = 400, Order = 1, Units = 360, Volume = 1.2, Weight = 100.0 };
                ProductType pt2 = new ProductType { Name = "Кирпич пустотелый 250x120x65", FormatNumber = 1, Order = 10, Units = 240, Volume = 1.1, Weight = 140.0 };
                ProductType pt3 = new ProductType { Name = "Кирпич полнотелый 250x120x88", FormatNumber = 2, Order = 10, Units = 460, Volume = 1.2, Weight = 100.0 };
                ProductType pt4 = new ProductType { Name = "Кирпич двойной 250x120x138", FormatNumber = 3, Order = 20, Units = 380, Volume = 1.3, Weight = 130.0 };
                ProductType pt5 = new ProductType { Name = "Кирпич евро пустотелый 250x120x120", FormatNumber = 800, Order = 30, Units = 280, Volume = 1.4, Weight = 120.0 };
                ProductType pt6 = new ProductType { Name = "Кирпич евро полнотелый 250x120x65", FormatNumber = 880, Order = 40, Units = 340, Volume = 1.2, Weight = 110.0 };
                context.ProductTypes.AddRange(pt1, pt2, pt3, pt4, pt5, pt6);
                await context.SaveChangesAsync();

                Place p1 = new Place { Name = "Главный стеллаж", Order = 1, Number = 1, ProductType = pt1, Count = 1, Size = 30, LastDateTime = DateTime.Today.AddHours(4), PlaceStatus = PlaceStatus.Default };
                Place p2 = new Place { Name = "Заполняемый стеллаж", Order = 10, Number = 2, ProductType = pt1, Count = 5, Size = 60, LastDateTime = DateTime.Today.AddHours(2), PlaceStatus = PlaceStatus.Collect };
                Place p3 = new Place { Name = "Заполняемый стеллаж", Order = 20, Number = 3, ProductType = pt2, Count = 20, Size = 60, LastDateTime = DateTime.Today.AddHours(8), PlaceStatus = PlaceStatus.Collect };
                Place p4 = new Place { Name = "Запасной стеллаж 1", Order = 40, Number = 4, ProductType = pt4, Count = 10, Size = 20, LastDateTime = DateTime.Today.AddHours(2), PlaceStatus = PlaceStatus.Wait };
                Place p5 = new Place { Name = "Запасной стеллаж 2", Order = 40, Number = 5, ProductType = pt5, Count = 20, Size = 20, LastDateTime = DateTime.Today.AddHours(3), PlaceStatus = PlaceStatus.Wait };
                Place p6 = new Place { Name = "Запасной стеллаж 3", Order = 40, Number = 6, ProductType = pt1, Count = 30, Size = 20, LastDateTime = DateTime.Today.AddHours(-2), PlaceStatus = PlaceStatus.Wait };
                Place p7 = new Place { Name = "Крайний стеллаж 1", Order = 70, Number = 7, ProductType = pt1, Count = 10, Size = 40, LastDateTime = DateTime.Today.AddHours(-8), PlaceStatus = PlaceStatus.Delivery };
                Place p8 = new Place { Name = "Крайний стеллаж 2", Order = 70, Number = 8, ProductType = pt6, Count = 40, Size = 40, LastDateTime = DateTime.Today.AddHours(-8), PlaceStatus = PlaceStatus.Delivery };
                context.Places.AddRange(p1, p2, p3, p4, p5, p6, p7, p8);
                await context.SaveChangesAsync();

                logger.LogInformation("Complete writing data to database ...");
            }
        }
    }
}
