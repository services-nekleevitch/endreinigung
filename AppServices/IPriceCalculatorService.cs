using AppServices.Models;

namespace AppServices
{
    public interface IPriceCalculatorService
    {
        decimal CalculateTotal(PropertyConfigurationModel config, int? priceSetId = null);
    
        //Task<decimal> CalculateTotalAsync(PropertyConfigurationViewModel config, int priceSetId);
    }
}