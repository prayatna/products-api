using System;
using System.Threading.Tasks;
using Domain.Interfaces;
using WebApi.Dto;
using WebApi.Helpers;

namespace WebApi.Services
{
    public class ProductService: IProductService
    {
        private IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> GetProductById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            return product.AsDto();
        }
    }
}
