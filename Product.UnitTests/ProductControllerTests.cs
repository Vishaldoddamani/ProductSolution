using Microsoft.AspNetCore.Mvc;
using Moq;
using Product.Application.Interfaces;
using Product.Application.Services;
using Product.WebApi.Controllers;
using Xunit;
using prod = Product.Domain.Entities;


namespace Product.UnitTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ProductService> _mockProductService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockProductService = new Mock<ProductService>(_mockProductRepository.Object);
            _controller = new ProductController(_mockProductService.Object);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOkResult_WithListOfProducts1()
        {
            // Arrange
            var products = new List<prod.Product>
                    {
                        new prod.Product { Id = 1, Name = "Product 1" },
                        new prod.Product { Id = 2, Name = "Product 2" }
                    };
            _mockProductRepository.Setup(repo => repo.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAllProducts();
            var okResult = Assert.IsType<List<prod.Product>>(result);
            Assert.Equal(2, okResult?.Count);
            Assert.Equal(1, result.ToList().FirstOrDefault()?.Id);

        }


        [Fact]
        public async Task GetProductById_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var product = new prod.Product { Id = 1, Name = "Product1" };
            _mockProductRepository.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult);
        }


        [Fact]
        public async Task AddProduct_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var product = new prod.Product { Id = 1, Name = "Product1" };
            _mockProductRepository.Setup(service => service.AddProductAsync(product)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddProduct(product);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnProduct = Assert.IsType<prod.Product>(createdAtActionResult.Value);
            Assert.Equal(1, returnProduct.Id);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContentResult()
        {
            // Arrange
            var product = new prod.Product { Id = 1, Name = "UpdatedProduct" };
            _mockProductRepository.Setup(service => service.UpdateProductAsync(product)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProduct(1, product);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContentResult()
        {
            // Arrange
            _mockProductRepository.Setup(service => service.DeleteProductAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }


}
