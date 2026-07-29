using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class ProductSeedData
    {
        public static void SeedData(IMongoCollection<Product> collection)
        {
            var existCollection = collection.Find(x => true).Any();
            if (existCollection) return;

            var jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "products.json");
            if (!File.Exists(jsonPath))
            {
                throw new Exception($"Product seed data not found in path: {jsonPath}");
            }

            var dataText = File.ReadAllText(jsonPath);
            var products = JsonSerializer.Deserialize<List<Product>>(dataText);
            if (products != null)
                collection.InsertMany(products);
        }
    }
}
