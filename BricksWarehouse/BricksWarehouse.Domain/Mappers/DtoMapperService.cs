using BricksWarehouse.Domain.Dto;
using BricksWarehouse.Domain.Interfaces;
using BricksWarehouse.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BricksWarehouse.Domain.Mappers
{
    public class DtoMapperService : IMapper<ProductType, ProductTypeDto>, IMapper<ProductTypeDto, ProductType>
    {
        ProductTypeDto IMapper<ProductType, ProductTypeDto>.Map(ProductType source)
        {
            var data = source;
            var dto = new ProductTypeDto
            {
                Id = data.Id,
                Name = data.Name,
                FormatNumber = data.FormatNumber,
                Order = data.Order,
                Units = data.Units,
                Volume = data.Volume,
                Weight = data.Weight,
                IsDelete = data.IsDelete,
                PlacesIds = data.Places?.Select(p => p.Id) ?? Enumerable.Empty<int>(),
            };
            return dto;
        }
        ProductType IMapper<ProductTypeDto, ProductType>.Map(ProductTypeDto source)
        {
            var dto = source;
            var data = new ProductType
            {
                Id = dto.Id,
                Name = dto.Name,
                FormatNumber = dto.FormatNumber,
                Order = dto.Order,
                Units = dto.Units,
                Volume = dto.Volume,
                Weight = dto.Weight,
                IsDelete = dto.IsDelete,
                Places = dto.PlacesIds.Select(p => new Place { Id = p }).ToArray(),
            };
            return data;
        }
    }
}
