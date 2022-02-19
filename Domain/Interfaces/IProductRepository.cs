using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Interfaces
{
    public interface IProductRepository : IRepository<Productx>
    {
        // Product Options
        Task<IEnumerable<ProductOptionx>> GetAllOptionsForProduct(Guid productId);

        Task<ProductOptionx> GetOptionForProduct(Guid productId, Guid productOptionId);

        Task AddProductOption(ProductOptionx productOption);

        Task UpdateProductOption(Guid productOptionId, string name, string description);

        //Task DeleteOption(Guid id);
    }
}
