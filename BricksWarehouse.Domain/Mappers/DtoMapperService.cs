using BricksWarehouse.Domain.Dto;
using BricksWarehouse.Domain.Interfaces;
using BricksWarehouse.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BricksWarehouse.Domain.Mappers
{
    public class DtoMapperService : IMapper<ProductType, ProductTypeDto>, IMapper<ProductTypeDto, ProductType>, IMapper<Place, PlaceDto>, IMapper<PlaceDto, Place>,
        IMapper<OutTask, OutTaskDto>, IMapper<OutTaskDto, OutTask>
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

        PlaceDto IMapper<Place, PlaceDto>.Map(Place source)
        {
            var data = source;
            IMapper<ProductType, ProductTypeDto> mapperTo = this;
            var dto = new PlaceDto
            {
                Id = data.Id,
                Name = data.Name,
                Order = data.Order,
                Number = data.Number,
                ProductTypeId = data.ProductTypeId ?? 0,
                ProductType = (data.ProductType is { } productType) ? mapperTo.Map(productType) : null,
                Count = data.Count,
                Size = data.Size,
                LastDateTime = data.LastDateTime,
                PlaceStatus = data.PlaceStatus,
                Comment = data.Comment,
                IsDelete = data.IsDelete,
            };
            return dto;
        }

        Place IMapper<PlaceDto, Place>.Map(PlaceDto source)
        {
            var dto = source;
            IMapper<ProductTypeDto, ProductType> mapperFrom = this;
            int? productTypeId = dto.ProductTypeId;
            if (productTypeId == 0) productTypeId = null;
            var place = new Place
            {
                Id = dto.Id,
                Name = dto.Name,
                Order = dto.Order,
                Number = dto.Number,
                ProductTypeId = productTypeId,
                ProductType = (dto.ProductType is { } productType) ? mapperFrom.Map(productType) : null,
                Count = dto.Count,
                Size = dto.Size,
                LastDateTime = dto.LastDateTime,
                PlaceStatus = dto.PlaceStatus,
                Comment = dto.Comment,
                IsDelete = dto.IsDelete,
            };
            return place;
        }

        OutTaskDto IMapper<OutTask, OutTaskDto>.Map(OutTask source)
        {
            var data = source;
            IMapper<ProductType, ProductTypeDto> mapperTo = this;
            var dto = new OutTaskDto
            {
                Id = data.Id,
                Name = data.Name,
                Number = data.Number,
                ProductTypeId = data.ProductTypeId ?? 0,
                ProductType = (data.ProductType is { } productType) ? mapperTo.Map(productType) : null,
                Count = data.Count,
                TruckNumber = data.TruckNumber,
                Loaded = data.Loaded,
                CreatedDateTime = data.CreatedDateTime,
                Comment = data.Comment,
                IsCompleted = data.IsCompleted,
            };
            return dto;
        }

        OutTask IMapper<OutTaskDto, OutTask>.Map(OutTaskDto source)
        {
            var dto = source;
            IMapper<ProductTypeDto, ProductType> mapperFrom = this;
            int? productTypeId = dto.ProductTypeId;
            if (productTypeId == 0) productTypeId = null;
            var outTask = new OutTask
            {
                Id = dto.Id,
                Name = dto.Name,
                Number = dto.Number,
                ProductTypeId = productTypeId,
                ProductType = (dto.ProductType is { } productType) ? mapperFrom.Map(productType) : null,
                Count = dto.Count,
                TruckNumber = dto.TruckNumber,
                Loaded = dto.Loaded,
                CreatedDateTime = dto.CreatedDateTime,
                Comment = dto.Comment,
                IsCompleted = dto.IsCompleted,
            };
            return outTask;
        }
    }
}
