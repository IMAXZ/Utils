using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.EntityFramework
{
    public class DbContextBase : DbContext
    {
        protected DbContextBase()
        {
        }

        protected DbContextBase(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        protected virtual Task BeforeSaveChanges() => Task.CompletedTask;

        public override int SaveChanges()
        {
            BeforeSaveChanges().Wait();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await BeforeSaveChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
