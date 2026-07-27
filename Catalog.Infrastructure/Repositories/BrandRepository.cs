using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Repositories
{
    
    public class BrandRepository(ICatalogContext context) : IBrandRepository
    {
        public async Task<IEnumerable<ProductBrand>> GetProductBrandsAsync()
        {
            return await context.Brands.Find(x=>true).ToListAsync();
        }
    }
}
