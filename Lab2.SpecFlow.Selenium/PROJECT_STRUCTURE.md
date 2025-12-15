# Структура проєкту лабораторної роботи №2

## Повний список файлів

### Кореневі файли
- `Lab2.SpecFlow.Selenium.csproj` - файл проєкту з усіма NuGet пакетами
- `Lab2.SpecFlow.Selenium.sln` - solution файл
- `README.md` - детальна документація українською
- `QUICK_START.md` - швидкий старт
- `specflow.json` - конфігурація SpecFlow
- `appsettings.json` - налаштування (URL, headless, таймаути)
- `.gitignore` - ігнорування файлів для Git

### Features/ (Gherkin сценарії)
- `DeleteCustomer.feature` - Gherkin сценарій для видалення клієнта
- `DeleteCustomer.feature.cs` - автоматично згенерований код (не редагувати вручну)

### StepDefinitions/ (Реалізація кроків)
- `DeleteCustomerSteps.cs` - реалізація всіх кроків з feature файлу

### Pages/ (Page Object Model)
- `HomePage.cs` - Page Object для головної сторінки (відкриття сайту, логін)
- `BankManagerPage.cs` - Page Object для сторінки менеджера (навігація)
- `CustomersPage.cs` - Page Object для сторінки клієнтів (пошук, видалення)

### Drivers/ (Управління WebDriver)
- `WebDriverFactory.cs` - фабрика для створення ChromeDriver з налаштуваннями

### Hooks/ (SpecFlow Hooks)
- `ScenarioHooks.cs` - BeforeScenario/AfterScenario hooks (створення/закриття драйвера, скріншоти)

### Support/ (Допоміжні класи)
- `Config.cs` - конфігурація (URL, headless, таймаути) з підтримкою змінних оточення
- `WaitHelper.cs` - допоміжні методи для роботи з очікуваннями (явні waits)

### TestData/ (Тестові дані)
- Папка для зберігання тестових даних (за потреби)

### TestResults/Screenshots/ (Результати тестів)
- Папка для зберігання скріншотів при падінні тестів

## Архітектура

### Потік виконання тесту

1. **BeforeScenario Hook** → Створює ChromeDriver через WebDriverFactory
2. **Step: Given I open XYZ Bank application** → HomePage.Open()
3. **Step: When I log in as bank manager** → HomePage.ClickBankManagerLogin()
4. **Step: And I open Customers page** → BankManagerPage.GoToCustomers()
5. **Step: And I search for customer** → CustomersPage.SearchCustomer()
6. **Step: And I delete the customer** → CustomersPage.DeleteCustomer()
7. **Step: Then the customer should not be present** → CustomersPage.IsCustomerPresent() + FluentAssertions
8. **AfterScenario Hook** → Скріншот (якщо помилка) + driver.Quit()

### Dependency Injection

SpecFlow автоматично інжектує `IWebDriver` через `IObjectContainer`:
- У Hooks (створення та реєстрація)
- У Step Definitions (через конструктор)
- У Page Objects (через конструктор у Steps)

### Page Object Model

Кожна сторінка має:
- Локатори (By) як приватні поля
- Методи для взаємодії зі сторінкою
- Використання WaitHelper для надійності
- Немає asserts (asserts тільки в Step Definitions)

## Технології

- **.NET 8.0** - платформа
- **NUnit 3.14.0** - фреймворк тестування
- **SpecFlow 3.9.74** - BDD фреймворк
- **Selenium WebDriver 4.15.0** - автоматизація браузера
- **FluentAssertions 6.12.0** - читабельні асерти
- **DotNetSeleniumExtras.WaitHelpers 3.11.0** - допоміжні методи для очікувань

## Команди для запуску

```bash
# Відновлення залежностей
dotnet restore

# Збірка проєкту
dotnet build

# Запуск тестів
dotnet test

# Запуск у headless режимі
HEADLESS=true dotnet test

# Запуск з детальним виводом
dotnet test --logger "console;verbosity=detailed"
```

