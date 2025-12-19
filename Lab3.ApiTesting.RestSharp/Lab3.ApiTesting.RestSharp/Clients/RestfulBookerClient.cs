using RestSharp;
using Lab3.ApiTesting.RestSharp.Models;
using Newtonsoft.Json;

namespace Lab3.ApiTesting.RestSharp.Clients;

/// <summary>
/// Клієнт для роботи з RestfulBooker API
/// </summary>
public class RestfulBookerClient
{
    private readonly RestClient _client;
    private readonly string _baseUrl;

    public RestfulBookerClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _client = new RestClient(baseUrl);
    }

    /// <summary>
    /// Створює нове бронювання
    /// </summary>
    public RestResponse<CreateBookingResponse> CreateBooking(Booking booking)
    {
        var request = new RestRequest("/booking", Method.Post);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");

        request.AddJsonBody(booking);

        var response = _client.Execute<CreateBookingResponse>(request);
        return response;
    }

    /// <summary>
    /// Отримує бронювання за ID
    /// </summary>
    public RestResponse<Booking> GetBooking(int bookingId)
    {
        var request = new RestRequest($"/booking/{bookingId}", Method.Get);
        request.AddHeader("Accept", "application/json");

        var response = _client.Execute<Booking>(request);
        return response;
    }

    /// <summary>
    /// Повністю оновлює бронювання (PUT)
    /// </summary>
    public RestResponse<Booking> UpdateBooking(int bookingId, string token, Booking booking)
    {
        var request = new RestRequest($"/booking/{bookingId}", Method.Put);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Cookie", $"token={token}");

        request.AddJsonBody(booking);

        var response = _client.Execute<Booking>(request);
        return response;
    }

    /// <summary>
    /// Частково оновлює бронювання (PATCH)
    /// </summary>
    public RestResponse<Booking> PartialUpdateBooking(int bookingId, string token, object partialData)
    {
        var request = new RestRequest($"/booking/{bookingId}", Method.Patch);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Cookie", $"token={token}");

        request.AddJsonBody(partialData);

        var response = _client.Execute<Booking>(request);
        return response;
    }

    /// <summary>
    /// Видаляє бронювання
    /// </summary>
    public RestResponse DeleteBooking(int bookingId, string token)
    {
        var request = new RestRequest($"/booking/{bookingId}", Method.Delete);
        request.AddHeader("Cookie", $"token={token}");

        var response = _client.Execute<object>(request);
        return response;
    }

    /// <summary>
    /// Отримує токен аутентифікації
    /// </summary>
    public RestResponse<AuthResponse> CreateToken(string username, string password)
    {
        var request = new RestRequest("/auth", Method.Post);
        request.AddHeader("Content-Type", "application/json");

        var authRequest = new AuthRequest
        {
            Username = username,
            Password = password
        };
        request.AddJsonBody(authRequest);

        var response = _client.Execute<AuthResponse>(request);
        return response;
    }
}

