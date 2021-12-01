using System;
using System.Collections.Generic;
using System.Text;

namespace BricksWarehouse.Domain.Dto
{
    public class ProductTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FormatNumber { get; set; }
        public int Order { get; set; }
        public int Units { get; set; }
        public double Volume { get; set; }
        public double Weight { get; set; }
        public bool IsDelete { get; set; }
        public IEnumerable<int> PlacesIds { get; set; } = new List<int>();
    }
}
