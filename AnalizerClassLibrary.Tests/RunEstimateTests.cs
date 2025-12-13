using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnalaizerClassLibrary;

namespace AnalizerClassLibrary.Tests
{
    /// <summary>
    /// Модульні тести для методу RunEstimate з класу AnalaizerClass
    /// Варіант 9: бібліотека AnalaizerClassLibrary, метод RunEstimate
    /// </summary>
    [TestClass]
    public class RunEstimateTests
    {
        private static string connectionString;

        /// <summary>
        /// Ініціалізація перед усіма тестами - отримання рядка підключення до БД
        /// </summary>
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Отримуємо рядок підключення з App.config
            connectionString = ConfigurationManager.ConnectionStrings["TestDatabase"]?.ConnectionString;
            
            if (string.IsNullOrEmpty(connectionString))
            {
                // Якщо немає в config, використовуємо стандартний шлях до SQLite файлу
                // Шукаємо файл БД у папці з проєктом або в корені solution
                string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "CalculatorTestDB.db");
                if (!File.Exists(dbPath))
                {
                    // Спробуємо знайти в папці Database
                    dbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Database", "CalculatorTestDB.db");
                }
                connectionString = $"Data Source={dbPath};Version=3;";
            }

            // Вимкнути показ повідомлень про помилки (для тестів)
            AnalaizerClass.ShowMessage = false;
        }

        /// <summary>
        /// Очищення після кожного тесту
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            // Очищаємо expression після кожного тесту
            AnalaizerClass.expression = "";
        }

        /// <summary>
        /// Допоміжний метод для підготовки виразу перед викликом RunEstimate
        /// RunEstimate викликає CreateStack(), який потребує відформатований вираз
        /// </summary>
        private string PrepareExpression(string inputExpression)
        {
            // Встановлюємо expression
            AnalaizerClass.expression = inputExpression;

            // Перевіряємо валідність дужок
            if (!AnalaizerClass.CheckCurrency())
            {
                return null; // Помилка в дужках
            }

            // Замінюємо унарні оператори
            AnalaizerClass.expression = AnalaizerClass.ReplaceUnaryPlusMinus(AnalaizerClass.expression);

            // Форматуємо вираз
            string formatted = AnalaizerClass.Format();
            
            if (string.IsNullOrEmpty(formatted) || formatted.StartsWith("&"))
            {
                return formatted; // Повертаємо помилку або порожній рядок
            }

            return formatted;
        }

        /// <summary>
        /// Тест на основі даних з бази даних
        /// Читає всі тестові випадки з БД і перевіряє їх
        /// </summary>
        [TestMethod]
        public void RunEstimate_AllTestsFromDatabase()
        {
            // Arrange
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TestName, Expression, ExpectedResult, IsError, ErrorCode, Description, TestCategory FROM TestExpressions ORDER BY Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        int testCount = 0;
                        int passedCount = 0;
                        int failedCount = 0;

                        while (reader.Read())
                        {
                            testCount++;
                            string testName = reader["TestName"].ToString();
                            string expression = reader["Expression"].ToString();
                            string expectedResult = reader["ExpectedResult"].ToString();
                            bool isError = Convert.ToBoolean(reader["IsError"]); // SQLite повертає INTEGER (0/1)
                            string description = reader["Description"].ToString();
                            string category = reader["TestCategory"].ToString();

                            try
                            {
                                // Act
                                string preparedExpression = PrepareExpression(expression);
                                
                                if (preparedExpression == null || preparedExpression.StartsWith("&"))
                                {
                                    // Якщо підготовка не вдалася (помилка форматування)
                                    if (isError)
                                    {
                                        // Очікувалась помилка - перевіряємо, чи відповідає результат
                                        if (preparedExpression != null && preparedExpression.Contains(expectedResult.Replace("&", "").Substring(0, 10)))
                                        {
                                            passedCount++;
                                            continue;
                                        }
                                    }
                                    // Якщо не очікувалась помилка, але вона виникла
                                    Assert.Fail($"Test '{testName}' failed: Expected '{expectedResult}', but got '{preparedExpression}'");
                                }

                                // Встановлюємо відформатований вираз
                                AnalaizerClass.expression = preparedExpression;
                                
                                // Викликаємо RunEstimate
                                string actualResult = AnalaizerClass.RunEstimate();

                                // Assert
                                if (isError)
                                {
                                    // Очікується помилка
                                    Assert.IsTrue(actualResult.StartsWith("&"), 
                                        $"Test '{testName}': Expected error, but got result: {actualResult}");
                                    
                                    // Перевіряємо код помилки
                                    if (!string.IsNullOrEmpty(expectedResult))
                                    {
                                        string expectedError = expectedResult.Replace("&", "").Trim();
                                        string actualError = actualResult.Replace("&", "").Trim();
                                        
                                        // Перевіряємо, чи містить результат очікуваний код помилки
                                        Assert.IsTrue(actualError.Contains(expectedError.Substring(0, Math.Min(15, expectedError.Length))), 
                                            $"Test '{testName}': Expected error '{expectedError}', but got '{actualError}'");
                                    }
                                }
                                else
                                {
                                    // Очікується коректний результат
                                    Assert.AreEqual(expectedResult, actualResult, 
                                        $"Test '{testName}' ({description}): Expected '{expectedResult}', but got '{actualResult}'");
                                }

                                passedCount++;
                            }
                            catch (AssertFailedException)
                            {
                                failedCount++;
                                throw; // Перекидаємо AssertFailedException далі
                            }
                            catch (Exception ex)
                            {
                                failedCount++;
                                Assert.Fail($"Test '{testName}' threw unexpected exception: {ex.Message}\nStack trace: {ex.StackTrace}");
                            }
                        }

                        // Виводимо статистику
                        System.Diagnostics.Debug.WriteLine($"\n=== Test Statistics ===");
                        System.Diagnostics.Debug.WriteLine($"Total tests: {testCount}");
                        System.Diagnostics.Debug.WriteLine($"Passed: {passedCount}");
                        System.Diagnostics.Debug.WriteLine($"Failed: {failedCount}");
                    }
                }
            }
        }

        /// <summary>
        /// Тест для коректних виразів (категорія Valid)
        /// </summary>
        [TestMethod]
        public void RunEstimate_ValidExpressions()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TestName, Expression, ExpectedResult, Description FROM TestExpressions WHERE TestCategory = 'Valid' ORDER BY Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string testName = reader["TestName"].ToString();
                            string expression = reader["Expression"].ToString();
                            string expectedResult = reader["ExpectedResult"].ToString();
                            string description = reader["Description"].ToString();

                            // Arrange & Act
                            string preparedExpression = PrepareExpression(expression);
                            Assert.IsNotNull(preparedExpression, $"Failed to prepare expression for test '{testName}'");
                            Assert.IsFalse(preparedExpression.StartsWith("&"), $"Format error for test '{testName}': {preparedExpression}");

                            AnalaizerClass.expression = preparedExpression;
                            string actualResult = AnalaizerClass.RunEstimate();

                            // Assert
                            Assert.AreEqual(expectedResult, actualResult, 
                                $"Test '{testName}' ({description}): Expected '{expectedResult}', but got '{actualResult}'");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Тест для помилок ділення на 0
        /// </summary>
        [TestMethod]
        public void RunEstimate_DivisionByZero()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TestName, Expression, ExpectedResult, Description FROM TestExpressions WHERE TestCategory = 'DivisionByZero' ORDER BY Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string testName = reader["TestName"].ToString();
                            string expression = reader["Expression"].ToString();
                            string expectedResult = reader["ExpectedResult"].ToString();
                            string description = reader["Description"].ToString();

                            // Arrange & Act
                            string preparedExpression = PrepareExpression(expression);
                            Assert.IsNotNull(preparedExpression, $"Failed to prepare expression for test '{testName}'");
                            Assert.IsFalse(preparedExpression.StartsWith("&"), $"Format error for test '{testName}': {preparedExpression}");

                            AnalaizerClass.expression = preparedExpression;
                            string actualResult = AnalaizerClass.RunEstimate();

                            // Assert
                            Assert.IsTrue(actualResult.StartsWith("&"), 
                                $"Test '{testName}': Expected error, but got result: {actualResult}");
                            Assert.IsTrue(actualResult.Contains("Error 09"), 
                                $"Test '{testName}': Expected ERROR_09, but got: {actualResult}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Тест для помилок переповнення
        /// </summary>
        [TestMethod]
        public void RunEstimate_Overflow()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TestName, Expression, ExpectedResult, Description FROM TestExpressions WHERE TestCategory = 'Overflow' ORDER BY Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string testName = reader["TestName"].ToString();
                            string expression = reader["Expression"].ToString();
                            string expectedResult = reader["ExpectedResult"].ToString();
                            string description = reader["Description"].ToString();

                            // Arrange & Act
                            string preparedExpression = PrepareExpression(expression);
                            Assert.IsNotNull(preparedExpression, $"Failed to prepare expression for test '{testName}'");
                            Assert.IsFalse(preparedExpression.StartsWith("&"), $"Format error for test '{testName}': {preparedExpression}");

                            AnalaizerClass.expression = preparedExpression;
                            string actualResult = AnalaizerClass.RunEstimate();

                            // Assert
                            Assert.IsTrue(actualResult.StartsWith("&"), 
                                $"Test '{testName}': Expected error, but got result: {actualResult}");
                            Assert.IsTrue(actualResult.Contains("Error 06"), 
                                $"Test '{testName}': Expected ERROR_06, but got: {actualResult}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Тест для помилки занадто багато операндів
        /// </summary>
        [TestMethod]
        public void RunEstimate_TooManyOperands()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TestName, Expression, ExpectedResult, Description FROM TestExpressions WHERE TestCategory = 'TooManyOperands' ORDER BY Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string testName = reader["TestName"].ToString();
                            string expression = reader["Expression"].ToString();
                            string expectedResult = reader["ExpectedResult"].ToString();
                            string description = reader["Description"].ToString();

                            // Arrange & Act
                            string preparedExpression = PrepareExpression(expression);
                            Assert.IsNotNull(preparedExpression, $"Failed to prepare expression for test '{testName}'");
                            Assert.IsFalse(preparedExpression.StartsWith("&"), $"Format error for test '{testName}': {preparedExpression}");

                            AnalaizerClass.expression = preparedExpression;
                            string actualResult = AnalaizerClass.RunEstimate();

                            // Assert
                            Assert.IsTrue(actualResult.StartsWith("&"), 
                                $"Test '{testName}': Expected error, but got result: {actualResult}");
                            Assert.IsTrue(actualResult.Contains("Error 08"), 
                                $"Test '{testName}': Expected ERROR_08, but got: {actualResult}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Тест для граничних випадків
        /// </summary>
        [TestMethod]
        public void RunEstimate_EdgeCases()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TestName, Expression, ExpectedResult, Description FROM TestExpressions WHERE TestCategory = 'EdgeCase' ORDER BY Id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string testName = reader["TestName"].ToString();
                            string expression = reader["Expression"].ToString();
                            string expectedResult = reader["ExpectedResult"].ToString();
                            string description = reader["Description"].ToString();

                            // Arrange & Act
                            string preparedExpression = PrepareExpression(expression);
                            Assert.IsNotNull(preparedExpression, $"Failed to prepare expression for test '{testName}'");
                            Assert.IsFalse(preparedExpression.StartsWith("&"), $"Format error for test '{testName}': {preparedExpression}");

                            AnalaizerClass.expression = preparedExpression;
                            string actualResult = AnalaizerClass.RunEstimate();

                            // Assert
                            Assert.AreEqual(expectedResult, actualResult, 
                                $"Test '{testName}' ({description}): Expected '{expectedResult}', but got '{actualResult}'");
                        }
                    }
                }
            }
        }

        // ============================================
        // Додаткові unit тести без використання БД
        // ============================================

        /// <summary>
        /// Тест: просте додавання
        /// </summary>
        [TestMethod]
        public void RunEstimate_SimpleAddition_ReturnsCorrectResult()
        {
            // Arrange
            string preparedExpression = PrepareExpression("2+2");
            AnalaizerClass.expression = preparedExpression;

            // Act
            string result = AnalaizerClass.RunEstimate();

            // Assert
            Assert.AreEqual("4", result);
        }

        /// <summary>
        /// Тест: вираз з одним елементом (queue.Count == 1)
        /// </summary>
        [TestMethod]
        public void RunEstimate_SingleElement_ReturnsElement()
        {
            // Arrange
            string preparedExpression = PrepareExpression("42");
            AnalaizerClass.expression = preparedExpression;

            // Act
            string result = AnalaizerClass.RunEstimate();

            // Assert
            Assert.AreEqual("42", result);
        }

        /// <summary>
        /// Тест: ділення на 0
        /// </summary>
        [TestMethod]
        public void RunEstimate_DivisionByZero_ReturnsError09()
        {
            // Arrange
            string preparedExpression = PrepareExpression("10/0");
            AnalaizerClass.expression = preparedExpression;

            // Act
            string result = AnalaizerClass.RunEstimate();

            // Assert
            Assert.IsTrue(result.StartsWith("&"));
            Assert.IsTrue(result.Contains("Error 09"));
        }

        /// <summary>
        /// Тест: унарний мінус
        /// </summary>
        [TestMethod]
        public void RunEstimate_UnaryMinus_ReturnsNegative()
        {
            // Arrange
            string preparedExpression = PrepareExpression("m5");
            AnalaizerClass.expression = preparedExpression;

            // Act
            string result = AnalaizerClass.RunEstimate();

            // Assert
            Assert.AreEqual("-5", result);
        }

        /// <summary>
        /// Тест: складний вираз з дужками
        /// </summary>
        [TestMethod]
        public void RunEstimate_ComplexExpressionWithBrackets_ReturnsCorrectResult()
        {
            // Arrange
            string preparedExpression = PrepareExpression("(2+3)*4");
            AnalaizerClass.expression = preparedExpression;

            // Act
            string result = AnalaizerClass.RunEstimate();

            // Assert
            Assert.AreEqual("20", result);
        }
    }
}
