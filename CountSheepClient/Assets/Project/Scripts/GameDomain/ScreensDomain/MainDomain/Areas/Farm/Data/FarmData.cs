using System.Collections.Generic;
using Newtonsoft.Json;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Data
{
    public class FarmData
    {
        [JsonProperty("area_level")] public int CurrentLevelArea = 0;
        [JsonProperty("collection")] public HashSet<string> Collection = new();
        [JsonProperty("animals")] public Dictionary<int, string> Animals = new();
    }
}