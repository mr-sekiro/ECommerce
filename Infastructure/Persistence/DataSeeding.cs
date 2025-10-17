using DomainLayer.Contracts;
using DomainLayer.Models;
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
        public void DataSeed()
        {
            try
            {
                //Ensure All Migrations are applied
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }

                if (!dbContext.ProductBrands.Any())
                {
                    var ProductBrandData = File.ReadAllText("..\\Infastructure\\Persistence\\Data\\DataSeed\\brands.json");
                    var ProductBrands = JsonSerializer.Deserialize<List<ProductBrand>>(ProductBrandData);

                    if (ProductBrands is not null && ProductBrands.Any())
                    {
                        dbContext.AddRange(ProductBrands);
                    }
                }

                if (!dbContext.ProductTypes.Any())
                {
                    var ProductTypeData = File.ReadAllText("..\\Infastructure\\Persistence\\Data\\DataSeed\\types.json");
                    var ProductTypes = JsonSerializer.Deserialize<List<ProductType>>(ProductTypeData);

                    if (ProductTypes is not null && ProductTypes.Any())
                    {
                        dbContext.AddRange(ProductTypes);
                    }
                }

                if (!dbContext.Products.Any())
                {
                    var ProductData = File.ReadAllText("..\\Infastructure\\Persistence\\Data\\DataSeed\\products.json");
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);

                    if (Products is not null && Products.Any())
                    {
                        dbContext.AddRange(Products);
                    }
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }

        }
    }
}
