using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext): base(dbContext)
        {
        }

        public Task<ProductOption> GetOptionForProduct(Guid productId, Guid productOptionId)
        {
           return _dbContext.Set<ProductOption>()
                .SingleOrDefaultAsync(p => p.ProductId == productId && p.Id == productOptionId);
        }

        public async Task<IEnumerable<ProductOption>> GetAllOptionsForProduct(Guid productId)
        {
            return await _dbContext.Set<ProductOption>()
                .Where(p => p.Product.Id == productId)
                .ToListAsync();
        }

        public async Task AddProductOption(ProductOption productOption)
        {
            await _dbContext.Set<ProductOption>().AddAsync(productOption);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductOption(Guid productOptionId, string name, string description)
        {
            var productOption = await _dbContext.Set<ProductOption>()
                .SingleOrDefaultAsync(p => p.Id == productOptionId);

            productOption.UpdateProductOption(name, description);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductOption(Guid productOptionId)
        {
            var productOption = await _dbContext.Set<ProductOption>()
                .SingleOrDefaultAsync(p => p.Id == productOptionId);

            _dbContext.Set<ProductOption>().Remove(productOption);

            await _dbContext.SaveChangesAsync();
        }
    }
}
