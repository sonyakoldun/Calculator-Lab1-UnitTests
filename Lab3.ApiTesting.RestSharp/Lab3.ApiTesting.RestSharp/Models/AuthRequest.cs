using Newtonsoft.Json;

namespace Lab3.ApiTesting.RestSharp.Models;

/// <summary>
/// Модель запиту для аутентифікації
/// </summary>
public class AuthRequest
{
    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;

    [JsonProperty("password")]
    public string Password { get; set; } = string.Empty;
}

