namespace Lab2.SpecFlow.Selenium.Support;

/// <summary>
/// Конфігурація для тестів
/// </summary>
public static class Config
{
    /// <summary>
    /// Базовий URL сайту XYZ Bank
    /// </summary>
    public static string BaseUrl => Environment.GetEnvironmentVariable("BASE_URL") 
        ?? "https://www.globalsqa.com/angularJs-protractor/BankingProject/#/login";

    /// <summary>
    /// Чи запускати браузер у headless режимі
    /// </summary>
    public static bool Headless => bool.Parse(
        Environment.GetEnvironmentVariable("HEADLESS") ?? "false");

    /// <summary>
    /// Таймаут очікування в секундах
    /// </summary>
    public static int TimeoutSeconds => int.Parse(
        Environment.GetEnvironmentVariable("TIMEOUT_SECONDS") ?? "10");
}

