using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IServiceManager _service;

        public ProductsController(IServiceManager service)
        {
            _service = service;
        }

        //GET: api/products
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParams Params)
        {
            var result = await _service.ProductService.GetAllProductsAsync(Params);
            return Ok(result);
        }
        //GET: api/products/paginated
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedProducts([FromQuery] ProductQueryParams Params)
        {
            var result = await _service.ProductService.GetProductsWithPaginationAsync(Params);
            return Ok(result);
        }
        //GET: api/products/count
        [HttpGet("count")]
        public async Task<IActionResult> GetProductsCount([FromQuery] ProductQueryParams Params)
        {
            var count = await _service.ProductService.GetProductsCountAsync(Params);
            return Ok(new { TotalCount = count });
        }

        //GET: api/products/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _service.ProductService.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { Message = $"Product with ID {id} not found." });

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.ProductService.AddAsync(productDto);

            //After saving, fetch the new product (if you want to return the created resource)
            var allProducts = await _service.ProductService.GetAllProductsAsync(new ProductQueryParams());
            var createdProduct = allProducts.LastOrDefault(); // or use a return value from service

            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct?.Id }, createdProduct);
        }

        //PUT: api/products/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
        {
            if (id != productDto.Id)
                return BadRequest("Product ID mismatch.");

            var existing = await _service.ProductService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Product with ID {id} not found." });

            await _service.ProductService.UpdateAsync(productDto);
            return NoContent();
        }

        //DELETE: api/products/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existing = await _service.ProductService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Product with ID {id} not found." });

            await _service.ProductService.DeleteAsync(id);
            return NoContent();
        }

        //GET: api/products/brands
        [HttpGet("brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _service.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }

        //GET: api/products/types
        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var types = await _service.ProductService.GetAllTypesAsync();
            return Ok(types);
        }
    }
}
