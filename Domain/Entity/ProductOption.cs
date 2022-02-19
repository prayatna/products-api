using System;
namespace Domain.Entity
{
    public class ProductOptionx
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public Guid ProductId { get; private set; }

        public Productx Product { get; private set; }

        public ProductOptionx()
        {

        }

        public ProductOptionx(string name, string description, Productx product)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Product = product;
        }
    }
}
