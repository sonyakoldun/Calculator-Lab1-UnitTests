using Newtonsoft.Json;

namespace Lab3.ApiTesting.RestSharp.Models;

/// <summary>
/// Модель для дат бронювання (checkin/checkout)
/// </summary>
public class BookingDates
{
    [JsonProperty("checkin")]
    public string Checkin { get; set; } = string.Empty;

    [JsonProperty("checkout")]
    public string Checkout { get; set; } = string.Empty;
}

