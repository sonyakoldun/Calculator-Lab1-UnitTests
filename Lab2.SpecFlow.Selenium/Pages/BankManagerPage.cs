using OpenQA.Selenium;
using Lab2.SpecFlow.Selenium.Support;

namespace Lab2.SpecFlow.Selenium.Pages;

/// <summary>
/// Page Object для сторінки Bank Manager Dashboard
/// </summary>
public class BankManagerPage
{
    private readonly IWebDriver _driver;
    
    // Локатори
    private readonly By _customersButton = By.XPath("//button[contains(text(), 'Customers')]");
    
    public BankManagerPage(IWebDriver driver)
    {
        _driver = driver;
    }
    
    /// <summary>
    /// Переходить на сторінку Customers
    /// </summary>
    public void GoToCustomers()
    {
        var button = WaitHelper.WaitForElementClickable(_driver, _customersButton);
        button.Click();
    }
}

