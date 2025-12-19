using Lab3.ApiTesting.RestSharp.Models;

namespace Lab3.ApiTesting.RestSharp.Support;

/// <summary>
/// Контекст для зберігання даних між кроками тесту
/// </summary>
public class TestContext
{
    public int? BookingId { get; set; }
    public string? AuthToken { get; set; }
    public Booking? CreatedBooking { get; set; }
    public Booking? UpdatedBooking { get; set; }
    public string? LastResponseStatus { get; set; }
    public string? LastResponseBody { get; set; }
}

