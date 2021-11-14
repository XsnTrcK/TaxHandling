using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TaxHandlingLibrary.Services
{
    public class AuthenticatedHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;
        public AuthenticatedHttpClient(string token)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public Task<string> GetAsStringAsync(Uri uri) => GetAsStringAsync(uri, CancellationToken.None);

        public Task<string> GetAsStringAsync(Uri uri, CancellationToken cancellationToken)
            => _httpClient.GetStringAsync(uri, cancellationToken);
    }
}
