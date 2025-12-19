using BoDi;
using FluentAssertions;
using TechTalk.SpecFlow;
using Lab3.ApiTesting.RestSharp.Clients;
using Lab3.ApiTesting.RestSharp.Models;
using Lab3.ApiTesting.RestSharp.Support;
using RestSharp;

namespace Lab3.ApiTesting.RestSharp.StepDefinitions;

[Binding]
public class CatFactsSteps
{
    private readonly CatFactsClient _client;
    private readonly TestContext _context;
    private readonly IObjectContainer _objectContainer;
    private CatFactResponse? _lastFactResponse;

    public CatFactsSteps(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
        _client = _objectContainer.Resolve<CatFactsClient>();
        _context = _objectContainer.Resolve<TestContext>();
    }

    [Given(@"базова API доступна")]
    [Scope(Feature = "Тестування Cat Facts API")]
    public void GivenBaseApiIsAvailable()
    {
        // Перевіряємо доступність API через простий запит
        var response = _client.GetRandomFact();
        response.Should().NotBeNull();
    }

    [When(@"я запитую випадковий факт про котів")]
    public void WhenIRequestRandomCatFact()
    {
        var response = _client.GetRandomFact();
        _context.LastResponseStatus = ((int)response.StatusCode).ToString();
        _context.LastResponseBody = response.Content;

        if (response.IsSuccessful && response.Data != null)
        {
            _lastFactResponse = response.Data;
        }
    }

    [Then(@"відповідь повинна містити факт")]
    public void ThenResponseShouldContainFact()
    {
        _lastFactResponse.Should().NotBeNull();
        _lastFactResponse!.Fact.Should().NotBeNullOrEmpty();
    }

    [Then(@"факт повинен мати довжину")]
    public void ThenFactShouldHaveLength()
    {
        _lastFactResponse.Should().NotBeNull();
        _lastFactResponse!.Length.Should().BeGreaterThan(0);
    }

    [When(@"я запитую неіснуючий endpoint")]
    public void WhenIRequestNonExistentEndpoint()
    {
        var request = new RestRequest("/nonexistent", Method.Get);
        request.AddHeader("Accept", "application/json");

        var client = new RestClient("https://catfact.ninja");
        var response = client.Execute<object>(request);
        
        _context.LastResponseStatus = ((int)response.StatusCode).ToString();
    }

    [Then(@"статус відповіді повинен бути (\d+)")]
    [Scope(Feature = "Тестування Cat Facts API")]
    public void ThenResponseStatusShouldBe(int expectedStatus)
    {
        var statusCode = int.Parse(_context.LastResponseStatus ?? "0");
        statusCode.Should().Be(expectedStatus);
    }
}

