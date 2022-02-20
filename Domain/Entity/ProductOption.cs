using System;
namespace Domain.Entity
{
    public class ProductOption
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public Guid ProductId { get; private set; }

        public Product Product { get; private set; }

        public ProductOption()
        {

        }

        public ProductOption(string name, string description, Product product)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Product = product;
        }

        public void UpdateProductOption(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
