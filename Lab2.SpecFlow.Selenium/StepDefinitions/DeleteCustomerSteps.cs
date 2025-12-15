using FluentAssertions;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Lab2.SpecFlow.Selenium.Pages;
using Lab2.SpecFlow.Selenium.Support;

namespace Lab2.SpecFlow.Selenium.StepDefinitions;

/// <summary>
/// Step Definitions для сценарію видалення клієнта
/// </summary>
[Binding]
public class DeleteCustomerSteps
{
    private readonly IWebDriver _driver;
    private HomePage? _homePage;
    private BankManagerPage? _bankManagerPage;
    private CustomersPage? _customersPage;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    
    public DeleteCustomerSteps(IWebDriver driver)
    {
        _driver = driver;
    }
    
    [Given(@"I open XYZ Bank application")]
    public void GivenIOpenXYZBankApplication()
    {
        _homePage = new HomePage(_driver);
        _homePage.Open(Config.BaseUrl);
    }
    
    [When(@"I log in as bank manager")]
    public void WhenILogInAsBankManager()
    {
        _homePage ??= new HomePage(_driver);
        _homePage.ClickBankManagerLogin();
        _bankManagerPage = new BankManagerPage(_driver);
    }
    
    [When(@"I open Customers page")]
    public void WhenIOpenCustomersPage()
    {
        _bankManagerPage ??= new BankManagerPage(_driver);
        _bankManagerPage.GoToCustomers();
        _customersPage = new CustomersPage(_driver);
    }
    
    [When(@"I search for customer ""(.*)"" ""(.*)""")]
    public void WhenISearchForCustomer(string firstName, string lastName)
    {
        _firstName = firstName;
        _lastName = lastName;
        _customersPage ??= new CustomersPage(_driver);
        _customersPage.SearchCustomer(firstName, lastName);
    }
    
    [When(@"I delete the customer")]
    public void WhenIDeleteTheCustomer()
    {
        _customersPage ??= new CustomersPage(_driver);
        _customersPage.DeleteCustomer(_firstName, _lastName);
    }
    
    [Then(@"the customer should not be present in the customers list")]
    public void ThenTheCustomerShouldNotBePresentInTheCustomersList()
    {
        _customersPage ??= new CustomersPage(_driver);
        
        // Очищаємо пошук, щоб побачити всіх клієнтів
        var searchInput = _driver.FindElement(By.XPath("//input[@placeholder='Search Customer']"));
        searchInput.Clear();
        Thread.Sleep(500);
        
        // Перевіряємо, що клієнта більше немає
        var isPresent = _customersPage.IsCustomerPresent(_firstName, _lastName);
        isPresent.Should().BeFalse($"Клієнт {_firstName} {_lastName} не повинен бути присутнім у списку після видалення");
    }
}

