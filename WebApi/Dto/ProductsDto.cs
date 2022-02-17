using System.Collections.Generic;

namespace WebApi.Dto
{
    public class ProductsDto
    {
        public List<ProductDto> Items { get; private set; }

        public ProductsDto()
        {
            Items = new List<ProductDto>();
        }

        public ProductsDto(List<ProductDto> products)
        {
            Items = products;
        }
    }
}
