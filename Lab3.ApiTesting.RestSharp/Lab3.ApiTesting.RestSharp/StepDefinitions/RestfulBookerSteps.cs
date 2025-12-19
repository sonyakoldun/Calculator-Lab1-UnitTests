using BoDi;
using FluentAssertions;
using TechTalk.SpecFlow;
using Lab3.ApiTesting.RestSharp.Clients;
using Lab3.ApiTesting.RestSharp.Models;
using Lab3.ApiTesting.RestSharp.Support;

namespace Lab3.ApiTesting.RestSharp.StepDefinitions;

[Binding]
public class RestfulBookerSteps
{
    private readonly RestfulBookerClient _client;
    private readonly TestContext _context;
    private readonly IObjectContainer _objectContainer;

    public RestfulBookerSteps(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
        _client = _objectContainer.Resolve<RestfulBookerClient>();
        _context = _objectContainer.Resolve<TestContext>();
    }

    [Given(@"базова API доступна")]
    [Scope(Feature = "Тестування CRUD операцій RestfulBooker API")]
    public void GivenBaseApiIsAvailable()
    {
        // Перевіряємо доступність API через простий запит
        var response = _client.GetBooking(1);
        // Не важливо чи 200 чи 404 - головне що API відповідає
        response.Should().NotBeNull();
    }

    [When(@"я створюю бронювання з валідними даними")]
    public void WhenICreateBookingWithValidData()
    {
        var booking = new Booking
        {
            Firstname = "John",
            Lastname = "Doe",
            Totalprice = 150,
            Depositpaid = true,
            Bookingdates = new BookingDates
            {
                Checkin = "2024-01-01",
                Checkout = "2024-01-05"
            },
            Additionalneeds = "Breakfast"
        };

        var response = _client.CreateBooking(booking);
        _context.LastResponseStatus = ((int)response.StatusCode).ToString();
        _context.LastResponseBody = response.Content;

        if (response.IsSuccessful && response.Data != null)
        {
            _context.BookingId = response.Data.Bookingid;
            _context.CreatedBooking = booking;
        }
    }

    [Then(@"статус відповіді повинен бути (\d+)")]
    [Then(@"статус повинен бути (\d+)")]
    public void ThenResponseStatusShouldBe(int expectedStatus)
    {
        var statusCode = int.Parse(_context.LastResponseStatus ?? "0");
        statusCode.Should().Be(expectedStatus);
    }

    [Then(@"booking id повинен бути повернутий")]
    public void ThenBookingIdShouldBeReturned()
    {
        _context.BookingId.Should().NotBeNull();
        _context.BookingId.Should().BeGreaterThan(0);
    }

    [When(@"я отримую створене бронювання за id")]
    public void WhenIGetCreatedBookingById()
    {
        if (_context.BookingId.HasValue)
        {
        var response = _client.GetBooking(_context.BookingId.Value);
        _context.LastResponseStatus = ((int)response.StatusCode).ToString();
            _context.LastResponseBody = response.Content;

            if (response.IsSuccessful && response.Data != null)
            {
                _context.CreatedBooking = response.Data;
            }
        }
    }

    [Then(@"деталі бронювання повинні відповідати створеним даним")]
    public void ThenBookingDetailsShouldMatchCreatedData()
    {
        _context.CreatedBooking.Should().NotBeNull();
        _context.CreatedBooking!.Firstname.Should().Be("John");
        _context.CreatedBooking.Lastname.Should().Be("Doe");
        _context.CreatedBooking.Totalprice.Should().Be(150);
        _context.CreatedBooking.Depositpaid.Should().BeTrue();
    }

    [Given(@"у мене є auth token")]
    public void GivenIHaveAuthToken()
    {
        var response = _client.CreateToken(Config.RestfulBookerUsername, Config.RestfulBookerPassword);
        
        response.IsSuccessful.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Token.Should().NotBeNullOrEmpty();
        
        _context.AuthToken = response.Data.Token;
    }

    [Given(@"у мене є існуюче бронювання")]
    public void GivenIHaveExistingBooking()
    {
        // Створюємо нове бронювання
        var booking = new Booking
        {
            Firstname = "Jane",
            Lastname = "Smith",
            Totalprice = 200,
            Depositpaid = false,
            Bookingdates = new BookingDates
            {
                Checkin = "2024-02-01",
                Checkout = "2024-02-05"
            },
            Additionalneeds = "WiFi"
        };

        var response = _client.CreateBooking(booking);
        response.IsSuccessful.Should().BeTrue();
        
        if (response.Data != null)
        {
            _context.BookingId = response.Data.Bookingid;
            _context.CreatedBooking = booking;
        }
    }

    [When(@"я оновлюю бронювання з новим firstname та lastname")]
    public void WhenIUpdateBookingWithNewFirstnameAndLastname()
    {
        if (_context.BookingId.HasValue && _context.AuthToken != null)
        {
            var updatedBooking = new Booking
            {
                Firstname = "UpdatedFirstName",
                Lastname = "UpdatedLastName",
                Totalprice = _context.CreatedBooking!.Totalprice,
                Depositpaid = _context.CreatedBooking.Depositpaid,
                Bookingdates = _context.CreatedBooking.Bookingdates,
                Additionalneeds = _context.CreatedBooking.Additionalneeds
            };

            var response = _client.UpdateBooking(_context.BookingId.Value, _context.AuthToken, updatedBooking);
            _context.LastResponseStatus = ((int)response.StatusCode).ToString();
            
            if (response.IsSuccessful && response.Data != null)
            {
                _context.UpdatedBooking = response.Data;
            }
        }
    }

    [Then(@"відповідь повинна містити оновлені значення")]
    public void ThenResponseShouldContainUpdatedValues()
    {
        _context.UpdatedBooking.Should().NotBeNull();
        _context.UpdatedBooking!.Firstname.Should().Be("UpdatedFirstName");
        _context.UpdatedBooking.Lastname.Should().Be("UpdatedLastName");
    }

    [Then(@"GET запит повинен повернути оновлені значення")]
    public void ThenGetRequestShouldReturnUpdatedValues()
    {
        if (_context.BookingId.HasValue)
        {
            var response = _client.GetBooking(_context.BookingId.Value);
            response.IsSuccessful.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Data!.Firstname.Should().Be("UpdatedFirstName");
            response.Data.Lastname.Should().Be("UpdatedLastName");
        }
    }

    [When(@"я видаляю бронювання")]
    public void WhenIDeleteBooking()
    {
        if (_context.BookingId.HasValue && _context.AuthToken != null)
        {
            var response = _client.DeleteBooking(_context.BookingId.Value, _context.AuthToken);
            _context.LastResponseStatus = ((int)response.StatusCode).ToString();
        }
    }

    [Then(@"наступний GET повинен повернути (\d+)")]
    public void ThenSubsequentGetShouldReturn(int expectedStatus)
    {
        if (_context.BookingId.HasValue)
        {
            var response = _client.GetBooking(_context.BookingId.Value);
            ((int)response.StatusCode).Should().Be(expectedStatus);
        }
    }

    [When(@"я запитую бронювання з неіснуючим id")]
    public void WhenIRequestBookingWithNonExistentId()
    {
        var response = _client.GetBooking(999999);
        _context.LastResponseStatus = ((int)response.StatusCode).ToString();
    }
}

