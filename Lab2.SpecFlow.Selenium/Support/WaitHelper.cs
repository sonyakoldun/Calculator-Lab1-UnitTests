using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Lab2.SpecFlow.Selenium.Support;

/// <summary>
/// Допоміжний клас для роботи з очікуваннями
/// </summary>
public static class WaitHelper
{
    /// <summary>
    /// Створює WebDriverWait з налаштованим таймаутом
    /// </summary>
    public static WebDriverWait CreateWait(IWebDriver driver, int timeoutSeconds = 10)
    {
        return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
    }

    /// <summary>
    /// Очікує, поки елемент стане видимим
    /// </summary>
    public static IWebElement WaitForElementVisible(IWebDriver driver, By locator, int timeoutSeconds = 10)
    {
        var wait = CreateWait(driver, timeoutSeconds);
        return wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    /// <summary>
    /// Очікує, поки елемент стане клікабельним
    /// </summary>
    public static IWebElement WaitForElementClickable(IWebDriver driver, By locator, int timeoutSeconds = 10)
    {
        var wait = CreateWait(driver, timeoutSeconds);
        return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    /// <summary>
    /// Очікує, поки елемент зникне з DOM
    /// </summary>
    public static bool WaitForElementToDisappear(IWebDriver driver, By locator, int timeoutSeconds = 10)
    {
        var wait = CreateWait(driver, timeoutSeconds);
        try
        {
            return wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
        }
        catch
        {
            return false;
        }
    }
}

