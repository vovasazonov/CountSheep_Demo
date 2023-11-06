using System.Collections.Generic;
using Newtonsoft.Json;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Config
{
    public class FarmConfig
    {
        [JsonProperty("areas")] public List<AreaConfig> Areas { get; set; }
        [JsonProperty("animals")] public List<AnimalConfig> Animals { get; set; }
    }
}