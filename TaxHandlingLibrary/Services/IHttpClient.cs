using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaxHandlingLibrary.Services
{
    public interface IHttpClient
    {
        Task<string> GetAsStringAsync(Uri uri);
        Task<string> GetAsStringAsync(Uri uri, CancellationToken cancellationToken);
    }

    public class InvalidResponseException : Exception
    {
        public InvalidResponseException(string message) : base(message) { }
    }
}
