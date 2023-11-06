using System.Collections.Generic;
using Newtonsoft.Json;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Shop.Data
{
    public class ShopSheepData
    {
        [JsonProperty("items")] public Dictionary<string, int> AmountPurchaseBySheepItem = new();
    }
}