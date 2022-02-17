using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Dto;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock = new();
        private readonly Random random = new();
        private ProductsController _controller;

        #region Products
        [Fact]
        public async Task GivenNoProductId_WhenGetMethodIsCalledAndResultListHasValue_ThenReturnsAllProductList()
        {
            // Arrange
            var fakeProduct1 = CreateFakeProduct();
            var fakeProduct2 = CreateFakeProduct();
            var allFakes = new List<ProductDto>
            {
                fakeProduct1,
                fakeProduct2
            };

            var allExpectedProducts = new ProductsDto(allFakes);

            _productServiceMock.Setup(s => s.GetAllProducts())
                .ReturnsAsync(allExpectedProducts);

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.Get();

            // Assert
            result.Value.Should().BeEquivalentTo(allExpectedProducts);
        }

        [Fact]
        public async Task GivenNoProductId_WhenGetMethodIsCalledAndResultListHasNoValue_ThenReturnsEmptyList()
        {
            // Arrange
            var expectedEmptyProductList = new ProductsDto();

            _productServiceMock.Setup(s => s.GetAllProducts())
                .ReturnsAsync(expectedEmptyProductList);

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.Get();

            // Assert
            result.Value.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task GivenProductId_WhenGetMethodIsCalledAndResultHasValue_ThenReturnsExpectedProduct()
        {
            // Arrange
            var expectedItem = CreateFakeProduct();

            _productServiceMock.Setup(s => s.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.Get(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedItem);
        }

        [Fact]
        public async Task GivenProductId_WhenGetMethodIsCalledAndResultIsEmpty_ThenReturnsNoContent()
        {
            // Arrange
            _productServiceMock.Setup(s => s.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync((ProductDto)null);

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.Get(Guid.NewGuid());

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }
        #endregion

        public ProductDto CreateFakeProduct()
        {
            return new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Description = "some random description",
                Price = (decimal)random.Next(1000),
                DeliveryPrice = (decimal)random.Next(50),
            };
        }
        
    }
}
