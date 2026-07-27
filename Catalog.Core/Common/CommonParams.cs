using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Core.Common
{
    public class CommonParams
    {
        private int _pageSize = 10;
        public int PageIndex { get; set; } = 1;

        private const int MaxPageSize = 80;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? Sort { get; set; }
        public string? Search { get; set; }
    }
}
