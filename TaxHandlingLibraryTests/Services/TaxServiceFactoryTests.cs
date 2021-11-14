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
    [TestFixture]
    public class TaxServiceFactoryTests
    {
        private TaxServiceFactory _taxServiceFactory;

        [SetUp]
        public void SetupTaxServiceDependencies()
        {
            var mockFactory = new Mock<IFactory<ITaxCalculator>>();
            mockFactory.Setup(factory => factory.Build(It.IsAny<string>())).Returns(new TaxJarCalculator(new AuthenticatedHttpClient(""), new Uri("https://test.url.com")));
            _taxServiceFactory = new TaxServiceFactory(mockFactory.Object);
        }

        [Test]
        public void TaxServiceCreated()
        {
            var taxService = _taxServiceFactory.Build("IMC_di");
            Assert.IsTrue(taxService.GetType() == typeof(TaxService));
        }
    }
}
