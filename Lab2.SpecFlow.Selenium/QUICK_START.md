# Швидкий старт

## Швидкий запуск тестів

```bash
# 1. Перейти в директорію проєкту
cd /Users/sonyakoldun/Downloads/LabTesting5_backup/Lab2.SpecFlow.Selenium

# 2. Відновити залежності
dotnet restore

# 3. Зібрати проєкт
dotnet build

# 4. Запустити тести
dotnet test
```

## Запуск у headless режимі

```bash
HEADLESS=true dotnet test
```

## Запуск з детальним виводом

```bash
dotnet test --logger "console;verbosity=detailed"
```

## Важливі примітки

1. **Переконайтеся, що Chrome встановлено** - ChromeDriver завантажиться автоматично
2. **Перевірте інтернет-з'єднання** - тести працюють з реальним сайтом
3. **Клієнт "Harry Potter"** повинен існувати на сайті перед запуском тесту
   - Якщо клієнта немає, спочатку створіть його через Bank Manager → Add Customer

## Troubleshooting

Якщо тест падає:
- Перевірте, що сайт доступний: https://www.globalsqa.com/angularJs-protractor/BankingProject/#/login
- Запустіть тест у звичайному режимі (не headless) для візуального контролю
- Перевірте скріншоти в `TestResults/Screenshots/`

