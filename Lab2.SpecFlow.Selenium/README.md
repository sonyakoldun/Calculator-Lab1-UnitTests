# Лабораторна робота №2: Тестування з Selenium WebDriver та SpecFlow

## Опис лабораторної роботи

**Варіант:** 9  
**Технології:** C#, Selenium WebDriver, SpecFlow, Page Object Model (POM), BDD  
**Сайт для тестування:** GlobalSQA BankingProject (XYZ Bank)  
**Сценарій:** Bank Manager Login → Customers → Delete Customer

### Мета роботи

Автоматизація тестування функціональності видалення клієнта з системи банку через інтерфейс Bank Manager.

### Сценарій тестування

1. Відкрити сайт XYZ Bank
2. Натиснути "Bank Manager Login"
3. Перейти на сторінку "Customers"
4. Знайти клієнта за ім'ям та прізвищем (наприклад, "Harry Potter")
5. Натиснути кнопку "Delete" для видалення клієнта
6. Перевірити, що клієнта більше немає в таблиці клієнтів

## Технології та підходи

- **Мова програмування:** C# (.NET 8.0)
- **Фреймворк тестування:** NUnit
- **Автоматизація браузера:** Selenium WebDriver 4.15.0
- **BDD фреймворк:** SpecFlow 3.9.74
- **Паттерн проектування:** Page Object Model (POM)
- **Браузер:** Google Chrome (з автоматичним управлінням ChromeDriver через Selenium Manager)
- **Асерти:** FluentAssertions

## Структура проєкту

```
Lab2.SpecFlow.Selenium/
├── Features/
│   └── DeleteCustomer.feature          # Gherkin сценарій
├── StepDefinitions/
│   └── DeleteCustomerSteps.cs          # Реалізація кроків
├── Pages/
│   ├── HomePage.cs                     # Page Object головної сторінки
│   ├── BankManagerPage.cs              # Page Object сторінки менеджера
│   └── CustomersPage.cs                # Page Object сторінки клієнтів
├── Drivers/
│   └── WebDriverFactory.cs             # Фабрика для створення WebDriver
├── Hooks/
│   └── ScenarioHooks.cs                # SpecFlow hooks (Before/After)
├── Support/
│   ├── Config.cs                       # Конфігурація (URL, таймаути)
│   └── WaitHelper.cs                   # Допоміжні методи для очікувань
├── TestData/                           # Тестові дані (за потреби)
├── TestResults/
│   └── Screenshots/                    # Скріншоти при падінні тестів
└── appsettings.json                    # Налаштування проєкту
```

## Встановлення та запуск на macOS

### Вимоги

1. **.NET SDK 8.0 або новіша версія**
   ```bash
   dotnet --version
   ```
   Якщо не встановлено, завантажте з [dotnet.microsoft.com](https://dotnet.microsoft.com/download)

2. **Google Chrome**
   - Встановіть з [google.com/chrome](https://www.google.com/chrome/)
   - ChromeDriver буде автоматично завантажений через Selenium Manager

3. **Git** (опційно, для клонування репозиторію)

### Крок 1: Відкрити проєкт

```bash
cd /Users/sonyakoldun/Downloads/Calculator/Lab2.SpecFlow.Selenium
```

### Крок 2: Відновити залежності

```bash
dotnet restore
```

### Крок 3: Згенерувати код з SpecFlow

```bash
dotnet build
```

Це автоматично згенерує код з `.feature` файлів.

### Крок 4: Запустити тести

```bash
dotnet test
```

Або з додатковими опціями:

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Крок 5: Переглянути результати

- Результати тестів відображаються в консолі
- Скріншоти при падінні зберігаються в `TestResults/Screenshots/`

## Налаштування

### Headless режим

За замовчуванням браузер запускається у звичайному режимі. Для запуску в headless режимі (без вікна браузера):

**Варіант 1: Через змінну оточення**
```bash
export HEADLESS=true
dotnet test
```

**Варіант 2: Безпосередньо в команді**
```bash
HEADLESS=true dotnet test
```

**Варіант 3: Змінити в коді**
Відредагуйте файл `Support/Config.cs`:
```csharp
public static bool Headless => true; // замість false
```

### Зміна базового URL

```bash
export BASE_URL="https://www.globalsqa.com/angularJs-protractor/BankingProject/#/login"
dotnet test
```

### Зміна таймауту очікування

```bash
export TIMEOUT_SECONDS=15
dotnet test
```

## Troubleshooting для macOS

### Проблема 1: ChromeDriver не знайдено

**Симптоми:** `WebDriverException: chromedriver executable may not be found`

**Рішення:**
- Selenium 4+ автоматично завантажує ChromeDriver через Selenium Manager
- Переконайтеся, що Chrome встановлено та оновлено до останньої версії
- Перевірте, що Chrome доступний в PATH:
  ```bash
  /Applications/Google\ Chrome.app/Contents/MacOS/Google\ Chrome --version
  ```

### Проблема 2: Дозволи macOS

**Симптоми:** Chrome не запускається, помилки доступу

**Рішення:**
1. Системні налаштування → Безпека та конфіденційність → Дозволи
2. Дозвольте доступ для:
   - Terminal (або ваш IDE)
   - Google Chrome
3. Перезапустіть термінал/IDE

### Проблема 3: M1/M2 Mac (Apple Silicon)

**Симптоми:** Помилки сумісності з ChromeDriver

**Рішення:**
- Selenium Manager автоматично визначає архітектуру
- Переконайтеся, що використовуєте нативну версію Chrome для Apple Silicon
- Якщо проблеми залишаються, встановіть ChromeDriver вручну:
  ```bash
  brew install chromedriver
  ```

### Проблема 4: Тест не знаходить елементи

**Симптоми:** `NoSuchElementException`, `ElementNotVisibleException`

**Рішення:**
1. Перевірте, що сайт доступний: відкрийте URL вручну в браузері
2. Збільште таймаут очікування:
   ```bash
   export TIMEOUT_SECONDS=20
   ```
3. Запустіть тест у звичайному режимі (не headless) для візуального контролю:
   ```bash
   HEADLESS=false dotnet test
   ```

### Проблема 5: SpecFlow не генерує код

**Симптоми:** Помилки компіляції, не знайдено step definitions

**Рішення:**
1. Переконайтеся, що пакет `SpecFlow.Tools.MsBuild.Generation` встановлено
2. Виконайте повну перебудову:
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

### Проблема 6: Клієнт не знайдено для видалення

**Симптоми:** Тест падає з помилкою "Клієнт не знайдено"

**Рішення:**
1. Перевірте, що клієнт "Harry Potter" існує на сайті
2. Якщо клієнта немає, спочатку створіть його через інтерфейс Bank Manager → Add Customer
3. Або змініть тестовий сценарій на існуючого клієнта

## Приклади використання

### Запуск одного тесту

```bash
dotnet test --filter "FullyQualifiedName~DeleteCustomer"
```

### Запуск з детальним логуванням

```bash
dotnet test --logger "console;verbosity=detailed" -- NUnit.Verbosity=2
```

### Запуск у headless режимі зі збільшеним таймаутом

```bash
HEADLESS=true TIMEOUT_SECONDS=15 dotnet test
```

## Архітектура проєкту

### Page Object Model (POM)

Кожна сторінка представлена окремим класом:
- **HomePage**: Відкриття сайту, логін
- **BankManagerPage**: Навігація по меню менеджера
- **CustomersPage**: Робота з таблицею клієнтів, пошук, видалення

### SpecFlow Hooks

- `[BeforeScenario]`: Створює WebDriver перед кожним тестом
- `[AfterScenario]`: Закриває WebDriver та робить скріншот при помилці

### Dependency Injection

SpecFlow автоматично інжектує `IWebDriver` у всі step definitions та page objects через `IObjectContainer`.

## Додаткові матеріали

- [Selenium WebDriver Documentation](https://www.selenium.dev/documentation/)
- [SpecFlow Documentation](https://docs.specflow.org/)
- [Page Object Model Pattern](https://www.selenium.dev/documentation/test_practices/encouraged/page_object_models/)
- [GlobalSQA BankingProject](https://www.globalsqa.com/angularJs-protractor/BankingProject/)

## Автор

Проєкт створено для лабораторної роботи №2 з тестування програмного забезпечення.

---

**Примітка:** Переконайтеся, що у вас є стабільне інтернет-з'єднання, оскільки тести працюють з реальним веб-сайтом.

