using System;
using System.Collections.Generic;
using System.Text;

namespace BricksWarehouse.Domain.Dto
{
    public class OutTaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int ProductTypeId { get; set; }
        public ProductTypeDto? ProductType { get; set; }
        public int Count { get; set; }
        public string TruckNumber { get; set; }
        public int Loaded { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? Comment { get; set; }
        public bool IsCompleted { get; set; }
    }
}
