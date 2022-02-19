using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class ProductRepository: Repository<Productx>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext): base(dbContext)
        {
        }

        public Task<ProductOptionx> GetOptionForProduct(Guid productId, Guid productOptionId)
        {
           return _dbContext.Set<ProductOptionx>()
                .SingleOrDefaultAsync(p => p.ProductId == productId && p.Id == productOptionId);
        }

        public async Task<IEnumerable<ProductOptionx>> GetAllOptionsForProduct(Guid productId)
        {
            return await _dbContext.Set<ProductOptionx>()
                .Where(p => p.Product.Id == productId)
                .ToListAsync();
        }

        public async Task AddProductOption(ProductOptionx productOption)
        {
            await _dbContext.Set<ProductOptionx>().AddAsync(productOption);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductOption(Guid productOptionId, string name, string description)
        {
            var productOption = await _dbContext.Set<ProductOptionx>()
                .SingleOrDefaultAsync(p => p.Id == productOptionId);

            productOption.UpdateProductOption(name, description);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductOption(Guid productOptionId)
        {
            var productOption = await _dbContext.Set<ProductOptionx>()
                .SingleOrDefaultAsync(p => p.Id == productOptionId);

            _dbContext.Set<ProductOptionx>().Remove(productOption);

            await _dbContext.SaveChangesAsync();
        }
    }
}
