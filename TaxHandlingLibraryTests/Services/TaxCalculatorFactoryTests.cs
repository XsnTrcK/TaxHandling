using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxHandlingLibrary.Framework;
using TaxHandlingLibrary.Services;

namespace TaxHandlingLibraryTests.Services
{
    public class TaxCalculatorFactoryTests
    {
        private TaxCalculatorFactory _taxCalculatorFactory;

        [SetUp]
        public void SetupTaxServiceDependencies()
        {
            _taxCalculatorFactory = new TaxCalculatorFactory(new AuthenticatedHttpClient(""), new Uri("https://test.url.com"));
        }

        [Test]
        public void TaxJarCalculatorCreated()
        {
            var taxCalculator = _taxCalculatorFactory.Build("IMC_di");
            Assert.AreEqual(typeof(TaxJarCalculator), taxCalculator.GetType());
        }

        [Test]
        public void ThrowsWhenInvalidKeyGiven()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _taxCalculatorFactory.Build("Alberto"));
        }

        [Test]
        public void ThrowsWhenEmptyKeyGiven()
        {
            Assert.Throws<ArgumentNullException>(() => _taxCalculatorFactory.Build(string.Empty));
            Assert.Throws<ArgumentNullException>(() => _taxCalculatorFactory.Build(null));
            Assert.Throws<ArgumentNullException>(() => _taxCalculatorFactory.Build(" "));
        }
    }
}
