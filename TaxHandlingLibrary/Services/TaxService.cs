using System;
using System.Threading.Tasks;
using TaxHandlingLibrary.Models;

namespace TaxHandlingLibrary.Services
{
    public class TaxService : ITaxService
    {
        private readonly ITaxCalculator _taxCalculator;

        public TaxService(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator ?? throw new ArgumentNullException(nameof(taxCalculator));
        }

        public Task<ITaxRate> GetTaxRateForLocationAsync(Location location) 
            => _taxCalculator.GetTaxRateForLocationAsync(location);

        public Task<decimal> GetTotalTaxForOrderAsync(decimal orderPrice, Location location)
            => _taxCalculator.GetTotalTaxForOrderAsync(orderPrice, location);
    }
}
