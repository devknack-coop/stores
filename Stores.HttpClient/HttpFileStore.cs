using DevKnack.Stores;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Common.Stores.HttpClient
{
    /// <summary>
    /// Storage over our stores API
    /// </summary>
    public class HttpFileStore : IFileStore
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _log;

        public HttpFileStore(IHttpClientFactory clientFactory, ILoggerFactory logFactory)
        {
            _clientFactory = clientFactory;
            _log = logFactory.CreateLogger<HttpFileStore>();
        }

        public async Task<string?> ReadStringFileAsync(string url, string path)
        {
            var client = _clientFactory.CreateClient("default");

            string encodedUrl = StoreUrlEncoder.Encode(url);
            string encodedPath = StoreUrlEncoder.Encode(path);

            return await client.GetStringAsync($"api/store/v1?url={encodedUrl}&path={encodedPath}");
        }

        public async Task WriteStringFileAsync(string url, string path, string contents)
        {
            var client = _clientFactory.CreateClient("default");

            var command = new
            {
                Url = StoreUrlEncoder.Encode(url),
                Path = StoreUrlEncoder.Encode(path),
                Contents = contents
            };

            await client.PutAsJsonAsync("api/store/v1", command);
        }
    }
}