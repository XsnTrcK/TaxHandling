using System.Threading.Tasks;
using TaxHandlingLibrary.Models;

namespace TaxHandlingLibrary.Services
{
    public interface ITaxCalculator
    {
        Task<ITaxRate> GetTaxRateForLocationAsync(Location location);

        Task<decimal> GetTotalTaxForOrderAsync(decimal orderPrice, Location location);
    }
}
