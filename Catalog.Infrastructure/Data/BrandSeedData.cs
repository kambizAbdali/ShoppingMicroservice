using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class BrandSeedData
    {
        public static void SeedData(IMongoCollection<ProductBrand> collection)
        {
            var existCollection = collection.Find(x => true).Any();
            if (existCollection) return;

            var jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "brands.txt");
            if (!File.Exists(jsonPath))
            {
                throw new Exception($"Brand seed data not found in path: {jsonPath}");
            }

            var dataText = File.ReadAllText(jsonPath);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(dataText);
            if (brands != null)
                collection.InsertMany(brands);
        }
    }
}
