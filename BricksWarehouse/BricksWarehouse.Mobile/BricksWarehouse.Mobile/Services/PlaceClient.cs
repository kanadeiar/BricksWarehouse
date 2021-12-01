using BricksWarehouse.Domain.Dto;
using BricksWarehouse.Domain.Interfaces;
using BricksWarehouse.Domain.Models;
using BricksWarehouse.Mobile.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BricksWarehouse.Mobile.Services
{
    public class PlaceClient : BaseClient
    {
        private readonly IMapper<Place, PlaceDto> _mapperTo;
        private readonly IMapper<PlaceDto, Place> _mapperFrom;
        public PlaceClient(HttpClient client, IMapper<Place, PlaceDto> mapperTo, IMapper<PlaceDto, Place> mapperFrom) 
            : base(client, "/api/mobileplace")
        {
            _mapperTo = mapperTo;
            _mapperFrom = mapperFrom;
        }

        public async Task<IEnumerable<Place>> GetAll()
        {
            var dtos = await GetAsync<IEnumerable<PlaceDto>>(Address).ConfigureAwait(false);
            return dtos.Select(d => _mapperFrom.Map(d));
        }

        public async Task<IEnumerable<Place>> GetTrashed()
        {
            var dtos = await GetAsync<IEnumerable<PlaceDto>>($"{Address}/trashed").ConfigureAwait(false);
            return dtos.Select(d => _mapperFrom.Map(d));
        }

        public async Task<Place> GetById(int id)
        {
            var dto = await GetAsync<PlaceDto>($"{Address}/{id}").ConfigureAwait(false);
            return _mapperFrom.Map(dto);
        }

        public async Task<Place> Add(Place place)
        {
            var dto = _mapperTo.Map(place);
            var result = await PostAsync(Address, dto).ConfigureAwait(false);
            dto = await result.Content.ReadFromJsonAsync<PlaceDto>();
            return _mapperFrom.Map(dto);
        }

        public async Task<Place> Update(Place place)
        {
            var dto = _mapperTo.Map(place);
            var result = await PutAsync(Address, dto).ConfigureAwait(false);
            dto = await result.Content.ReadFromJsonAsync<PlaceDto>();
            return _mapperFrom.Map(dto);
        }

        public async Task<Place> Delete(int id)
        {
            var result = await DeleteAsync($"{Address}/{id}").ConfigureAwait(false);
            var dto = await result.Content.ReadFromJsonAsync<PlaceDto>();
            return _mapperFrom.Map(dto);
        }

        public async Task<bool> ToTrash(int id)
        {
            var result = await GetAsync<bool>($"{Address}/totrash/{id}").ConfigureAwait(false);
            return result;
        }

        public async Task<bool> FromTrash(int id)
        {
            var result = await GetAsync<bool>($"{Address}/fromtrash/{id}").ConfigureAwait(false);
            return result;
        }
    }
}
