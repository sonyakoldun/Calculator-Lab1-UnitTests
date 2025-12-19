using Newtonsoft.Json;

namespace Lab3.ApiTesting.RestSharp.Models;

/// <summary>
/// Модель запиту для створення бронювання
/// </summary>
public class CreateBookingRequest
{
    [JsonProperty("booking")]
    public Booking Booking { get; set; } = new();
}

