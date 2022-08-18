using ColizeumSDK.API.Responses;
using ColizeumSDK.Models;
using static ColizeumSDK.API.Responses.GetSecondaryCurrencyResponse;

namespace ColizeumSDK.Factories
{
    public static class SecondaryCurrencyFactory
    {
        public static SecondaryCurrency Create(SecondaryCurrencyItem secondaryCurrencyItem)
        {
            return new SecondaryCurrency
            {
                total = secondaryCurrencyItem.total
            };
        }
    }
}