using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper.Configuration;
using AutoMapper;

namespace API.Controllers
{
    
	[ApiController] //states that the controller is an API Controller
	[Route("api/[controller]")] //states the beginning of the url 
	public class ProductsController : ControllerBase //inherits from ControllerBase from the AspNetCore.MVC 
	{
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        // constructor that injects the interface for Product Repository into the controller


        public ProductsController(IGenericRepository<Product> productsRepo, 
            IGenericRepository<ProductBrand> productBrandRepo, 
            IGenericRepository<ProductType> productTypeRepo,
            IMapper mapper)
        {
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }


        // method to get all products using the method in the repository 
        // url is the base url : "/api/products"
        [HttpGet]
		public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesandBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            return Ok(
                _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }


        // method to get a specific product with parameter Id 
        // url is "/api/products/{id}" => id is an integer aka "api/products/2" 
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            // uses the method in the repository class 
            var spec = new ProductsWithTypesandBrandsSpecification(id);
			var product = await _productsRepo.GetEntityWithSpec(spec);

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        // method to get a list of the brands using method in repository class
        // url is "/api/products/brands"
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            // use Ok as you can't convert a readonly list to a 
            return Ok(await _productTypeRepo.ListAllAsync());
        }

        //method to get a list of the types using method in repository class 
        // url is "/api/products/types"
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

    }


}
