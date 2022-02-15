using System;
using System.Collections.Generic;

namespace RefactorThis.Domain.Models
{
    public class Productx
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public List<ProductOptionx> ProductOptions { get; } = new();
    }
}
