using BricksWarehouse.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BricksWarehouse.Domain.Dto
{
    public class PlaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int Number { get; set; }
        public int ProductTypeId { get; set; }
        public ProductTypeDto ProductType { get; set; }
        public int Count { get; set; }
        public int Size { get; set; }
        public DateTime LastDateTime { get; set; }
        public PlaceStatus PlaceStatus { get; set; }
        public string Comment { get; set; }
        public bool IsDelete { get; set; }
    }
}
