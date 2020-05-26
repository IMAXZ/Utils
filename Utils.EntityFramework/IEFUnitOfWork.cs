using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.EntityFramework
{
    public interface IEFUnitOfWork<out TDbContext>:IDisposable where TDbContext : DbContext
    {
        void Commit();

        Task CommitAsync(CancellationToken cancellationToken = default);

        void Rollback();

        Task RollbackAsync(CancellationToken cancellationToken = default);

        TDbContext DbContext { get; }
    }
}
