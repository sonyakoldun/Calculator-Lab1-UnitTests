using Newtonsoft.Json;

namespace Lab3.ApiTesting.RestSharp.Models;

/// <summary>
/// Модель відповіді на аутентифікацію
/// </summary>
public class AuthResponse
{
    [JsonProperty("token")]
    public string Token { get; set; } = string.Empty;
}

