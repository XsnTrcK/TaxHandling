using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaxHandlingLibrary.Models;

namespace TaxHandlingLibrary.Services
{
    public class TaxJarCalculator : ITaxCalculator
    {
        private readonly IHttpClient _httpClient;
        private readonly Uri _taxJarBaseUri;

        public TaxJarCalculator(IHttpClient httpClient, Uri taxJarBaseUri)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _taxJarBaseUri = taxJarBaseUri ?? throw new ArgumentNullException(nameof(taxJarBaseUri));
        }

        public async Task<ITaxRate> GetTaxRateForLocationAsync(Location location)
        {
            if (!string.IsNullOrWhiteSpace(location.Country) && !location.Country.Equals("us", StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentOutOfRangeException(nameof(location.Country), "Do not currently support tax rates outside of US");

            var taxJarGetUri = new UriBuilder(_taxJarBaseUri)
            {
                Query = location.GenerateUriQuery()
            };
            if (taxJarGetUri.Path.EndsWith('/'))
                taxJarGetUri.Path += "rates";
            else
                taxJarGetUri.Path += "/rates";
            var ratesString = await _httpClient.GetAsStringAsync(taxJarGetUri.Uri).ConfigureAwait(false);
            // Need to add a way to deserialize other tax rates once supported
            var returnWrapper = JsonConvert.DeserializeObject<Dictionary<string, UsTaxRates>>(ratesString);
            if (returnWrapper.Count > 1)
                throw new InvalidResponseException($"Invalid response recieved:\r\n{JsonConvert.SerializeObject(returnWrapper)}");
            return returnWrapper["rate"];
        }

        public async Task<decimal> GetTotalTaxForOrderAsync(decimal orderTotal, Location location)
        {
            var taxRate = await GetTaxRateForLocationAsync(location).ConfigureAwait(false);
            var combinedRate = taxRate.GetRate(RateType.CombinedRate);
            if (combinedRate is null)
                throw new InvalidResponseException("No tax rate was received");

            return orderTotal * combinedRate.Value;
        }
    }
}
