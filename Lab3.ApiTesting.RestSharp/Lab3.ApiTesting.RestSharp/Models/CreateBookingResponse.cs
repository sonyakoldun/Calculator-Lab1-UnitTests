using Newtonsoft.Json;

namespace Lab3.ApiTesting.RestSharp.Models;

/// <summary>
/// Модель відповіді на створення бронювання
/// RestfulBooker API повертає об'єкт з полями bookingid та booking
/// </summary>
public class CreateBookingResponse
{
    [JsonProperty("bookingid")]
    public int Bookingid { get; set; }

    [JsonProperty("booking")]
    public Booking? Booking { get; set; }
}

