using System;
using Core.Interfaces;
using Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    //Implementation of the interface 
    // Holds the data access logic - abstracts it from the controller 
    // Middleman? - interacts with the context class instead 

    public class ProductRepository : IProductRepository
    {

        // idependency injection of StoreContext that interacts/queries the database
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;
        }


        // returns the list of brands in a readonlylist so it cannot be edited
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToArrayAsync();
        }


        // returns a product by its id 
        public async Task<Product> GetProductByIdAsync(int id)
        {

            return await _context.Products
                // joins the information from the other tables 
                // otherwise will be null 
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                // returns the first matching entity 
                .FirstOrDefaultAsync(p => p.Id == id);
            // could use SingleOrDefault - if it find more than one entity, it will return an exception
        }


        // returns a list of products 
        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products
                 .Include(p => p.ProductType)
                 .Include(p => p.ProductBrand)
                 .ToListAsync();
           
        }

        // returns a list of product types 
        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }

}
