using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{

    [ApiController] //states that the controller is an API Controller
	[Route("api/[controller]")] //states the beginning of the url 
	public class ProductsController : BaseApiController //inherits from ControllerBase from the AspNetCore.MVC 
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
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
          [FromQuery]ProductSpecParams productParams)
        {

            var spec = new ProductsWithTypesandBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);
            var products = await _productsRepo.ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }


        // method to get a specific product with parameter Id 
        // url is "/api/products/{id}" => id is an integer aka "api/products/2" 
		[HttpGet("{id}")]
        // tells swagger and us specifically what could be returned 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            // uses the method in the repository class 
            var spec = new ProductsWithTypesandBrandsSpecification(id);
			var product = await _productsRepo.GetEntityWithSpec(spec);
            if (product == null) return NotFound(new ApiResponse(404));

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
