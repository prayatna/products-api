using System;
using System.Threading.Tasks;
using Domain.Interfaces;
using WebApi.Dto;

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

            //TODO: mapper
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };
        }
    }
}
