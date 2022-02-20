using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interfaces;
using WebApi.Dto;
using WebApi.Helpers;

namespace WebApi.Services
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        #region Products
        public async Task<ProductDto> GetProductById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if(product is null)
            {
                throw new ApplicationException($"Product with {id} not found");
            }

            return product.AsDto();
        }

        public async Task<ProductsDto> GetAllProducts()
        {
            var products = (await _productRepository.GetAllAsync())
                .Select(a => a.AsDto())
                .ToList();

            var allProducts = new ProductsDto(products);

            return allProducts;
        }

        public async Task<ProductsDto> GetAllProductsByName(string name)
        {
            var products = (await _productRepository.GetProductsByName(name))
                .Select(p => p.AsDto())
                .ToList(); ;

            var allProducts = new ProductsDto(products);

            return allProducts;
        }

        public async Task<ProductDto> AddProduct(AddUpdateProductDto product)
        {

            var productEntity = new Product(product.Name, product.Description,
                product.Price, product.DeliveryPrice);

            await _productRepository.AddAsync(productEntity);

            return productEntity.AsDto();
        }

        public async Task UpdateProduct(Guid productId, AddUpdateProductDto product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productId);

            if (existingProduct is null)
            {
                throw new ApplicationException($"No product found for productId: {productId}");
            }

            existingProduct.UpdateProduct(product.Name, product.Description,
                product.Price, product.DeliveryPrice);

            await _productRepository.UpdateAsync();
        }

        public async Task DeleteProduct(Guid productId)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productId);

            if (existingProduct is null)
            {
                throw new ApplicationException($"No product found for productId: {productId}");
            }

            await _productRepository.DeleteAsync(existingProduct); // TODO: cascade delete product options
        }

        #endregion

        #region ProductOptions

        public async Task<ProductOptionsDto> GetAllOptionsForProduct(Guid productId)
        {
            var productOptions = (await _productRepository.GetAllOptionsForProduct(productId))
                .Select(po => po.AsDto())
                .ToList();

            var allProductOptions = new ProductOptionsDto(productOptions);

            return allProductOptions;
        }

        public async Task<ProductOptionDto> GetOptionForProduct(Guid productId, Guid productOptionId)
        {
            return (await _productRepository.GetOptionForProduct(productId, productOptionId)).AsDto();
        }

        public async Task<ProductOptionDto> AddOptionForProduct(Guid productId,
            AddUpdateProductOptionDto productOptionDto)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if(product is null)
            {
                throw new ApplicationException("Cannot have production option without a product");
            }

            var newProductOption = new ProductOption(productOptionDto.Name, productOptionDto.Description, product);

            await _productRepository.AddProductOption(newProductOption);

            return newProductOption.AsDto();
        }

        public async Task UpdateProductOption(Guid productId, AddUpdateProductOptionDto productOptionDto)
        {
            await _productRepository.UpdateProductOption(productId, productOptionDto.Name, productOptionDto.Description);
        }

        public async Task DeleteProductOption(Guid productId)
        {
            await _productRepository.DeleteProductOption(productId);
        }

        #endregion
    }
}
