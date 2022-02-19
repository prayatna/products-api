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
        public async Task GivenGetProducts_WhenMethodIsCalledAndResultListHasValue_ThenReturnsAllProductList()
        {
            // Arrange
            var allProductList = new List<ProductDto>
            {
                CreateFakeProduct(),
                CreateFakeProduct()
            };

            var allExpectedProducts = new ProductsDto(allProductList);

            _productServiceMock.Setup(s => s.GetAllProducts())
                .ReturnsAsync(allExpectedProducts);

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.GetProductsAsync();

            // Assert
            result.Value.Should().BeEquivalentTo(allExpectedProducts);
        }

        [Fact]
        public async Task GivenGetProducts_WhenMethodIsCalledAndResultListHasNoValue_ThenReturnsEmptyList()
        {
            // Arrange
            var expectedEmptyProductList = new ProductsDto();

            _productServiceMock.Setup(s => s.GetAllProducts())
                .ReturnsAsync(expectedEmptyProductList);

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.GetProductsAsync();

            // Assert
            result.Value.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task GivenProductId_WhenGetProductMethodIsCalledAndResultHasValue_ThenReturnsExpectedProduct()
        {
            // Arrange
            var expectedItem = CreateFakeProduct();

            _productServiceMock.Setup(s => s.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.GetProductAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedItem);
        }

        [Fact]
        public async Task GivenProductId_WhenGetProductMethodIsCalledAndResultIsEmpty_ThenReturnsNoContent()
        {
            // Arrange
            _productServiceMock.Setup(s => s.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync((ProductDto)null);

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.GetProductAsync(Guid.NewGuid());

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task GivenNewProduct_WhenPostMethodIsCalled_ThenReturnsCreatedProduct()
        {
            // Arrange
            var toAddNewProduct = new AddUpdateProductDto
            {
                Name = "Fake product",
                Description = "Some random Description",
                Price = 989.99M,
                DeliveryPrice = 10.00M
            };
            var expectedNewProduct = new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = toAddNewProduct.Name,
                Description = toAddNewProduct.Description,
                Price = toAddNewProduct.Price,
                DeliveryPrice = toAddNewProduct.DeliveryPrice
            };
            _productServiceMock.Setup(s => s.AddProduct(It.IsAny<AddUpdateProductDto>()))
                .ReturnsAsync(expectedNewProduct);

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.PostProductAsync(toAddNewProduct);

            // Assert
            var createdItem = ((CreatedAtActionResult)result.Result).Value as ProductDto;

            createdItem.Should().BeEquivalentTo(expectedNewProduct);
            createdItem.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GivenProductIdAndUpdatedProduct_WhenUpdateProductMethodIsCalled_ThenReturnsNoContent()
        {
            // Arrange
            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.UpdateAsync(Guid.NewGuid(), new AddUpdateProductDto());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GivenInvalidProductIdAndUpdatedProduct_WhenUpdateProductMethodIsCalled_ThenReturnsBadRequest()
        {
            // Arrange
            var nonExistantProductId = Guid.NewGuid();
            _productServiceMock.Setup(s => s.UpdateProduct(It.IsAny<Guid>(), It.IsAny<AddUpdateProductDto>()))
                .ThrowsAsync(new ApplicationException($"No product found for productId: {nonExistantProductId}"));

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.UpdateAsync(Guid.NewGuid(), new AddUpdateProductDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenProductId_WhenDeleteProductMethodIsCalled_ThenReturnsNoContent()
        {
            // Arrange
            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GivenInvalidProductId_WhenDeleteProductMethodIsCalled_ThenReturnsBadRequest()
        {
            // Arrange
            var nonExistantProductId = Guid.NewGuid();
            _productServiceMock.Setup(s => s.DeleteProduct(It.IsAny<Guid>()))
                .ThrowsAsync(new ApplicationException($"No product found for productId: {nonExistantProductId}"));

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region ProductOptions

        [Fact]
        public async Task GivenNewProductOption_WhenPostMethodIsCalled_ThenReturnsCreatedProductOption()
        {
            // Arrange
            var toAddNewProductOption = new AddUpdateProductOptionDto
            {
                Name = "Fake product option",
                Description = "Some random Description",
            };
            var expectedNewProductOption = new ProductOptionDto
            {
                Id = Guid.NewGuid(),
                Name = toAddNewProductOption.Name,
                Description = toAddNewProductOption.Description,
            };
            _productServiceMock.Setup(s => s.AddOptionForProduct(It.IsAny<Guid>(),It.IsAny<AddUpdateProductOptionDto>()))
                .ReturnsAsync(expectedNewProductOption);

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.CreateOptionAsync(Guid.NewGuid(), toAddNewProductOption);

            // Assert
            var createdItem = ((CreatedAtActionResult)result.Result).Value as ProductOptionDto;

            createdItem.Should().BeEquivalentTo(expectedNewProductOption);
            createdItem.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GivenInvalidProductIdAndNewProductOption_WhenPostMethodIsCalled_ThenReturnsBadRequest()
        {
            // Arrange
            var toAddNewProductOption = new AddUpdateProductOptionDto
            {
                Name = "Fake product option",
                Description = "Some random Description",
            };
            
            _productServiceMock.Setup(s => s.AddOptionForProduct(It.IsAny<Guid>(), It.IsAny<AddUpdateProductOptionDto>()))
                .ThrowsAsync(new ApplicationException("Cannot have production option without a product"));

            _controller = new ProductsController(_productServiceMock.Object);

            // Act
            var result = await _controller.CreateOptionAsync(Guid.NewGuid(), toAddNewProductOption);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
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
