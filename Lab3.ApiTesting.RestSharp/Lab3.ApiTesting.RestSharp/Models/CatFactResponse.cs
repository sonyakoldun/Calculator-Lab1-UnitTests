using Newtonsoft.Json;

namespace Lab3.ApiTesting.RestSharp.Models;

/// <summary>
/// Модель відповіді з фактом про котів (Cat Facts API)
/// </summary>
public class CatFactResponse
{
    [JsonProperty("fact")]
    public string Fact { get; set; } = string.Empty;

    [JsonProperty("length")]
    public int Length { get; set; }
}

