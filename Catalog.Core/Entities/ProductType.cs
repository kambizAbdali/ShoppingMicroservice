using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Core.Entities
{
    public class ProductType : BaseEntity
    {
        [BsonElement(nameof(Name))]
        public string Name { get; set; }
    }
}
