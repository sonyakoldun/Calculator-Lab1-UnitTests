using System;
using System.IO;
using Mono.Data.Sqlite;
using AnalaizerClassLibrary;

namespace ManualTestRunnerFx
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                // Вимикаємо UI-повідомлення в бібліотеці
                AnalaizerClass.ShowMessage = false;

                string solutionRoot = FindSolutionRoot();
                string dbPath = Path.Combine(solutionRoot, "Database", "calculator_test.db");

                if (!File.Exists(dbPath))
                {
                    Console.WriteLine("❌ Database file not found: " + dbPath);
                    return 2;
                }

                string cs = $"URI=file:{dbPath}"; // формат для Mono.Data.Sqlite

                int total = 0, passed = 0, failed = 0;

                using (var conn = new SqliteConnection(cs))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText =
                            "SELECT TestName, Expression, ExpectedResult, IsError, Description, TestCategory " +
                            "FROM TestExpressions ORDER BY Id";

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                total++;
                                string testName = reader["TestName"]?.ToString() ?? $"Test_{total}";
                                string expression = reader["Expression"]?.ToString() ?? "";
                                string expected = reader["ExpectedResult"]?.ToString() ?? "";
                                bool isError = ToBool(reader["IsError"]);
                                string category = reader["TestCategory"]?.ToString() ?? "";
                                string descr = reader["Description"] == DBNull.Value ? "" : reader["Description"]?.ToString() ?? "";

                                bool ok = RunSingle(testName, expression, expected, isError, category, descr, out string actualOrError);

                                if (ok) passed++;
                                else
                                {
                                    failed++;
                                    Console.WriteLine($"❌ FAIL: {testName} [{category}] {descr}");
                                    Console.WriteLine($"   expr: {expression}");
                                    Console.WriteLine($"   expected: {expected}");
                                    Console.WriteLine($"   actual:   {actualOrError}");
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("=================================");
                Console.WriteLine($"TOTAL:  {total}");
                Console.WriteLine($"PASSED: {passed}");
                Console.WriteLine($"FAILED: {failed}");
                Console.WriteLine("=================================");

                return failed == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("💥 Runner crashed: " + ex);
                return 3;
            }
        }

        private static bool RunSingle(
            string testName,
            string inputExpression,
            string expectedResult,
            bool isError,
            string category,
            string description,
            out string actualResult)
        {
            // 1) PrepareExpression як у твоєму MSTest файлі
            string prepared = PrepareExpression(inputExpression);

            // Якщо на етапі підготовки вже помилка
            if (prepared == null || prepared.StartsWith("&"))
            {
                if (isError)
                {
                    // очікували помилку — перевіряємо, що хоч якось співпадає
                    actualResult = prepared ?? "null";
                    return CompareError(expectedResult, actualResult);
                }

                actualResult = prepared ?? "null";
                return false;
            }

            // 2) RunEstimate
            AnalaizerClass.expression = prepared;
            string actual = AnalaizerClass.RunEstimate();

            // 3) Check
            if (isError)
            {
                actualResult = actual;
                return CompareError(expectedResult, actual);
            }
            else
            {
                actualResult = actual;
                return string.Equals(expectedResult, actual, StringComparison.Ordinal);
            }
        }

        private static string PrepareExpression(string inputExpression)
        {
            AnalaizerClass.expression = inputExpression;

            if (!AnalaizerClass.CheckCurrency())
                return null;

            AnalaizerClass.expression = AnalaizerClass.ReplaceUnaryPlusMinus(AnalaizerClass.expression);

            string formatted = AnalaizerClass.Format();
            return formatted;
        }

        private static bool CompareError(string expected, string actual)
        {
            if (string.IsNullOrEmpty(actual) || !actual.StartsWith("&"))
                return false;

            if (string.IsNullOrEmpty(expected))
                return true;

            // Легка перевірка: щоб у тексті помилки був код/фрагмент очікуваного
            string exp = expected.Replace("&", "").Trim();
            string act = actual.Replace("&", "").Trim();

            if (exp.Length == 0) return true;

            string needle = exp.Substring(0, Math.Min(15, exp.Length));
            return act.Contains(needle);
        }

        private static bool ToBool(object dbValue)
        {
            if (dbValue == null || dbValue == DBNull.Value) return false;

            if (dbValue is long l) return l != 0;
            if (dbValue is int i) return i != 0;

            // якщо зберіглось як "0"/"1"
            if (bool.TryParse(dbValue.ToString(), out bool b)) return b;
            if (int.TryParse(dbValue.ToString(), out int n)) return n != 0;

            return false;
        }

        private static string FindSolutionRoot()
        {
            // стартуємо з папки, де лежить exe
            string dir = AppDomain.CurrentDomain.BaseDirectory;

            // підіймаємось вгору, доки не знайдемо папку Database або .sln
            for (int i = 0; i < 8; i++)
            {
                string candidateDb = Path.Combine(dir, "Database");
                string candidateSln = Path.Combine(dir, "Calculator_Exam_CommandProject.sln");

                if (Directory.Exists(candidateDb) || File.Exists(candidateSln))
                    return dir;

                dir = Path.GetFullPath(Path.Combine(dir, ".."));
            }

            // fallback: корінь репозиторію як у тебе зараз
            return Directory.GetCurrentDirectory();
        }
    }
}