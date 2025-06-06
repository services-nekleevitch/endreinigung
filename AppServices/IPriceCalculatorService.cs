using EndreinigungZurich.Models.Booking;

namespace AppServices
{
    public interface IPriceCalculatorService
    {
        Task<decimal> CalculateTotalAsync(PropertyConfigurationViewModel config, int priceSetId);
    }
}