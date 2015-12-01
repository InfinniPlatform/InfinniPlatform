using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Api.RestApi.CommonApi.RouteTraces;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace InfinniPlatform.Api.SelfDocumentation
{
    public static class DocumentationKeeperBuilder
    {
        private static readonly RouteTraceSaveQueryLog InnerTracer = new RouteTraceSaveQueryLog();

        public static RouteTraceSaveQueryLog Tracer
        {
            get { return InnerTracer; }
        }

        public static DocumentationKeeper Build(
            string helpPath,
            string assemblyPath,
            IDocumentationFormatter documentationFormatter)
        {
            var documentationKeeper = new DocumentationKeeper(helpPath, documentationFormatter);

            var assembly = Assembly.Load(
                new AssemblyName
                {
                    CodeBase = assemblyPath
                });

            var testClasses = assembly.GetTypes().Where(
                t => t.IsDefined(typeof (TestFixtureAttribute)) &&
                     !t.IsDefined(typeof (IgnoreAttribute)));

            foreach (var testClass in testClasses)
            {
                var setUpMethod =
                    testClass.GetMethods().FirstOrDefault(m => m.IsDefined(typeof (TestFixtureSetUpAttribute)));

                var tearDownMethod =
                    testClass.GetMethods().FirstOrDefault(m => m.IsDefined(typeof (TestFixtureTearDownAttribute)));

                var documentedTests =
                    testClass.GetMethods()
                        .Where(
                            m =>
                                m.IsDefined(typeof (SelfDocumentedTestAttribute)) &&
                                !m.IsDefined(typeof (IgnoreAttribute))).ToArray();

                var testClassObject = Activator.CreateInstance(testClass);

                if (setUpMethod != null && documentedTests.Any())
                {
                    setUpMethod.Invoke(testClassObject, new object[0]);
                }

                foreach (var documentedTest in documentedTests)
                {
                    InnerTracer.ClearQueries();

                    try
                    {
                        documentedTest.Invoke(testClassObject, new object[0]);
                    }
                    catch (TargetInvocationException e)
                    {
                        // Извлекаем более "понятное" сообщение
                        throw new Exception(e.InnerException.ToString());
                    }

                    var docInfos = documentedTest.GetCustomAttributes(typeof (SelfDocumentedTestAttribute)) as
                        IEnumerable<SelfDocumentedTestAttribute>;

                    if (docInfos != null)
                    {
                        foreach (var docInfo in docInfos)
                        {
                            foreach (var data in InnerTracer.GetCatchedData())
                            {
                                documentationKeeper.AddHelpInfo(
                                    new RestQueryInfo
                                    {
                                        Configuration = data.Configuration,
                                        Metadata = data.Metadata,
                                        Action = data.Action,
                                        Comment = docInfo.Comment,
                                        Body = data.Body,
                                        QueryType = data.QueryType,
                                        ResponceContent = JToken.Parse(data.ResponseContent).ToString(),
                                        Url = data.Url,
                                        ExampleSource =
                                            string.Format("Assembly: {0}\n\rTest class: {1}\n\rTest name: {2}",
                                                Path.GetFileName(assemblyPath), testClass.Name, documentedTest.Name)
                                    });
                            }
                        }
                    }
                }

                if (tearDownMethod != null && documentedTests.Any())
                {
                    tearDownMethod.Invoke(testClassObject, new object[0]);
                }
            }

            return documentationKeeper;
        }
    }
}