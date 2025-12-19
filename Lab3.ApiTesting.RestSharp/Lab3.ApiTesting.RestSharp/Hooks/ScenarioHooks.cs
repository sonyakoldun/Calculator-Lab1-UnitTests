using BoDi;
using TechTalk.SpecFlow;
using Lab3.ApiTesting.RestSharp.Clients;
using Lab3.ApiTesting.RestSharp.Support;

namespace Lab3.ApiTesting.RestSharp.Hooks;

[Binding]
public class ScenarioHooks
{
    private readonly IObjectContainer _objectContainer;

    public ScenarioHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        // Реєструємо клієнти
        var restfulBookerClient = new RestfulBookerClient(Config.RestfulBookerBaseUrl);
        var catFactsClient = new CatFactsClient(Config.CatFactsBaseUrl);
        
        _objectContainer.RegisterInstanceAs(restfulBookerClient);
        _objectContainer.RegisterInstanceAs(catFactsClient);
        
        // Реєструємо контекст тесту
        var testContext = new TestContext();
        _objectContainer.RegisterInstanceAs(testContext);
    }

    [AfterScenario]
    public void AfterScenario()
    {
        // Очищення ресурсів при необхідності
        // RestSharp клієнти не потребують явного Dispose в новіших версіях
    }
}

