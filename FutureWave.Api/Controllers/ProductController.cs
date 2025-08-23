using AutoMapper;
using FutureWave.Api.Entities;
using FutureWave.Api.Repositories;
using FutureWave.Api.Repositories.Contracts;
using FutureWave.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace FutureWave.Api.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            this.productRepository = productRepository;
            _logger = logger;


        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await productRepository.GetProducts();

                if (products == null || !products.Any()) // Check for empty list
                {
                    return NoContent(); // 204 No Content
                }

                return Ok(products); // 200 OK with data
            }
            catch (Exception ex)
            {
                // Log the error (Assuming you have logging configured)
                _logger.LogError(ex, "An error occurred while fetching products.");

                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }


        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProductAsync([FromBody] ProductDto productDto)
        {
            try
            {
           
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    CategoryId = productDto.CategoryId,
                    CategoryName = productDto.CategoryName,
                    Image = productDto.Image,
                    Qty = productDto.Qty,
                    Price = productDto.Price
                };

                // Add the product to the repository
                await productRepository.AddProduct(product);

          
                return NoContent();
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound(); // Return 404 if the product is not found
            }

            return Ok(product); // Return 200 with the product data
        }



        [HttpGet("category/{Id}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetCategoryById([FromBody] ProductDto productDto)
        {

            var product = await productRepository.GetProductCartegoryById(productDto.Id);

            return Ok(product);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        { 
            var results = await productRepository.DeleteProduct(id);
            return Ok(results);

        }


        [HttpPut("Edit")]
        public async Task<IActionResult> EditProduct([FromBody] ProductDto productDto)
        {
            var results = await productRepository.EditProduct(productDto);
            return Ok(results);

        }

    }
}
