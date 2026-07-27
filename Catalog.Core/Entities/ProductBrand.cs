using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Core.Entities
{
    public class ProductBrand:BaseEntity
    {
        [BsonElement(nameof(Name))]
        public string Name { get; set; }
    }
}
