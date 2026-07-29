using Catalog.Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Responses
{
    public class ProductResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }

        public string Description { get; set; }
        public string ImageFile { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        //Relations
        public BrandResponse Brands { get; set; }
        public TypeResponse Types { get; set; }
    }
}
