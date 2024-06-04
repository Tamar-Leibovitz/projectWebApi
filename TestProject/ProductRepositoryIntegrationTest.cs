using DTOs;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class ProductRepositoryIntegrationTest : IClassFixture<DatabaseFixture>
    {
        private readonly Shop214928673Context _dbContext;
        private readonly ProductRepository _productRepository;

        public ProductRepositoryIntegrationTest(DatabaseFixture databaseFixture)
        {
            _dbContext = databaseFixture.Context;
            _productRepository = new ProductRepository(_dbContext);
        }


        [Fact]
        public async Task GetProductById_ShouldReturnCorrectProduct()
        {
            // Arrange
            Category category = new Category { CategoryName = "Test Category" };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            int categoryId = category.CategoryId;
            Product product = new Product { ProductName = "Test Product", Price = 9, ImageUrl = "./testUrl", Description = "Test Description", CategoryId = categoryId };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            int productId = product.ProductId;
            // Act
            var retrievedProduct = await _productRepository.GetProductById(productId);

            // Assert
            Assert.NotNull(retrievedProduct);
            Assert.Equal(productId, retrievedProduct.ProductId);
            Assert.Equal("Test Product", retrievedProduct.ProductName);
            Assert.Equal(9, retrievedProduct.Price);
        }
    }

}

