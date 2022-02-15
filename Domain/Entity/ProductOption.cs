using System;
namespace Domain.Entity
{
    public class ProductOptionx
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid ProductId { get; set; }

        public Productx Product { get; set; }
    }
}
