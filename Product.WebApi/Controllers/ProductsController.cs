using Microsoft.AspNetCore.Mvc;
using Product.Application.Services;
using Prod = Product.Domain.Entities;

namespace Product.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        #region Get all products.
        /// <summary>
        /// Get All products.
        /// </summary>
        /// <returns>Json Product.</returns>
        [HttpGet]
        public async Task<IReadOnlyCollection<Prod.Product>> GetAllProducts()
        {
            return await _productService.GetAllProductsAsync();
        }

        #endregion

        #region Get Product By Id.
        /// <summary>
        /// Get Product By Id.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Product</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            return Ok(_productService.GetProductByIdAsync(id));
        }



        #endregion

        #region Add a product into a database.

        /// <summary>
        /// Adds a product into a database.
        /// </summary>
        /// <param name="product">product</param>
        /// <returns>201 Successfully created. when adding a resource</returns>
        [HttpPost]
        public async Task<IActionResult> AddProduct(Prod.Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        #endregion

        #region Updates a product by Product by Id.
        /// <summary>
        /// Updates a product by product by id.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="product">product</param>
        /// <returns>NoContent</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Prod.Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        #endregion

        #region Delete a product.

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        #endregion
    }
}
