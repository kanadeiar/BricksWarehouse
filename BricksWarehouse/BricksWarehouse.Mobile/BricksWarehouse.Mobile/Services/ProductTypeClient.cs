using BricksWarehouse.Domain.Dto;
using BricksWarehouse.Domain.Interfaces;
using BricksWarehouse.Domain.Mappers;
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
    public class ProductTypeClient : BaseClient
    {
        private readonly IMapper<ProductType, ProductTypeDto> _mapperTo;
        private readonly IMapper<ProductTypeDto, ProductType> _mapperFrom;
        public ProductTypeClient(HttpClient client, IMapper<ProductType, ProductTypeDto> mapperTo, IMapper<ProductTypeDto, ProductType> mapperFrom) 
            : base(client, "/api/mobileproducttype")
        {
            _mapperTo = mapperTo;
            _mapperFrom = mapperFrom;
        }

        public async Task<IEnumerable<ProductType>> GetAll()
        {
            var dtos = await GetAsync<IEnumerable<ProductTypeDto>>(Address).ConfigureAwait(false);
            return dtos.Select(d => _mapperFrom.Map(d));
        }

        public async Task<IEnumerable<ProductType>> GetTrashed()
        {
            var dtos = await GetAsync<IEnumerable<ProductTypeDto>>($"{Address}/trashed").ConfigureAwait(false);
            return dtos.Select(d => _mapperFrom.Map(d));
        }

        public async Task<ProductType> GetById(int id)
        {
            var dto = await GetAsync<ProductTypeDto>($"{Address}/{id}").ConfigureAwait(false);
            return _mapperFrom.Map(dto);
        }

        public async Task<ProductType> Add(ProductType productType)
        {
            var dto = _mapperTo.Map(productType);
            var result = await PostAsync(Address, dto).ConfigureAwait(false);
            dto = await result.Content.ReadFromJsonAsync<ProductTypeDto>();
            return _mapperFrom.Map(dto);
        }

        public async Task<ProductType> Update(ProductType productType)
        {
            var dto = _mapperTo.Map(productType);
            var result = await PutAsync(Address, dto).ConfigureAwait(false);
            dto = await result.Content.ReadFromJsonAsync<ProductTypeDto>();
            return _mapperFrom.Map(dto);
        }

        public async Task<ProductType> Delete(int id)
        {
            var result = await DeleteAsync($"{Address}/{id}").ConfigureAwait(false);
            var dto = await result.Content.ReadFromJsonAsync<ProductTypeDto>();
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
