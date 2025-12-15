using OpenQA.Selenium;
using Lab2.SpecFlow.Selenium.Support;

namespace Lab2.SpecFlow.Selenium.Pages;

/// <summary>
/// Page Object для головної сторінки XYZ Bank
/// </summary>
public class HomePage
{
    private readonly IWebDriver _driver;
    
    // Локатори
    private readonly By _bankManagerLoginButton = By.XPath("//button[contains(text(), 'Bank Manager Login')]");
    
    public HomePage(IWebDriver driver)
    {
        _driver = driver;
    }
    
    /// <summary>
    /// Відкриває головну сторінку банку
    /// </summary>
    public void Open(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }
    
    /// <summary>
    /// Натискає кнопку Bank Manager Login
    /// </summary>
    public void ClickBankManagerLogin()
    {
        var button = WaitHelper.WaitForElementClickable(_driver, _bankManagerLoginButton);
        button.Click();
    }
}

