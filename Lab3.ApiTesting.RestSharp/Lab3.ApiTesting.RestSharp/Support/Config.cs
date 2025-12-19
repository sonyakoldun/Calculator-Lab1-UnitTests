using Microsoft.Extensions.Configuration;

namespace Lab3.ApiTesting.RestSharp.Support;

/// <summary>
/// Конфігурація для тестів
/// </summary>
public static class Config
{
    private static readonly IConfigurationRoot Configuration;

    static Config()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();
    }

    /// <summary>
    /// Базовий URL для RestfulBooker API
    /// </summary>
    public static string RestfulBookerBaseUrl => 
        Environment.GetEnvironmentVariable("RESTFUL_BOOKER_BASE_URL") 
        ?? Configuration["RestfulBooker:BaseUrl"] 
        ?? "https://restful-booker.herokuapp.com";

    /// <summary>
    /// Базовий URL для Cat Facts API
    /// </summary>
    public static string CatFactsBaseUrl => 
        Environment.GetEnvironmentVariable("CAT_FACTS_BASE_URL") 
        ?? Configuration["CatFacts:BaseUrl"] 
        ?? "https://catfact.ninja";

    /// <summary>
    /// Username для аутентифікації в RestfulBooker
    /// </summary>
    public static string RestfulBookerUsername => 
        Environment.GetEnvironmentVariable("RESTFUL_BOOKER_USERNAME") 
        ?? Configuration["RestfulBooker:Username"] 
        ?? "admin";

    /// <summary>
    /// Password для аутентифікації в RestfulBooker
    /// </summary>
    public static string RestfulBookerPassword => 
        Environment.GetEnvironmentVariable("RESTFUL_BOOKER_PASSWORD") 
        ?? Configuration["RestfulBooker:Password"] 
        ?? "password123";
}

