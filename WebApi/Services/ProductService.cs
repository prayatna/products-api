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

        public async Task<ProductDto> GetProductById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

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

        public async Task<ProductDto> AddProduct(AddProductDto product)
        {

            var productEntity = new Productx(product.Name, product.Description,
                product.Price, product.DeliveryPrice);

            await _productRepository.AddAsync(productEntity);

            return productEntity.AsDto();
        }

        public async Task UpdateProduct(Guid productId, AddProductDto product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productId);

            if(existingProduct is null)
            {
                throw new ApplicationException($"No product found for productId: {productId}");
            }

            existingProduct.UpdateProduct(product.Name, product.Description,
                product.Price, product.DeliveryPrice);

            await _productRepository.UpdateAsync();
        }
    }
}
