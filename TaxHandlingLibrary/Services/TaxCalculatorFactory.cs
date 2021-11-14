using System;
using System.Collections.Generic;
using TaxHandlingLibrary.Framework;

namespace TaxHandlingLibrary.Services
{
    public class TaxCalculatorFactory : IFactory<ITaxCalculator>
    {
        private readonly IHttpClient _httpClient;
        private readonly Dictionary<string, Uri> _baseUris;

        public TaxCalculatorFactory(IHttpClient httpClient, Dictionary<string, Uri> baseUris)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _baseUris = baseUris ?? throw new ArgumentNullException(nameof(baseUris));
        }

        public ITaxCalculator Build(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            switch (key)
            {
                case "IMC_di":
                    return new TaxJarCalculator(_httpClient, _baseUris[key]);
                default:
                    throw new ArgumentOutOfRangeException(nameof(key), $"Do not suppoort customer: {key}");
            }
        }
    }
}
