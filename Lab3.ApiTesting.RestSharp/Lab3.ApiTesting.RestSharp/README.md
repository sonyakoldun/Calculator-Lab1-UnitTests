# Лабораторна робота №3: Тестування Web API

## Опис лабораторної роботи

**Тема:** Тестування Web API з використанням RestSharp та SpecFlow  
**Технології:** C#, .NET 8.0, xUnit, SpecFlow, RestSharp, FluentAssertions, BDD  
**API для тестування:**
- RestfulBooker (https://restful-booker.herokuapp.com/) - CRUD операції
- Cat Facts API (https://catfact.ninja/) - публічний API

### Мета роботи

Автоматизація тестування Web API з використанням BDD підходу (Gherkin), SpecFlow для специфікації тестів та RestSharp для виконання HTTP запитів.

### Сценарії тестування

#### RestfulBooker API (CRUD операції):
1. **Створення та отримання бронювання**
   - Створення нового бронювання
   - Перевірка повернутого booking ID
   - Отримання створеного бронювання
   - Перевірка відповідності даних

2. **Оновлення бронювання (PUT)**
   - Отримання токену аутентифікації
   - Створення бронювання
   - Повне оновлення бронювання
   - Перевірка оновлених даних

3. **Видалення бронювання**
   - Отримання токену аутентифікації
   - Створення бронювання
   - Видалення бронювання
   - Перевірка, що бронювання видалено

4. **Негативний кейс**
   - Спроба отримати неіснуюче бронювання
   - Перевірка повернення 404

#### Cat Facts API:
1. **Отримання випадкового факту про котів**
   - GET запит до /fact
   - Перевірка статусу 200
   - Перевірка наявності факту та його довжини

2. **Негативний кейс**
   - Запит до неіснуючого endpoint
   - Перевірка повернення 404

## Технології та підходи

- **Мова програмування:** C# (.NET 8.0)
- **Фреймворк тестування:** xUnit
- **BDD фреймворк:** SpecFlow 3.9.74
- **HTTP клієнт:** RestSharp 110.2.0
- **Асерти:** FluentAssertions 6.12.0
- **JSON серіалізація:** Newtonsoft.Json 13.0.3
- **Конфігурація:** Microsoft.Extensions.Configuration

## Структура проєкту

```
Lab3.ApiTesting.RestSharp/
├── Lab3.ApiTesting.RestSharp.sln
├── Lab3.ApiTesting.RestSharp/
│   ├── Clients/
│   │   ├── RestfulBookerClient.cs      # Клієнт для RestfulBooker API
│   │   └── CatFactsClient.cs            # Клієнт для Cat Facts API
│   ├── Models/
│   │   ├── Booking.cs                   # Модель бронювання
│   │   ├── BookingDates.cs              # Модель дат бронювання
│   │   ├── CreateBookingResponse.cs     # Модель відповіді на створення
│   │   ├── AuthRequest.cs               # Модель запиту аутентифікації
│   │   ├── AuthResponse.cs              # Модель відповіді аутентифікації
│   │   └── CatFactResponse.cs           # Модель відповіді Cat Facts API
│   ├── Features/
│   │   ├── RestfulBookerCrud.feature    # Gherkin сценарії для RestfulBooker
│   │   └── CatFactsApi.feature          # Gherkin сценарії для Cat Facts
│   ├── StepDefinitions/
│   │   ├── RestfulBookerSteps.cs        # Реалізація кроків RestfulBooker
│   │   └── CatFactsSteps.cs             # Реалізація кроків Cat Facts
│   ├── Hooks/
│   │   └── ScenarioHooks.cs             # SpecFlow hooks (Before/After)
│   ├── Support/
│   │   ├── Config.cs                    # Конфігурація (URL, credentials)
│   │   └── TestContext.cs               # Контекст для зберігання даних
│   ├── appsettings.json                 # Налаштування проєкту
│   ├── specflow.json                    # Конфігурація SpecFlow
│   └── Lab3.ApiTesting.RestSharp.csproj # Файл проєкту
├── README.md                            # Цей файл
└── REPORT_NOTES.md                      # Матеріали для звіту
```

## Встановлення та запуск на macOS

### Вимоги

1. **.NET SDK 8.0 або новіша версія**
   ```bash
   dotnet --version
   ```
   Якщо не встановлено, завантажте з [dotnet.microsoft.com](https://dotnet.microsoft.com/download)

2. **Git** (опційно, для клонування репозиторію)

### Крок 1: Відкрити проєкт

```bash
cd Lab3.ApiTesting.RestSharp
```

### Крок 2: Відновити залежності

```bash
dotnet restore
```

### Крок 3: Згенерувати код з SpecFlow

```bash
dotnet build
```

Це автоматично згенерує код з `.feature` файлів (файли `*.feature.cs`).

### Крок 4: Запустити тести

```bash
dotnet test
```

Для детальнішого виводу:

```bash
dotnet test --logger "console;verbosity=detailed"
```

## Конфігурація

Налаштування API endpoints та credentials зберігаються в `appsettings.json`:

```json
{
  "RestfulBooker": {
    "BaseUrl": "https://restful-booker.herokuapp.com",
    "Username": "admin",
    "Password": "password123"
  },
  "CatFacts": {
    "BaseUrl": "https://catfact.ninja"
  }
}
```

Також можна перевизначити через змінні оточення:
- `RESTFUL_BOOKER_BASE_URL`
- `RESTFUL_BOOKER_USERNAME`
- `RESTFUL_BOOKER_PASSWORD`
- `CAT_FACTS_BASE_URL`

## Архітектура

### Потік виконання тесту

1. **BeforeScenario Hook** → Створює та реєструє API клієнти та контекст
2. **Step: Given** → Підготовка (перевірка доступності API, створення токену)
3. **Step: When** → Виконання дії (CRUD операції)
4. **Step: Then** → Перевірка результатів через FluentAssertions
5. **AfterScenario Hook** → Очищення ресурсів (якщо необхідно)

### Dependency Injection

SpecFlow використовує BoDi для Dependency Injection:
- Клієнти реєструються в Hooks через `IObjectContainer`
- Step Definitions отримують клієнти через конструктор
- Контекст тесту зберігає дані між кроками

### API Клієнти

Кожен API має свій окремий клієнт клас:
- **RestfulBookerClient** - методи для CRUD операцій
- **CatFactsClient** - методи для роботи з Cat Facts API

Всі HTTP запити виконуються через RestSharp, відповіді десеріалізуються в моделі.

## Приклади використання

### Створення бронювання

```csharp
var booking = new Booking
{
    Firstname = "John",
    Lastname = "Doe",
    Totalprice = 150,
    Depositpaid = true,
    Bookingdates = new BookingDates
    {
        Checkin = "2024-01-01",
        Checkout = "2024-01-05"
    },
    Additionalneeds = "Breakfast"
};

var response = _client.CreateBooking(booking);
response.IsSuccessful.Should().BeTrue();
```

### Отримання токену

```csharp
var response = _client.CreateToken("admin", "password123");
var token = response.Data.Token;
```

## Висновки

Проєкт демонструє:
- ✅ Використання BDD підходу (Gherkin + SpecFlow)
- ✅ Тестування CRUD операцій через REST API
- ✅ Роботу з публічними API
- ✅ Чисту архітектуру з розділенням відповідальності
- ✅ Використання FluentAssertions для читабельних assertions
- ✅ Конфігурацію через appsettings.json
- ✅ Незалежність тестів

## Посилання

- [RestfulBooker API Documentation](https://restful-booker.herokuapp.com/apidoc/index.html)
- [Cat Facts API](https://catfact.ninja/)
- [SpecFlow Documentation](https://docs.specflow.org/)
- [RestSharp Documentation](https://restsharp.dev/)
- [FluentAssertions Documentation](https://fluentassertions.com/)

