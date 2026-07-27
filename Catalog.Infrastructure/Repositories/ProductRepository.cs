using Catalog.Core.Entities;
using Catalog.Core.EntityParams;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(string id)
    {
        return await _context.Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
    {
        return await _context.Products
            .Find(p => p.Name == name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByBrandAsync(string brand)
    {
        return await _context.Products
            .Find(p => p.Brands.Name == brand)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByBrandIdAsync(string brandId)
    {
        return await _context.Products
            .Find(p => p.Brands.Id == brandId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByTypeAsync(string type)
    {
        return await _context.Products
            .Find(p => p.Types.Name == type)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByTypeIdAsync(string typeId)
    {
        return await _context.Products
            .Find(p => p.Types.Id == typeId)
            .ToListAsync();
    }

    public async Task<Pagination<Product>> GetProductsAsync(CatalogParams catalogParams)
    {
        var filter = BuildFilter(catalogParams);
        var sort = BuildSort(catalogParams);

        var totalItems = await _context.Products.CountDocumentsAsync(filter);

        var data = await _context.Products
            .Find(filter)
            .Sort(sort)
            .Skip((catalogParams.PageIndex - 1) * catalogParams.PageSize)
            .Limit(catalogParams.PageSize)
            .ToListAsync();

        return new Pagination<Product>(
            catalogParams.PageIndex,
            catalogParams.PageSize,
            (int)totalItems,
            data);
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        await _context.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var result = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(Product product)
    {
        return await DeleteProductAsync(product.Id);
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var result = await _context.Products.DeleteOneAsync(p => p.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    private static SortDefinition<Product> BuildSort(CatalogParams catalogParams)
    {
        return catalogParams.Sort switch
        {
            "priceAsc" => Builders<Product>.Sort.Ascending(x => x.Price),
            "priceDesc" => Builders<Product>.Sort.Descending(x => x.Price),
            _ => Builders<Product>.Sort.Ascending(x => x.Name)
        };
    }

    private static FilterDefinition<Product> BuildFilter(CatalogParams catalogParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(catalogParams.Search))
        {
            var search = catalogParams.Search.Trim().ToLower();
            filter &= builder.Where(x => x.Name.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(catalogParams.BrandId))
        {
            filter &= builder.Eq(x => x.Brands.Id, catalogParams.BrandId);
        }

        if (!string.IsNullOrWhiteSpace(catalogParams.TypeId))
        {
            filter &= builder.Eq(x => x.Types.Id, catalogParams.TypeId);
        }

        return filter;
    }
}
