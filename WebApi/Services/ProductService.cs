using System;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
