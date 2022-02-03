using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BricksWarehouse.Mobile.Services.Base
{
    public abstract class BaseClient
    {
        protected readonly HttpClient Client;
        protected readonly string Address;
        public BaseClient(HttpClient client, string address)
        {
            Client = client;
            Address = address;
        }

        /// <summary> Асинхронное получение данных с веб апи сервера </summary>
        /// <typeparam name="T">тип данных</typeparam>
        /// <param name="url">конечная точка</param>
        /// <param name="cancel">токен отмены</param>
        /// <returns>данные</returns>
        protected async Task<T> GetAsync<T>(string url, CancellationToken cancel = default)
        {
            var response = await Client
                .GetAsync(url, cancel).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NoContent)
                return default;
            return await response
                .EnsureSuccessStatusCode()
                .Content.ReadFromJsonAsync<T>(cancellationToken: cancel);
        }
        /// <summary> Асинхронное добавление данных в веб апи сервер </summary>
        /// <typeparam name="T">тип данных</typeparam>
        /// <param name="url">конечная точка</param>
        /// <param name="item">данные</param>
        /// <param name="cancel">токен отмены</param>
        /// <returns>статус добавления</returns>
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken cancel = default)
        {
            var response = await Client
                .PostAsJsonAsync(url, item, cancel).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        /// <summary> Асинхронное обновление данных в веб апи сервере </summary>
        /// <typeparam name="T">тип данных</typeparam>
        /// <param name="url">конечная точка</param>
        /// <param name="item">данные</param>
        /// <param name="cancel">токен отмены</param>
        /// <returns>результат обновления</returns>
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken cancel = default)
        {
            var response = await Client
                .PutAsJsonAsync(url, item, cancel).ConfigureAwait(false);
            return response.EnsureSuccessStatusCode();
        }
        /// <summary> Асинхронное удаление данных из веб апи сервера </summary>
        /// <param name="url">конечная точка</param>
        /// <param name="cancel">токен отмены</param>
        /// <returns>результат обновления</returns>
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancel = default)
        {
            var response = await Client
                .DeleteAsync(url, cancel).ConfigureAwait(false);
            return response;
        }
    }
}
