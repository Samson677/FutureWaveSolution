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
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;


        }
        [HttpGet]
        public async Task <ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {

            try
            {
                var products = await productRepository.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal Server Error: {ex.Message}");
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















        [HttpGet("Id")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductById([FromBody] ProductDto productDto)
        {
            var product = await productRepository.GetProductById(productDto.Id);
            return Ok(product);
        }


        [HttpGet("category/{Id}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetCategoryById([FromBody] ProductDto productDto)
        {

            var product = await productRepository.GetProductCartegoryById(productDto.Id);

            return Ok(product);
        }

        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteProduct([FromBody] ProductDto productDto)
        { 
            var results = await productRepository.DeleteProduct(productDto.Id);
            return Ok(results);

        }


        [HttpPut("Edit")]
        public async Task<IActionResult> EditProduct([FromBody] ProductDto productDto)
        {
            var results = await productRepository.EditProduct(productDto.Id);
            return Ok(results);

        }

    }
}
