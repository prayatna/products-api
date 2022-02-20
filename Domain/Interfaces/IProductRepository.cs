using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        // Product Options
        Task<IEnumerable<ProductOption>> GetAllOptionsForProduct(Guid productId);

        Task<ProductOption> GetOptionForProduct(Guid productId, Guid productOptionId);

        Task AddProductOption(ProductOption productOption);

        Task UpdateProductOption(Guid productOptionId, string name, string description);

        Task DeleteProductOption(Guid productOptionId);
    }
}
