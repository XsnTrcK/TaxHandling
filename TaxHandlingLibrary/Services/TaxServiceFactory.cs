using System;
using TaxHandlingLibrary.Framework;

namespace TaxHandlingLibrary.Services
{
    public class TaxServiceFactory : IFactory<ITaxService>
    {
        private readonly IFactory<ITaxCalculator> _taxCalculatorFactory;

        public TaxServiceFactory(IFactory<ITaxCalculator> taxCalculatorFactory)
        {
            _taxCalculatorFactory = taxCalculatorFactory ?? throw new ArgumentNullException(nameof(taxCalculatorFactory));
        }

        public ITaxService Build(string key) => new TaxService(_taxCalculatorFactory.Build(key));
    }
}
