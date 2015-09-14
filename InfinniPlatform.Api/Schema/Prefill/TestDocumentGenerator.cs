using System;
using System.Collections.Generic;
using System.Threading;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.Api.Schema.Prefill
{
    public sealed class TestDocumentGenerator
    {
        private readonly string _configId;
        private readonly string _documentId;
        private readonly TestDataBuilder _testDataBuilder;

        public TestDocumentGenerator(string configId, string documentId)
        {
            _configId = configId;
            _documentId = documentId;
            _testDataBuilder = new TestDataBuilder(configId);
        }

        public TestDataBuilder TestDataBuilder
        {
            get { return _testDataBuilder; }
        }

        public void GenerateTestDocument(int countDocumentsToInsert)
        {
            var threadsCount = 0;
            if (countDocumentsToInsert < 100)
            {
                threadsCount = 1;
            }
            else
            {
                threadsCount = 5;
            }

            var docInThread = countDocumentsToInsert/threadsCount;

            ThreadPool.SetMaxThreads(threadsCount, threadsCount);

            var waitHandles = new List<EventWaitHandle>();

            var lockObj = new object();
            var waitHandleCount = 0;
            for (var threadNum = 0; threadNum < threadsCount; threadNum++)
            {
                ThreadPool.QueueUserWorkItem(data =>
                {
                    var items = new List<dynamic>();
                    var waitHandle = new ManualResetEvent(false);
                    try
                    {
                        lock (lockObj)
                        {
                            waitHandles.Add(waitHandle);
                            Interlocked.Increment(ref waitHandleCount);
                        }

                        for (var i = 0; i < docInThread; i++)
                        {
                            dynamic instance = new DocumentApi().CreateDocument(_configId, _documentId);
                            if (instance.IsValid == false)
                            {
                                throw new ArgumentException(string.Format("try create invalid instance. Error: {0}",
                                    instance.ValidationMessage));
                            }

                            TestDataBuilder.Build(instance);
                            items.Add(instance);
                        }

                        new DocumentApi().SetDocuments(_configId, _documentId, items);
                    }
                    finally
                    {
                        waitHandle.Set();
                    }
                });
            }

            while (waitHandleCount != threadsCount)
            {
                Thread.Sleep(100);
            }


            var waitHandlesArray = waitHandles.ToArray();
            WaitHandle.WaitAll(waitHandlesArray);
        }
    }
}