using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.EntityFramework
{
    public static class EFUnitOfWorkExtensions
    {
        public static DbSet<TEntity> DbSet<TEntity>(this IEFUnitOfWork<DbContext> unitOfWork)
            where TEntity : class
        {
            if (null == unitOfWork)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }
            return unitOfWork.DbContext.Set<TEntity>();
        }

        public static IEFRepository<TDbContext, TEntity> GetRepository<TDbContext, TEntity>(this IEFUnitOfWork<TDbContext> unitOfWork)
            where TDbContext : DbContext
            where TEntity : class
        {
            return new EFRepository<TDbContext, TEntity>(unitOfWork.DbContext);
        }
    }
}
