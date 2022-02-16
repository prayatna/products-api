using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interfaces;

namespace Data.Repository
{
    public class ProductRepository: Repository<Productx>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext): base(dbContext)
        {
        }

        public Task<ProductOptionx> GetOption(Guid productId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateOption(Guid productId, ProductOptionx productOption)
        {
            var product = await GetByIdAsync(productId);
            productOption.Product = product;

            await _dbContext.Set<ProductOptionx>().AddAsync(productOption);
            await UpdateAsync();
        }

        public IEnumerable<ProductOptionx> GetAllOptionsForProduct(Guid productId)
        {
            return _dbContext.Set<ProductOptionx>()
                .Where(p => p.Product.Id == productId)
                .AsEnumerable();
        }
        
    }
}
