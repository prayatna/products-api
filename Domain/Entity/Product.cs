using System;
using System.Collections.Generic;

namespace Domain.Entity
{
    public class Productx
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public decimal Price { get; private set; }

        public decimal DeliveryPrice { get; private set; }

        public List<ProductOptionx> ProductOptions { get; } = new();

        public Productx(string name, string description, decimal price,
            decimal deliveryPrice)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            DeliveryPrice = deliveryPrice;
        }

        public void UpdateProduct(string name, string description, decimal price,
            decimal deliveryPrice)
        {
            Name = name;
            Description = description;
            Price = price;
            DeliveryPrice = deliveryPrice;
        }
    }
}
