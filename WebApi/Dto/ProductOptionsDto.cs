using System.Collections.Generic;

namespace WebApi.Dto
{
    public class ProductOptionsDto
    {
        public List<ProductOptionDto> Items { get; private set; }

        public ProductOptionsDto()
        {
            Items = new List<ProductOptionDto>();
        }

        public ProductOptionsDto(List<ProductOptionDto> products)
        {
            Items = products;
        }
    }
}
