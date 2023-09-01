using Newtonsoft.Json;

namespace BackendLib.Json;

/// <summary>
/// Default implementation of IJsonService, using Newtonsoft.Json library
/// </summary>
public class NewtonsoftJsonService : IJsonService
{
    public TValue Deserialize<TValue>(string json)
    {
        return JsonConvert.DeserializeObject<TValue>(json) ?? throw new Exception("json parse failed");
    }

    public string Serialize<TValue>(TValue value, params JsonConverter[] converters)
    {
        return JsonConvert.SerializeObject(value, converters);
    }
}