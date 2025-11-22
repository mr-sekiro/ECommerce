using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModels;
using DomainLayer.Models.ProductModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataSeeding(StoreDbContext dbContext) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            try
            {
                //Ensure All Migrations are applied

                //if (dbContext.Database.GetAppliedMigrationsAsync().Result.Any())
                //if ((await dbContext.Database.GetAppliedMigrationsAsync()).Any())
                var PendingMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
                if (PendingMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync();
                }

                if (!dbContext.ProductBrands.Any())
                {
                    //var ProductBrandData = await File.ReadAllTextAsync("..\\Infastructure\\Persistence\\Data\\DataSeed\\brands.json");
                    var ProductBrandData = File.OpenRead("..\\Infastructure\\Persistence\\Data\\DataSeed\\brands.json");
                    var ProductBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductBrandData);

                    if (ProductBrands is not null && ProductBrands.Any())
                    {
                        await dbContext.AddRangeAsync(ProductBrands);
                    }
                }

                if (!dbContext.ProductTypes.Any())
                {
                    var ProductTypeData = File.OpenRead("..\\Infastructure\\Persistence\\Data\\DataSeed\\types.json");
                    var ProductTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductTypeData);

                    if (ProductTypes is not null && ProductTypes.Any())
                    {
                        await dbContext.AddRangeAsync(ProductTypes);
                    }
                }

                if (!dbContext.Products.Any())
                {
                    var ProductData = File.OpenRead("..\\Infastructure\\Persistence\\Data\\DataSeed\\products.json");
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductData);

                    if (Products is not null && Products.Any())
                    {
                        await dbContext.AddRangeAsync(Products);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

        }
    }
}
