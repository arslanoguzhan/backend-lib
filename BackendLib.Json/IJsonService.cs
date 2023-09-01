using Newtonsoft.Json;

namespace BackendLib.Json;

/// <summary>
/// Mockable json-serialization service definition
/// </summary>
public interface IJsonService
{
    string Serialize<TValue>(TValue value, params JsonConverter[] converters);

    TValue Deserialize<TValue>(string json);
}