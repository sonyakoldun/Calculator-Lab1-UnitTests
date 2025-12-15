-- =============================================
-- Скрипт створення бази даних SQLite для модульних тестів
-- Метод: RunEstimate з AnalizerClassLibrary
-- Використання: DBeaver або інший SQLite клієнт
-- =============================================

-- Видалення таблиці, якщо вона існує
DROP TABLE IF EXISTS TestExpressions;

-- Створення таблиці для тестових даних
CREATE TABLE TestExpressions
(
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    TestName TEXT NOT NULL,
    Expression TEXT NOT NULL,
    ExpectedResult TEXT NOT NULL,
    IsError INTEGER NOT NULL,  -- SQLite використовує INTEGER замість BIT (0 або 1)
    ErrorCode TEXT NULL,
    Description TEXT NULL,
    TestCategory TEXT NOT NULL  -- 'Valid', 'DivisionByZero', 'Overflow', 'TooManyOperands', 'EdgeCase'
);

-- =============================================
-- Вставка тестових даних
-- =============================================

-- Коректні вирази (Valid)
INSERT INTO TestExpressions (TestName, Expression, ExpectedResult, IsError, ErrorCode, Description, TestCategory)
VALUES
    -- Прості арифметичні операції
    ('SimpleAddition', '2+2', '4', 0, NULL, 'Просте додавання', 'Valid'),
    ('SimpleSubtraction', '5-3', '2', 0, NULL, 'Просте віднімання', 'Valid'),
    ('SimpleMultiplication', '4*3', '12', 0, NULL, 'Просте множення', 'Valid'),
    ('SimpleDivision', '10/2', '5', 0, NULL, 'Просте ділення', 'Valid'),
    ('SimpleModulo', '10%3', '1', 0, NULL, 'Простий модуль', 'Valid'),
    
    -- Вирази з дужками
    ('ExpressionWithBrackets', '(2+3)*4', '20', 0, NULL, 'Вирази з дужками', 'Valid'),
    ('NestedBrackets', '((2+3)*4)-1', '19', 0, NULL, 'Вкладені дужки', 'Valid'),
    ('ComplexExpression1', '(10+5)*2-3', '27', 0, NULL, 'Складний вираз 1', 'Valid'),
    ('ComplexExpression2', '2*(3+4)-5', '9', 0, NULL, 'Складний вираз 2', 'Valid'),
    
    -- Вирази з унарними операторами
    ('UnaryMinus', 'm5', '-5', 0, NULL, 'Унарний мінус', 'Valid'),
    ('UnaryPlus', 'p5', '5', 0, NULL, 'Унарний плюс', 'Valid'),
    ('UnaryMinusInExpression', 'm5+3', '-2', 0, NULL, 'Унарний мінус у виразі', 'Valid'),
    ('UnaryPlusInExpression', 'p5+3', '8', 0, NULL, 'Унарний плюс у виразі', 'Valid'),
    ('UnaryMinusWithBrackets', '(m5+3)*2', '-4', 0, NULL, 'Унарний мінус з дужками', 'Valid'),
    
    -- Складні вирази
    ('ComplexWithAllOperators', '10+5*2-3/1', '17', 0, NULL, 'Всі оператори', 'Valid'),
    ('ModuloComplex', '20%7+3', '6', 0, NULL, 'Модуль у складному виразі', 'Valid'),
    ('LargeNumbers', '1000+2000', '3000', 0, NULL, 'Великі числа', 'Valid'),
    ('NegativeResult', '5-10', '-5', 0, NULL, 'Негативний результат', 'Valid'),
    
    -- Граничні випадки (EdgeCase)
    ('SingleNumber', '42', '42', 0, NULL, 'Один елемент у черзі', 'EdgeCase'),
    ('Zero', '0', '0', 0, NULL, 'Нуль', 'EdgeCase'),
    ('OnePlusOne', '1+1', '2', 0, NULL, '1+1', 'EdgeCase'),
    ('MaxIntValue', '2147483647', '2147483647', 0, NULL, 'Максимальне значення int', 'EdgeCase'),
    ('MinIntValue', 'm2147483648', '-2147483648', 0, NULL, 'Мінімальне значення int (через унарний мінус)', 'EdgeCase');

-- Помилки ділення на 0 (DivisionByZero)
INSERT INTO TestExpressions (TestName, Expression, ExpectedResult, IsError, ErrorCode, Description, TestCategory)
VALUES
    ('DivisionByZero1', '10/0', '&Error 09 – Помилка ділення на 0.', 1, 'ERROR_09', 'Ділення на 0', 'DivisionByZero'),
    ('DivisionByZero2', '5/0', '&Error 09 – Помилка ділення на 0.', 1, 'ERROR_09', 'Ділення на 0', 'DivisionByZero'),
    ('ModuloByZero', '10%0', '&Error 09 – Помилка ділення на 0.', 1, 'ERROR_09', 'Модуль на 0', 'DivisionByZero'),
    ('ComplexDivisionByZero', '(10+5)/0', '&Error 09 – Помилка ділення на 0.', 1, 'ERROR_09', 'Складний вираз з діленням на 0', 'DivisionByZero');

-- Помилки переповнення (Overflow)
INSERT INTO TestExpressions (TestName, Expression, ExpectedResult, IsError, ErrorCode, Description, TestCategory)
VALUES
    ('OverflowAddition', '2147483647+1', '&Error 06 — Дуже мале, або дуже велике значення числа для int. Числа повинні бути в межах від -2 147 483 648 до 2 147 483 647.', 1, 'ERROR_06', 'Переповнення при додаванні', 'Overflow'),
    ('OverflowMultiplication', '2147483647*2', '&Error 06 — Дуже мале, або дуже велике значення числа для int. Числа повинні бути в межах від -2 147 483 648 до 2 147 483 647.', 1, 'ERROR_06', 'Переповнення при множенні', 'Overflow'),
    ('OverflowSubtraction', '-2147483648-1', '&Error 06 — Дуже мале, або дуже велике значення числа для int. Числа повинні бути в межах від -2 147 483 648 до 2 147 483 647.', 1, 'ERROR_06', 'Переповнення при відніманні', 'Overflow');

-- Помилка занадто багато операндів (TooManyOperands)
INSERT INTO TestExpressions (TestName, Expression, ExpectedResult, IsError, ErrorCode, Description, TestCategory)
VALUES
    ('TooManyOperands', '1+2+3+4+5+6+7+8+9+10+11+12+13+14+15+16+17+18+19+20+21+22+23+24+25+26+27+28+29+30+31', '&Error 08 — Сумарна кількість чисел і операторів перевищує 30.', 1, 'ERROR_08', 'Занадто багато операндів', 'TooManyOperands');

-- Створення індексів для швидкого пошуку
CREATE INDEX IF NOT EXISTS IX_TestExpressions_Category ON TestExpressions(TestCategory);
CREATE INDEX IF NOT EXISTS IX_TestExpressions_IsError ON TestExpressions(IsError);

-- Виведення статистики
SELECT 'База даних CalculatorTestDB створена успішно!' AS Message;
SELECT 'Кількість тестових записів: ' || COUNT(*) AS Statistics FROM TestExpressions;