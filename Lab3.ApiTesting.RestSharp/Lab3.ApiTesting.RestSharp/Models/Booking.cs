using Newtonsoft.Json;

namespace Lab3.ApiTesting.RestSharp.Models;

/// <summary>
/// Модель бронювання
/// </summary>
public class Booking
{
    [JsonProperty("firstname")]
    public string Firstname { get; set; } = string.Empty;

    [JsonProperty("lastname")]
    public string Lastname { get; set; } = string.Empty;

    [JsonProperty("totalprice")]
    public int Totalprice { get; set; }

    [JsonProperty("depositpaid")]
    public bool Depositpaid { get; set; }

    [JsonProperty("bookingdates")]
    public BookingDates Bookingdates { get; set; } = new();

    [JsonProperty("additionalneeds")]
    public string Additionalneeds { get; set; } = string.Empty;
}

