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

            var nUnitLiteArgs = new List<string>();

            // Defines test assemblies

            var testAssemblies = Directory.GetFiles(currentDirectory, "*.Tests.dll");

            foreach (var testAssembly in testAssemblies)
            {
                nUnitLiteArgs.Add(Path.GetFileName(testAssembly));
            }

            // Defines test scope by using https://github.com/nunit/docs/wiki/Test-Selection-Language
            //nUnitLiteArgs.AddRange(new[] { "--where", "cat==UnitTest" });
            //nUnitLiteArgs.AddRange(new[] { "--where", "method==UniqueTestMethod" });
            //nUnitLiteArgs.AddRange(new[] { "--where", "test==Namespace.TestClass.TestMethod" });

            // Defines the testing result file
            nUnitLiteArgs.Add($"--result={Path.Combine(currentDirectory, "TestResult.xml")}");

            var result = new AutoRun(null).Execute(nUnitLiteArgs.ToArray());

#if DEBUG
            Console.ReadLine();
#endif

            return result;
        }
    }
}