using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using NUnitLite;

namespace InfinniPlatform.TestRunner
{
    public class Program
    {
        private const string TestResultSuffix = ".TestResult.xml";


        public static int Main(string[] args)
        {
            var result = 0;

            Console.OutputEncoding = System.Text.Encoding.Unicode;

            var testCategory = GetTestCategory(args);
            var outDirectory = GetOutDirectory();
            var testAssemblies = FindTestAssemblies(outDirectory);

            foreach (var testAssembly in testAssemblies)
            {
                var testAssemblyResult = Path.Combine(outDirectory, Path.GetFileNameWithoutExtension(testAssembly) + TestResultSuffix);

                result |= RunTests(testCategory, testAssembly, testAssemblyResult);
            }

            Console.WriteLine();

            if (result == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("All tests successfully passed".ToUpper());
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Some tests passed with errors".ToUpper());
            }

            Console.ResetColor();

#if DEBUG
            Console.ReadLine();
#endif

            return result;
        }


        private static string GetTestCategory(string[] args)
        {
            return (args != null && args.Length > 0) ? args[0] : null;
        }

        private static string GetOutDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        private static IEnumerable<string> FindTestAssemblies(string outputDirectory)
        {
            return Directory.GetFiles(outputDirectory, "*.Tests.dll");
        }

        private static int RunTests(string testCategory, string testAssembly, string testAssemblyResult)
        {
            var nUnitLiteArgs = new List<string> { testAssembly, "--noheader" };

            if (!string.IsNullOrWhiteSpace(testCategory))
            {
                nUnitLiteArgs.AddRange(new[] { "--where", $"cat=={testCategory}" });
            }

            // Defines test scope by using https://github.com/nunit/docs/wiki/Test-Selection-Language
            //nUnitLiteArgs.AddRange(new[] { "--where", "cat==UnitTest" });
            //nUnitLiteArgs.AddRange(new[] { "--where", "method==UniqueTestMethod" });
            //nUnitLiteArgs.AddRange(new[] { "--where", "test==Namespace.TestClass.TestMethod" });

            // Defines the testing result file
            nUnitLiteArgs.Add($"--result={testAssemblyResult}");

            return new AutoRun(null).Execute(nUnitLiteArgs.ToArray());
        }
    }
}