using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto
{
    public class AddProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.5, 10000.00)]
        public decimal Price { get; set; }

        [Range(0.0, 200.0)]
        public decimal DeliveryPrice { get; set; }
    }
}
