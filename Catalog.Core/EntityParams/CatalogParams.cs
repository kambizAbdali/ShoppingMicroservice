using Catalog.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Core.EntityParams
{
    public class CatalogParams : CommonParams
    {
        public string? BrandId { get; set; }
        public string? TypeId { get; set; }
    }
}
