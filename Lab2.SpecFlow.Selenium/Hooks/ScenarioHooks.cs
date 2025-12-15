using BoDi;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Lab2.SpecFlow.Selenium.Drivers;
using Lab2.SpecFlow.Selenium.Support;

namespace Lab2.SpecFlow.Selenium.Hooks;

/// <summary>
/// SpecFlow hooks для управління життєвим циклом WebDriver
/// </summary>
[Binding]
public class ScenarioHooks
{
    private readonly IObjectContainer _objectContainer;
    private IWebDriver? _driver;
    
    public ScenarioHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }
    
    /// <summary>
    /// Виконується перед кожним сценарієм - створює WebDriver
    /// </summary>
    [BeforeScenario]
    public void BeforeScenario()
    {
        _driver = WebDriverFactory.CreateChromeDriver();
        _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
    }
    
    /// <summary>
    /// Виконується після кожного сценарію - закриває WebDriver та робить скріншот при помилці
    /// </summary>
    [AfterScenario]
    public void AfterScenario(ScenarioContext scenarioContext)
    {
        if (_driver != null)
        {
            // Робимо скріншот, якщо сценарій упав
            if (scenarioContext.TestError != null)
            {
                TakeScreenshot(scenarioContext);
            }
            
            _driver.Quit();
            _driver.Dispose();
        }
    }
    
    /// <summary>
    /// Зберігає скріншот при падінні тесту
    /// </summary>
    private void TakeScreenshot(ScenarioContext scenarioContext)
    {
        try
        {
            var screenshotDir = Path.Combine(Directory.GetCurrentDirectory(), "TestResults", "Screenshots");
            if (!Directory.Exists(screenshotDir))
            {
                Directory.CreateDirectory(screenshotDir);
            }
            
            var screenshot = ((ITakesScreenshot)_driver!).GetScreenshot();
            var fileName = $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}_{scenarioContext.ScenarioInfo.Title.Replace(" ", "_")}.png";
            var filePath = Path.Combine(screenshotDir, fileName);
            screenshot.SaveAsFile(filePath);
            
            Console.WriteLine($"Screenshot saved: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to take screenshot: {ex.Message}");
        }
    }
}

