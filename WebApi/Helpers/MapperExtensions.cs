using Domain.Entity;
using WebApi.Dto;

namespace WebApi.Helpers
{
    public static class MapperExtensions
    {
        public static ProductDto AsDto(this Productx product)
        {
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
