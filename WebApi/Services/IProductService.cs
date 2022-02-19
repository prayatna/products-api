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

        Task<ProductOptionsDto> GetAllProductOptionsForProduct(Guid productId);

        Task<ProductOptionDto> AddOptionForProduct(Guid productId,
            AddUpdateProductOptionDto productOptionDto);
    }
}
