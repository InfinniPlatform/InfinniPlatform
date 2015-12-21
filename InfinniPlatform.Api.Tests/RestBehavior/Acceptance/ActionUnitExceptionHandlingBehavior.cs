﻿using System;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Api;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ActionUnitExceptionHandlingBehavior
    {
        private const string ConfigurationId = "TestConfiguration";
        private const string DocumentType = "ExceptionHandlingDocument";

        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = InfinniPlatformInprocessHost.Start();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldFormatExceptionMessage()
        {
            // Given
            var documentApi = new InfinniDocumentApi(HostingConfig.Default.Name, HostingConfig.Default.Port);
            var document = new { Id = Guid.NewGuid(), LastName = "123" };

            // When
            var result = Assert.Catch(() => documentApi.SetDocument(ConfigurationId, DocumentType, document));

            // Then
            Assert.IsTrue(result.Message.Contains("Important exception details"));
        }
    }
}