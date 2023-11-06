using Newtonsoft.Json;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Data
{
    public class ScoreData
    {
        [JsonProperty("Max")] public int MaxScore;
    }
}