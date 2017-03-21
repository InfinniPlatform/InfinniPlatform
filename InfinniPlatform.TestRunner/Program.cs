using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using NUnitLite;

namespace InfinniPlatform.TestRunner
{
    public class Program
    {
        public static int Main()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            var executingAssembly = typeof(Program).GetTypeInfo().Assembly;
            var currentDirectory = Path.GetDirectoryName(executingAssembly.Location);
            Directory.SetCurrentDirectory(currentDirectory);

            var success = true;

            // Finds test assemblies
            var testAssemblies = Directory.GetFiles(currentDirectory, "*.Tests.dll");

            foreach (var testAssembly in testAssemblies)
            {
                var testAssemblyFile = Path.GetFileName(testAssembly);
                var testAssemblyResult = Path.GetFileNameWithoutExtension(testAssemblyFile) + ".TestResult.xml";

                var nUnitLiteArgs = new List<string>();

                nUnitLiteArgs.Add(testAssemblyFile);

                // Defines test scope by using https://github.com/nunit/docs/wiki/Test-Selection-Language
                //nUnitLiteArgs.AddRange(new[] { "--where", "cat==UnitTest" });
                //nUnitLiteArgs.AddRange(new[] { "--where", "method==UniqueTestMethod" });
                //nUnitLiteArgs.AddRange(new[] { "--where", "test==Namespace.TestClass.TestMethod" });

                // Defines the testing result file
                nUnitLiteArgs.Add($"--result={testAssemblyResult}");

                var result = new AutoRun(null).Execute(nUnitLiteArgs.ToArray());

                success &= (result == 0);
            }

            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("All tests successfully passed");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Some tests passed with errors");
                Console.ResetColor();
            }

#if DEBUG
            Console.ReadLine();
#endif

            return success ? 0 : 1;
        }
    }
}