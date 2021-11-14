using TaxHandlingLibrary.Models;

namespace TaxHandlingLibrary.Services
{
    // Could implement ITaxCalculator since the service is just a wrapper around  actual implementation
    // Would want to keep separate though so that if we need a service to do more like give us a valid calculator it could
    public interface ITaxService : ITaxCalculator
    {
    }
}
