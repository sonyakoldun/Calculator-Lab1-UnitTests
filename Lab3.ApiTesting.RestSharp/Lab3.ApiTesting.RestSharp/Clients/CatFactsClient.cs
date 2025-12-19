using RestSharp;
using Lab3.ApiTesting.RestSharp.Models;

namespace Lab3.ApiTesting.RestSharp.Clients;

/// <summary>
/// Клієнт для роботи з Cat Facts API
/// </summary>
public class CatFactsClient
{
    private readonly RestClient _client;
    private readonly string _baseUrl;

    public CatFactsClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _client = new RestClient(baseUrl);
    }

    /// <summary>
    /// Отримує випадковий факт про котів
    /// </summary>
    public RestResponse<CatFactResponse> GetRandomFact()
    {
        var request = new RestRequest("/fact", Method.Get);
        request.AddHeader("Accept", "application/json");

        var response = _client.Execute<CatFactResponse>(request);
        return response;
    }

    /// <summary>
    /// Отримує список фактів про котів з параметрами
    /// </summary>
    public RestResponse<string> GetFacts(int limit = 10, int maxLength = 140)
    {
        var request = new RestRequest("/facts", Method.Get);
        request.AddHeader("Accept", "application/json");
        request.AddParameter("limit", limit);
        request.AddParameter("max_length", maxLength);

        var response = _client.Execute<string>(request);
        return response;
    }
}

