﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        ValueTask<TEntity> GetByIdAsync(Guid id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        Task AddAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task UpdateAsync(); //TODO: test

    }
}
