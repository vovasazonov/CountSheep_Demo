using Newtonsoft.Json;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Config
{
    public class AreaConfig
    {
        [JsonProperty("level")] public int Level;
        [JsonProperty("cost")] public int Cost;
        [JsonProperty("amount_animal")] public int AmountAnimal;
    }
}