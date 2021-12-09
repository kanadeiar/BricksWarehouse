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

namespace BricksWarehouse.Mobile.Services
{
    public class TaskClient : BaseClient
    {
        private readonly IMapper<OutTask, OutTaskDto> _mapperTo;
        private readonly IMapper<OutTaskDto, OutTask> _mapperFrom;
        public TaskClient(HttpClient client, IMapper<OutTask, OutTaskDto> mapperTo, IMapper<OutTaskDto, OutTask> mapperFrom) 
            : base(client, "/api/mobiletask")
        {
            _mapperTo = mapperTo;
            _mapperFrom = mapperFrom;
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


    }
}
