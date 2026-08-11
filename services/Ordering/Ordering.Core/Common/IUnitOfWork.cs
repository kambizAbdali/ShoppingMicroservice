using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Core.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
