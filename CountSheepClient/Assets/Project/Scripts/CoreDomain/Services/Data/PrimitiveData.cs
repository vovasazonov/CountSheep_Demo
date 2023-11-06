using Newtonsoft.Json;

namespace Project.CoreDomain.Services.Data
{
    public class PrimitiveData<T>
    {
        [JsonProperty("value")] public T Value;
    }
}