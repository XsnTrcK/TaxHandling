using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TaxHandlingLibrary.Models;
using TaxHandlingLibrary.Services;

namespace TaxHandlingLibraryTests
{
    [TestFixture]
    public class TaxJarCalculatorTests
    {
        private static readonly Uri _taxJarUri = new Uri("https://api.taxjar.com/v2/");
        private readonly IHttpClient _httpClient;

        public TaxJarCalculatorTests()
        {
            var responseString = File.ReadAllText("Resources/usTaxRateResponse.json");

            var invalidResponseWrapper = JsonConvert.DeserializeObject<Dictionary<string, UsTaxRates>>(responseString);
            invalidResponseWrapper["rate"].CombinedRate = null;            
            var invalidRateDataString = JsonConvert.SerializeObject(invalidResponseWrapper);

            invalidResponseWrapper.Add("rateTwo", invalidResponseWrapper["rate"]);
            var invalidResponseString = JsonConvert.SerializeObject(invalidResponseWrapper);

            var mockHttpClient = new Mock<IHttpClient>();
            mockHttpClient.Setup(client => client.GetAsStringAsync(new Uri("https://api.taxjar.com/v2/rates?zip=90404"))).ReturnsAsync(responseString);
            mockHttpClient.Setup(client => client.GetAsStringAsync(new Uri("https://api.taxjar.com/v2/rates?zip=60188"))).ReturnsAsync(invalidResponseString);
            mockHttpClient.Setup(client => client.GetAsStringAsync(new Uri("https://api.taxjar.com/v2/rates?zip=60183"))).ReturnsAsync(invalidRateDataString);
            _httpClient = mockHttpClient.Object;
        }

        [Test]
        public async Task AbleToGetTaxRateForUsLocation()
        {
            var taxJarCalculator = new TaxJarCalculator(_httpClient, _taxJarUri);
            var rate = await taxJarCalculator.GetTaxRateForLocationAsync(new Location("90404"));
            Assert.IsNotNull(rate);
            Assert.AreEqual(decimal.Parse("0.0975"), rate?.GetRate(RateType.CombinedRate));
        }

        [Test]
        public void ThrowsWhenGettingRateOutsideUs()
        {
            var taxJarCalculator = new TaxJarCalculator(_httpClient, _taxJarUri);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => taxJarCalculator.GetTaxRateForLocationAsync(new Location("00000") { Country = "CA" }));
        }

        [Test]
        public void ThrowsWhenResponseIsInvalid()
        {
            var taxJarCalculator = new TaxJarCalculator(_httpClient, _taxJarUri);
            Assert.ThrowsAsync<InvalidResponseException>(() => taxJarCalculator.GetTaxRateForLocationAsync(new Location("60188")));
        }

        [Test]
        public async Task AbleToCalculateTaxesForAnOrder()
        {
            var orderTotal = (decimal)18.5;
            var combinedTaxRate = (decimal)0.0975;
            var taxJarCalculator = new TaxJarCalculator(_httpClient, _taxJarUri);
            var totalTaxForOrder = await taxJarCalculator.GetTotalTaxForOrderAsync(orderTotal, new Location("90404"));

            Assert.AreEqual(orderTotal * combinedTaxRate, totalTaxForOrder);
        }

        [Test]
        public void ThrowsWhenInvalidDataReturned()
        {
            var taxJarCalculator = new TaxJarCalculator(_httpClient, _taxJarUri);
            Assert.ThrowsAsync<InvalidResponseException>(() => taxJarCalculator.GetTotalTaxForOrderAsync((decimal)18.5, new Location("60183")));
        }
    }
}
