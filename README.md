# Лабораторна робота №1: Модульне тестування ПЗ

## 📋 Опис

Цей проєкт містить unit-тести для методу `RunEstimate()` з класу `AnalaizerClass` (бібліотека `AnalizerClassLibrary`).

**Варіант:** №9  
**Курс:** Автоматизовані системи тестування програмного продукту  
**Платформа:** macOS (через Mono)

## ⚠️ Важливо

Проєкт використовує **.NET Framework 4.7.2**, який на macOS потребує **Mono** для збірки та запуску.  
**`dotnet test` НЕ ПРАЦЮЄ** з .NET Framework проєктами на macOS.

## 🚀 Швидкий старт

### Вимоги

- **Mono** (встановлений через Homebrew)
- **NuGet CLI** (для встановлення пакетів)
- **SQLite** (вже встановлений на macOS)

### Встановлення

```bash
# 1. Встановити Mono
brew install mono

# 2. Завантажити NuGet CLI
cd /Users/sonyakoldun/Downloads/Calculator
curl -L -o nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
chmod +x nuget.exe

# 3. Встановити NuGet пакети
mono nuget.exe install MSTest.TestFramework -Version 2.2.7 -OutputDirectory packages -NoHttpCache
mono nuget.exe install System.Data.SQLite.Core -Version 1.0.118.0 -OutputDirectory packages -NoHttpCache
```

### Збірка проєкту

```bash
cd /Users/sonyakoldun/Downloads/Calculator

# Збірка через Mono MSBuild
/Library/Frameworks/Mono.framework/Versions/Current/bin/msbuild \
  AnalizerClassLibrary.Tests/AnalizerClassLibrary.Tests.csproj \
  /t:Build /p:Configuration=Debug /p:RestorePackages=false
```

### Запуск тестів

Детальні інструкції дивіться у файлі: **[RUN_TESTS_MACOS.md](RUN_TESTS_MACOS.md)**

## 📁 Структура проєкту

```
Calculator/
├── AnalizerClassLibrary/          # Бібліотека для тестування
├── AnalizerClassLibrary.Tests/   # Unit-тести
│   ├── RunEstimateTests.cs      # Тести для RunEstimate()
│   └── packages.config           # NuGet пакети
├── Database/
│   └── calculator_test.db       # SQLite база даних з тестовими випадками
├── packages/                     # NuGet пакети (встановлюються окремо)
├── RUN_TESTS_MACOS.md           # Інструкції для запуску тестів
└── WHY_DOTNET_CLI_DOES_NOT_WORK.md  # Пояснення чому dotnet CLI не працює
```

## 🧪 Тести

Тести читають дані з таблиці `TestExpressions` в базі даних `Database/calculator_test.db` та перевіряють:

- ✅ **Valid** (18 тестів) - коректні вирази
- ❌ **DivisionByZero** (4 тести) - помилки ділення на 0 (Error 09)
- ❌ **Overflow** (3 тести) - помилки переповнення (Error 06)
- ❌ **TooManyOperands** (1 тест) - занадто багато операндів (Error 08)
- 🔍 **EdgeCase** (5 тестів) - граничні випадки

**Всього:** 31 тест + 5 додаткових unit-тестів без БД

## 📚 Документація

- **[RUN_TESTS_MACOS.md](RUN_TESTS_MACOS.md)** - Детальні інструкції для запуску тестів на macOS
- **[WHY_DOTNET_CLI_DOES_NOT_WORK.md](WHY_DOTNET_CLI_DOES_NOT_WORK.md)** - Технічне пояснення чому `dotnet test` не працює

## ⚙️ Технічні деталі

- **Framework:** .NET Framework 4.7.2
- **Test Framework:** MSTest 2.2.7
- **Database:** SQLite (System.Data.SQLite.Core 1.0.118.0)
- **Build Tool:** Mono MSBuild
- **OS:** macOS

## ✅ Перевірка роботи

Після успішного запуску тестів ви побачите:

```
Test run for AnalizerClassLibrary.Tests.dll
Total tests: 36
Passed: 36
Failed: 0
```

## 📝 Примітки

1. База даних `Database/calculator_test.db` **НЕ ЗМІНЮЄТЬСЯ** - вона вже містить всі необхідні тестові дані
2. Всі тести використовують метод `PrepareExpression()` для підготовки виразів перед викликом `RunEstimate()`
3. Тести автоматично очищають `AnalaizerClass.expression` після кожного тесту
