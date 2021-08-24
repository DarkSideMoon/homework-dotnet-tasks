using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReactGaApp
{
    public interface IGaHttpClient
    {
        Task<Dictionary<int, string>> CollectDataAsync(FormUrlEncodedContent data);

        void InsertDataAsync(int id, string data);
    }

    public class GaHttpClient : IGaHttpClient
    {
        private HttpClient _httpClient;
        private static Dictionary<int, string> items = new();

        public GaHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<int, string>> CollectDataAsync(FormUrlEncodedContent data)
        {
            var response = await _httpClient.PostAsync("collect", data);

            response.EnsureSuccessStatusCode();

            return items;
        }

        public void InsertDataAsync(int id, string data)
        {
            items.Add(id, data);
        }
    }
}
