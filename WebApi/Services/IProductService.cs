using System;
using System.Threading.Tasks;
using WebApi.Dto;

namespace WebApi.Services
{
    public interface IProductService
    {
        Task<ProductDto> GetProductById(Guid id);

        Task<ProductsDto> GetAllProducts();

        Task<ProductDto> AddProduct(AddProductDto product);
    }
}
