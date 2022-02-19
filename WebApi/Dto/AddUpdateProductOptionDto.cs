using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto
{
    public class AddUpdateProductOptionDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(24)]
        public string Description { get; set; }
    }
}
