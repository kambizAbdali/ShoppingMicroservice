using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public static class TypeSeedData
    {
        public static void SeedData(IMongoCollection<ProductType> collection)
        {
            var existCollection = collection.Find(x => true).Any();
            if (existCollection) return;

            var jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "types.json");
            if (!File.Exists(jsonPath))
            {
                throw new Exception($"Type seed data not found in path: {jsonPath}");
            }

            var dataText = File.ReadAllText(jsonPath);
            var types = JsonSerializer.Deserialize<List<ProductType>>(dataText);
            if (types != null)
                collection.InsertMany(types);
        }
    }
}
