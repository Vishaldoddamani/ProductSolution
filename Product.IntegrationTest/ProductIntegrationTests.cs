using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using prod = Product.Domain.Entities;

namespace Product.IntegrationTests
{
    public class ProductControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Act
            var response = await _client.GetAsync("/api/Product");

            // Assert
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadFromJsonAsync<List<prod.Product>>();
            Assert.NotNull(products);
            Assert.True(products.Count > 0);
        }

        [Fact]
        public async Task GetProductById_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var productId = 1; // Ensure this ID exists in your test database

            // Act
            var response = await _client.GetAsync($"/api/Product/{productId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var product = await response.Content.ReadFromJsonAsync<prod.Product>();
            Assert.NotNull(product);
            Assert.Equal(productId, product.Id);
        }

        [Fact]
        public async Task AddProduct_ReturnsCreatedResult()
        {
            // Arrange
            var newProduct = new prod.Product { Name = "New Product" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Product", newProduct);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
            var createdProduct = await response.Content.ReadFromJsonAsync<prod.Product>();
            Assert.NotNull(createdProduct);
            Assert.Equal(newProduct.Name, createdProduct.Name);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContent()
        {
            // Arrange
            var productId = 1; // Ensure this ID exists in your test database
            var updatedProduct = new prod.Product { Id = productId, Name = "Updated Product" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Product/{productId}", updatedProduct);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            // Arrange
            var productId = 1; // Ensure this ID exists in your test database

            // Act
            var response = await _client.DeleteAsync($"/api/Product/{productId}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}