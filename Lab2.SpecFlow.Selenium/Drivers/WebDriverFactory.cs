using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Lab2.SpecFlow.Selenium.Support;

namespace Lab2.SpecFlow.Selenium.Drivers;

/// <summary>
/// Фабрика для створення WebDriver екземплярів
/// </summary>
public static class WebDriverFactory
{
    /// <summary>
    /// Створює новий екземпляр ChromeDriver з налаштуваннями
    /// </summary>
    public static IWebDriver CreateChromeDriver()
    {
        var options = new ChromeOptions();
        
        if (Config.Headless)
        {
            options.AddArgument("--headless");
        }
        
        // Опції для стабільності на macOS
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--window-size=1920,1080");
        
        // Вимкнення повідомлень про автоматизацію
        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalOption("useAutomationExtension", false);
        
        var driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.TimeoutSeconds);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        driver.Manage().Window.Maximize();
        
        return driver;
    }
}

