using Newtonsoft.Json;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Data
{
    public class CurrencyData
    {
        [JsonProperty("amount")] public int Amount;
    }
}