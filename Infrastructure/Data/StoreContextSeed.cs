using Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    // seeds the database with information taken from cvs files that were turned in to json files
    public class StoreContextSeed
    {
        // allows you to use the method without creating an instance of the class 
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                // checks if the ProductBrands table has any data 
                if (!context.ProductBrands.Any())
                {

                    //otherwise it will read from the json file 
                    var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                    // convert the json into a list of productbrand objects
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    // adds each one into the table in the databse
                    foreach (var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }


                    // saves all changes to database
                    await context.SaveChangesAsync();
                }


                
                if (!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach (var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    foreach (var item in products)
                    {
                        context.Products.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {

                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);

            }
        }
    }
}
