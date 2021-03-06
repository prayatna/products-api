using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly Mock<ILogger<ProductsController>> _loggerMock = new();
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

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.GetProductsAsync(string.Empty);

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

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.GetProductsAsync(string.Empty);

            // Assert
            result.Value.Items.Should().BeEmpty();
            _productServiceMock.Verify(service => service.GetAllProducts(), Times.Once());
            _productServiceMock.Verify(service => service.GetAllProductsByName(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task GivenGetProductsAndNameAsQuery_WhenMethodIsCalledAndResultListHasNoValue_ThenReturnsEmptyList()
        {
            // Arrange
            var searchName = "randomName";
            var expectedEmptyProductList = new ProductsDto();

            _productServiceMock.Setup(s => s.GetAllProductsByName(searchName))
                .ReturnsAsync(expectedEmptyProductList);

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.GetProductsAsync(searchName);

            // Assert
            result.Value.Items.Should().BeEmpty();
            _productServiceMock.Verify(service => service.GetAllProducts(), Times.Never());
            _productServiceMock.Verify(service => service.GetAllProductsByName(searchName), Times.Once());
        }

        [Fact]
        public async Task GivenProductId_WhenGetProductMethodIsCalledAndResultHasValue_ThenReturnsExpectedProduct()
        {
            // Arrange
            var expectedItem = CreateFakeProduct();

            _productServiceMock.Setup(s => s.GetProductById(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

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
                .ThrowsAsync(new ApplicationException($"Product with id not found"));

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.GetProductAsync(Guid.NewGuid());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
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

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

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
            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

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

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.UpdateAsync(Guid.NewGuid(), new AddUpdateProductDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenProductId_WhenDeleteProductMethodIsCalled_ThenReturnsNoContent()
        {
            // Arrange
            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

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

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

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

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

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

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.CreateOptionAsync(Guid.NewGuid(), toAddNewProductOption);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GivenProductId_WhenGetProductOptionsIsCalledAndResultListHasValue_ThenReturnsAllProductOptions()
        {
            // Arrange
            var allProductOptionList = new List<ProductOptionDto>
            {
                CreateFakeProductOption(),
                CreateFakeProductOption()
            };

            var allExpectedProductOptions = new ProductOptionsDto(allProductOptionList);

            _productServiceMock.Setup(s => s.GetAllOptionsForProduct(It.IsAny<Guid>()))
                .ReturnsAsync(allExpectedProductOptions);

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.GetAllOptionsAsync(It.IsAny<Guid>());

            // Assert
            result.Value.Should().BeEquivalentTo(allExpectedProductOptions);
        }

        [Fact]
        public async Task GivenProductId_WhenGetProductOptionsIsCalledAndResultListHasNoValue_ThenReturnsEmptyList()
        {
            // Arrange
            var expectedEmptyProductOptionList = new ProductOptionsDto();

            _productServiceMock.Setup(s => s.GetAllOptionsForProduct(It.IsAny<Guid>()))
                .ReturnsAsync(expectedEmptyProductOptionList);

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.GetAllOptionsAsync(Guid.NewGuid());

            // Assert
            result.Value.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task GivenProductIdAndOptionId_WhenGetOptionMethodIsCalledAndResultHasValue_ThenReturnsExpectedOption()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var optionId = Guid.NewGuid();
            var expectedOption = CreateFakeProductOption(optionId);

            _productServiceMock.Setup(s => s.GetOptionForProduct(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(expectedOption);

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.GetOptionAsync(productId, optionId);

            // Assert
            result.Value.Should().BeEquivalentTo(expectedOption);
        }

        [Fact]
        public async Task GivenProductIdAndOptionId_WhenGetOptionMethodIsCalledAndResultHasNoValue_ThenReturnsBadRequest()
        {
            // Arrange
            var expectedOption = CreateFakeProductOption();

            _productServiceMock.Setup(s => s.GetOptionForProduct(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ThrowsAsync(new NullReferenceException());

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.GetOptionAsync(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GivenProductOptionIdAndUpdatedProductOption_WhenUpdateProductOptionMethodIsCalled_ThenReturnsNoContent()
        {
            // Arrange
            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.UpdateOptionAsync(Guid.NewGuid(), new AddUpdateProductOptionDto());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GivenNonExistantProductOptionIdAndUpdatedProductOption_WhenUpdateProductOptionMethodIsCalled_ThenReturnsBadRequest()
        {
            // Arrange
            var nonExistantProductOptionId = Guid.NewGuid();
            _productServiceMock.Setup(s => s.UpdateProductOption(It.IsAny<Guid>(), It.IsAny<AddUpdateProductOptionDto>()))
                .ThrowsAsync(new NullReferenceException());

            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.UpdateOptionAsync(Guid.NewGuid(), new AddUpdateProductOptionDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GivenProductOptionId_WhenDeleteProductOptionMethodIsCalled_ThenReturnsNoContent()
        {
            // Arrange
            _controller = new ProductsController(_productServiceMock.Object, _loggerMock.Object);

            // Act
            var result = await _controller.DeleteOption(Guid.NewGuid());

            // Assert
            Assert.IsType<NoContentResult>(result);
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

        public ProductOptionDto CreateFakeProductOption(Guid? optionId = null)
        {
            return new ProductOptionDto
            {
                Id = optionId == null ? Guid.NewGuid() : optionId.Value,
                Name = Guid.NewGuid().ToString(),
                Description = "some random description",
            };
        }
    }
}
