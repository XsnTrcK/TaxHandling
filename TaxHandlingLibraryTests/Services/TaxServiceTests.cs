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
    public class TaxServiceTests
    {
        private TaxService _taxService;

        [SetUp]
        public void SetupTaxServiceTests()
        {
            var responseString = File.ReadAllText("Resources/usTaxRateResponse.json");
            var usTaxRate = JsonConvert.DeserializeObject<Dictionary<string, UsTaxRates>>(responseString)["rate"];

            var mockTaxCalculator = new Mock<ITaxCalculator>();
            mockTaxCalculator.Setup(calculator => calculator.GetTaxRateForLocationAsync(It.IsAny<Location>())).ReturnsAsync(usTaxRate);
            mockTaxCalculator.Setup(calculator => calculator.GetTotalTaxForOrderAsync(It.IsAny<decimal>(), It.IsAny<Location>())).ReturnsAsync((decimal)1.8);

            _taxService = new TaxService(mockTaxCalculator.Object);
        }

        [Test]
        public async Task AbleToGetTaxRateForLocation()
        {
            var givenRate = await _taxService.GetTaxRateForLocationAsync(new Location("90404"));
            Assert.AreEqual((decimal)0.0975, givenRate.GetRate(RateType.CombinedRate));
        }

        [Test]
        public async Task AbleToCalculateTaxesForAnOrder()
        {
            var totalTax = await _taxService.GetTotalTaxForOrderAsync((decimal)18.5, new Location("90404"));
            Assert.AreEqual((decimal)1.8, totalTax);
        }
    }
}
