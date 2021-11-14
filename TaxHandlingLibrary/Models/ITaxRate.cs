namespace TaxHandlingLibrary.Models
{
    public interface ITaxRate
    {
        decimal? GetRate(RateType rateType);
    }
}
