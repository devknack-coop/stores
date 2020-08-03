using DevKnack.Stores;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Common.Stores.HttpClient
{
    public class HttpSearchRepositoryQuery : ISearchRepositoryQuery
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _log;

        public HttpSearchRepositoryQuery(IHttpClientFactory clientFactory, ILoggerFactory logFactory)
        {
            _clientFactory = clientFactory;
            _log = logFactory.CreateLogger<HttpFileStore>();
        }

        public async Task<IEnumerable<string>> SearchAsync(string url, string extension)
        {
            var client = _clientFactory.CreateClient("default");

            return await client.GetFromJsonAsync<IEnumerable<string>>($"api/store/v1/search?url={url}&extension={extension}");
        }
    }
}