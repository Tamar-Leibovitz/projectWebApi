using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class ProductRepositoryUnitTest
{
    [Fact]
    public async Task GetAllProducts_WithVariousFilters_ReturnsFilteredProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { ProductId = 1, ProductName = "Product1", Price = 10, CategoryId = 1 },
            new Product { ProductId = 2, ProductName = "Product2", Price = 20, CategoryId = 2 },
            new Product { ProductId = 3, ProductName = "Product3", Price = 30, CategoryId = 1 },
            new Product { ProductId = 4, ProductName = "Product4", Price = 40, CategoryId = 3 }
        };

        var mockContext = new Mock<Shop214928673Context>();
        mockContext.Setup(x => x.Products).ReturnsDbSet(products);

        var productRepository = new ProductRepository(mockContext.Object);

        // Act
        var result = await productRepository.GetAllProducts("Product", 15, 35, new int?[] { 1, 3 });

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count);
        Assert.Contains(result, p => p.ProductId == 3);
    }
}
