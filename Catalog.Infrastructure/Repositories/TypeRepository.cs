using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Repositories
{
    public class TypeRepository(ICatalogContext context) : ITypeRepository
    {
        public async Task<IEnumerable<ProductType>> GetProductTypeAsync()
        {
            return await context.Types.Find(x=>true).ToListAsync();
        }
    }
}