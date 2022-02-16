using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Interfaces
{
    public interface IProductRepository : IRepository<Productx>
    {
        // Product Options
        //TODO: add what is required
        IEnumerable<ProductOptionx> GetAllOptionsForProduct(Guid productId);

        Task<ProductOptionx> GetOption(Guid productId);

        Task CreateOption(Guid productId, ProductOptionx productOption);

        //Task UpdateOption(Guid id, ProductOptionx productOption);

        //Task DeleteOption(Guid id);
    }
}
