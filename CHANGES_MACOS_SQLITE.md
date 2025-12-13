# 🔄 Зміни: Переробка під macOS та SQLite

## ✅ Що було змінено

### 1. SQL скрипт (`Database/CreateTestDatabase.sql`)

**Було (MS SQL Server):**
```sql
CREATE DATABASE CalculatorTestDB;
USE CalculatorTestDB;
CREATE TABLE TestExpressions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IsError BIT NOT NULL,
    ...
);
```

**Стало (SQLite):**
```sql
DROP TABLE IF EXISTS TestExpressions;
CREATE TABLE TestExpressions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IsError INTEGER NOT NULL,  -- 0 або 1 замість BIT
    ...
);
```

**Основні зміни:**
- ✅ Видалено `CREATE DATABASE` та `USE` (SQLite не потребує)
- ✅ `INT IDENTITY(1,1)` → `INTEGER PRIMARY KEY AUTOINCREMENT`
- ✅ `BIT` → `INTEGER` (0 або 1)
- ✅ `NVARCHAR` → `TEXT`
- ✅ `NVARCHAR(MAX)` → `TEXT`
- ✅ Додано `IF EXISTS` та `IF NOT EXISTS` для безпечного виконання
- ✅ Замінено `PRINT` на `SELECT` для виведення статистики

### 2. App.config

**Було:**
```xml
<connectionStrings>
  <add name="TestDatabase" 
       connectionString="Data Source=localhost;Initial Catalog=CalculatorTestDB;Integrated Security=True;..." 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Стало:**
```xml
<connectionStrings>
  <add name="TestDatabase" 
       connectionString="Data Source=CalculatorTestDB.db;Version=3;" 
       providerName="System.Data.SQLite" />
</connectionStrings>
```

### 3. Код тестів (`RunEstimateTests.cs`)

**Було:**
```csharp
using System.Data.SqlClient;
...
using (SqlConnection connection = new SqlConnection(connectionString))
using (SqlCommand command = new SqlCommand(query, connection))
using (SqlDataReader reader = command.ExecuteReader())
{
    bool isError = (bool)reader["IsError"];
}
```

**Стало:**
```csharp
using System.Data.SQLite;
using System.IO;
...
using (SQLiteConnection connection = new SQLiteConnection(connectionString))
using (SQLiteCommand command = new SQLiteCommand(query, connection))
using (SQLiteDataReader reader = command.ExecuteReader())
{
    bool isError = Convert.ToBoolean(reader["IsError"]); // SQLite повертає INTEGER
}
```

**Основні зміни:**
- ✅ `SqlConnection` → `SQLiteConnection`
- ✅ `SqlCommand` → `SQLiteCommand`
- ✅ `SqlDataReader` → `SQLiteDataReader`
- ✅ `(bool)reader["IsError"]` → `Convert.ToBoolean(reader["IsError"])`
- ✅ Додано автоматичний пошук файлу БД у різних місцях

### 4. Проєкт файл (`.csproj`)

**Додано:**
```xml
<Reference Include="System.Data.SQLite, Version=1.0.118.0, ...">
  <HintPath>..\packages\System.Data.SQLite.Core.1.0.118.0\lib\net46\System.Data.SQLite.dll</HintPath>
</Reference>
```

### 5. NuGet пакети (`packages.config`)

**Додано:**
```xml
<package id="System.Data.SQLite.Core" version="1.0.118.0" targetFramework="net472" />
```

### 6. Документація

**Створено нові файли:**
- ✅ `README_TESTS_MACOS.md` - повна документація для macOS
- ✅ `QUICK_START_MACOS.md` - швидкий старт для macOS
- ✅ `CHANGES_MACOS_SQLITE.md` - цей файл

---

## 📋 Інструкції для використання

### Швидкий старт:

1. **Встановіть DBeaver:**
   ```bash
   brew install --cask dbeaver-community
   ```

2. **Створіть БД в DBeaver:**
   - Відкрийте DBeaver
   - Створіть нове підключення SQLite
   - Вкажіть шлях: `Database/CalculatorTestDB.db`
   - Виконайте скрипт `Database/CreateTestDatabase.sql`

3. **Запустіть тести:**
   ```bash
   dotnet test AnalizerClassLibrary.Tests/AnalizerClassLibrary.Tests.csproj
   ```

Детальні інструкції: `QUICK_START_MACOS.md`

---

## 🔍 Відмінності SQLite від MS SQL Server

| Аспект | MS SQL Server | SQLite |
|--------|--------------|--------|
| Тип БД | Серверна | Файлова |
| Файл | `.mdf`, `.ldf` | `.db` |
| Підключення | Сервер + база | Файл |
| Типи даних | `BIT`, `NVARCHAR`, `INT IDENTITY` | `INTEGER`, `TEXT`, `INTEGER AUTOINCREMENT` |
| Синтаксис | `IF EXISTS`, `GO` | `IF EXISTS`, без `GO` |
| Клієнт | SSMS | DBeaver, DB Browser, тощо |

---

## ✅ Переваги SQLite для macOS

1. **Не потребує сервера** - просто файл `.db`
2. **Легко встановити** - вже є на macOS або через Homebrew
3. **DBeaver** - безкоштовний та потужний клієнт
4. **Портативність** - файл БД можна легко переносити
5. **Швидкість** - для тестів достатньо

---

## ⚠️ Важливі примітки

1. **Файл БД:** SQLite створює файл `CalculatorTestDB.db` у вказаній папці
2. **Шлях:** Можна використовувати відносний або абсолютний шлях
3. **NuGet пакет:** Потрібно встановити `System.Data.SQLite.Core`
4. **Типи даних:** SQLite використовує `INTEGER` замість `BIT` (0 або 1)

---

**Готово!** Тепер все працює на macOS з SQLite! 🎉
