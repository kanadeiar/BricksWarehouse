using BricksWarehouse.Domain.Dto;
using BricksWarehouse.Domain.Interfaces;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BricksWarehouse.Domain.Mappers;

namespace BricksWarehouse.Mobile.Services
{
    public class TaskClient : BaseClient
    {
        private readonly IMapper<OutTaskDto, OutTask> _mapperFrom;
        private readonly IMapper<ProductTypeDto, ProductType> _mapperProductTypeFrom;
        private readonly IMapper<PlaceDto, Place> _mapperPlaceFrom;

        public TaskClient(HttpClient client, IMapper<OutTaskDto, OutTask> mapperFrom, 
            IMapper<ProductTypeDto, ProductType> mapperProductTypeFrom, IMapper<PlaceDto, Place> mapperPlaceFrom) 
            : base(client, "/api/mobiletask")
        {
            _mapperFrom = mapperFrom;
            _mapperProductTypeFrom = mapperProductTypeFrom;
            _mapperPlaceFrom = mapperPlaceFrom;
        }

        public async Task<IEnumerable<OutTask>> GetAll()
        {
            var dtos = await GetAsync<IEnumerable<OutTaskDto>>(Address).ConfigureAwait(false);
            return dtos.Select(d => _mapperFrom.Map(d));
        }

        public async Task<OutTask> GetById(int id)
        {
            var dto = await GetAsync<OutTaskDto>($"{Address}/{id}").ConfigureAwait(false);
            return _mapperFrom.Map(dto);
        }

        public async Task<ProductType> GetProductTypeByFormat(int format)
        {
            var dto = await GetAsync<ProductTypeDto>($"{Address}/producttype/{format}").ConfigureAwait(false);
            return _mapperProductTypeFrom.Map(dto);
        }

        public async Task<IEnumerable<Place>> GetRecommendedLoadPlaces(int productTypeId)
        {
            var dtos = await GetAsync<IEnumerable<PlaceDto>>($"{Address}/producttypesload/{productTypeId}").ConfigureAwait(false);
            return dtos.Select(d => _mapperPlaceFrom.Map(d));
        }

        public async Task<Place> GetPlaceByNumber(int number)
        {
            var dto = await GetAsync<PlaceDto>($"{Address}/place/{number}").ConfigureAwait(false);
            return _mapperPlaceFrom.Map(dto);
        }

        public async Task<Place> LoadProductToPlace(int productTypeId, int placeId, int count)
        {
            var dto = await GetAsync<PlaceDto>($"{Address}/load/{productTypeId}/{placeId}/{count}").ConfigureAwait(false);
            return _mapperPlaceFrom.Map(dto);
        }

        public async Task<IEnumerable<Place>> GetRecommendedShipmentPlaces(int productTypeId)
        {
            var dtos = await GetAsync<IEnumerable<PlaceDto>>($"{Address}/producttypeshipment/{productTypeId}").ConfigureAwait(false);
            return dtos.Select(d => _mapperPlaceFrom.Map(d));
        }

        public async Task<Place> ShipmentProductToPlace(int placeId, int taskId, int count)
        {
            var dto = await GetAsync<PlaceDto>($"{Address}/shipment/{placeId}/{taskId}/{count}").ConfigureAwait(false);
            return _mapperPlaceFrom.Map(dto);
        }
    }
}
