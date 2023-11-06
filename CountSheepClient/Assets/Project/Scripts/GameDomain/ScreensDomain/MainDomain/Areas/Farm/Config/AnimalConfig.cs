using System.Collections.Generic;
using Newtonsoft.Json;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Config
{
    public class AnimalConfig
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("parents")] public List<string> Parents { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("coins_per_click")] public int CoinsPerClick { get; set; }
    }
}