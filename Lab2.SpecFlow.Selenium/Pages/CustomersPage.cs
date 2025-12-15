using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Lab2.SpecFlow.Selenium.Support;

namespace Lab2.SpecFlow.Selenium.Pages;

/// <summary>
/// Page Object для сторінки управління клієнтами
/// </summary>
public class CustomersPage
{
    private readonly IWebDriver _driver;
    
    // Локатори
    private readonly By _searchInput = By.CssSelector("input[placeholder='Search Customer']");
    private readonly By _customersTable = By.CssSelector("table.table"); // сама таблиця
    private readonly By _tableRows = By.CssSelector("table.table tbody tr");
    private readonly By _deleteButtonInRow = By.XPath(".//button[contains(normalize-space(.), 'Delete')]");

    public CustomersPage(IWebDriver driver)
    {
        _driver = driver;
    }
    
    /// <summary>
    /// Шукає клієнта за ім'ям та прізвищем
    /// </summary>
    public void SearchCustomer(string firstName, string lastName)
    {
        var searchInput = WaitHelper.WaitForElementVisible(_driver, _searchInput, Config.TimeoutSeconds);

        searchInput.Clear();
        searchInput.SendKeys(firstName);

        // Очікуємо, що таблиця існує (не рядок!)
        WaitHelper.WaitForElementVisible(_driver, _customersTable, Config.TimeoutSeconds);
    }
    
    /// <summary>
    /// Перевіряє, чи присутній клієнт у таблиці
    /// </summary>
    public bool IsCustomerPresent(string firstName, string lastName)
    {
        try
        {
            var rows = _driver.FindElements(_tableRows);
            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells.Count >= 2)
                {
                    var fullName = $"{cells[0].Text} {cells[1].Text}".Trim();
                    if (fullName.Contains(firstName, StringComparison.OrdinalIgnoreCase) &&
                        fullName.Contains(lastName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// Видаляє клієнта з таблиці
    /// </summary>
    public void DeleteCustomer(string firstName, string lastName)
    {
    // Чекаємо, що таблиця точно є на сторінці
        WaitHelper.WaitForElementVisible(_driver, _customersTable, Config.TimeoutSeconds);

    // Беремо всі рядки (якщо 0 — значить фільтр нічого не знайшов/сторінка не завантажилась)
        var allRows = _driver.FindElements(_tableRows);

        IWebElement? targetRow = null;
        foreach (var row in allRows)
        {
            var cells = row.FindElements(By.TagName("td"));
            if (cells.Count >= 2)
            {
                var fullName = $"{cells[0].Text} {cells[1].Text}".Trim();
                if (fullName.Contains(firstName, StringComparison.OrdinalIgnoreCase) &&
                    fullName.Contains(lastName, StringComparison.OrdinalIgnoreCase))
                {
                    targetRow = row;
                    break;
                }
            }
        }

    if (targetRow == null)
        throw new Exception($"Клієнт {firstName} {lastName} не знайдено в таблиці");

    // КНОПКА DELETE — тільки в цьому рядку (увага на .//)
    var deleteButton = targetRow.FindElement(_deleteButtonInRow);

    // Клікаємо
    deleteButton.Click();

    // Чекаємо, що клієнт зникне (правильна перевірка після видалення)
    WaitHelper.CreateWait(_driver, Config.TimeoutSeconds)
        .Until(_ => !IsCustomerPresent(firstName, lastName));
}
    
    /// <summary>
    /// Очікує оновлення таблиці після операції
    /// </summary>
    private void WaitForTableUpdate()
    {
        var wait = WaitHelper.CreateWait(_driver, Config.TimeoutSeconds);
        // Очікуємо, поки таблиця оновиться (можна перевірити зміну кількості рядків)
        Thread.Sleep(1000); // Невелика затримка для оновлення таблиці
    }
}

