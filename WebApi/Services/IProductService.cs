using System;
using System.Threading.Tasks;
using WebApi.Dto;

namespace WebApi.Services
{
    public interface IProductService
    {
        Task<ProductDto> GetProductById(Guid id);

        Task<ProductsDto> GetAllProducts();

        Task<ProductDto> AddProduct(AddUpdateProductDto product);

        Task UpdateProduct(Guid productId, AddUpdateProductDto product);

        Task DeleteProduct(Guid productId);

        Task<ProductOptionsDto> GetAllOptionsForProduct(Guid productId);

        Task<ProductOptionDto> GetOptionForProduct(Guid productId, Guid productOptionId);

        Task<ProductOptionDto> AddOptionForProduct(Guid productId,
            AddUpdateProductOptionDto productOptionDto);

        Task UpdateProductOption(Guid productId, AddUpdateProductOptionDto productOptionDto);

        Task DeleteProductOption(Guid productId);
    }
}
