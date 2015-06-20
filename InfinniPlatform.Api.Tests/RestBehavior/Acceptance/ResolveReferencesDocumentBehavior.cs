using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Sdk.Application.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ResolveReferencesDocumentBehavior
    {
        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

            TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void ShouldResolveReferencesLessThan150ms(bool resolve, bool inline)
        {
            //Given


            MetadataManagerConfiguration managerConfiguration =
                ManagerFactoryConfiguration.BuildConfigurationManager(null);
            string configurationId = "testconfigreferences";
            dynamic config = managerConfiguration.CreateItem(configurationId);
            managerConfiguration.DeleteItem(config);
            managerConfiguration.MergeItem(config);

            MetadataManagerDocument managerDocument =
                new ManagerFactoryConfiguration(null, configurationId).BuildDocumentManager();
            string patientDoc = "patient";
            string addressDoc = "address";
            string regionDoc = "region";

            new IndexApi().RebuildIndex(configurationId, patientDoc);

            dynamic metadataPatient = managerDocument.CreateItem(patientDoc);
            dynamic metadataAddress = managerDocument.CreateItem(addressDoc);
            dynamic metadataRegion = managerDocument.CreateItem(regionDoc);

            //patient document schema
            metadataPatient.Schema = new DynamicWrapper();
            metadataPatient.Schema.Type = patientDoc;
            metadataPatient.Schema.Caption = patientDoc;
            metadataPatient.Schema.Properties = new DynamicWrapper();

            metadataPatient.Schema.Properties.Name = new DynamicWrapper();
            metadataPatient.Schema.Properties.Name.Type = "String";
            metadataPatient.Schema.Properties.Name.Caption = "Patient name";

            metadataPatient.Schema.Properties.Address = new DynamicWrapper();
            metadataPatient.Schema.Properties.Address.Type = "Object";
            metadataPatient.Schema.Properties.Address.TypeInfo = new DynamicWrapper();
            metadataPatient.Schema.Properties.Address.TypeInfo.DocumentLink = new DynamicWrapper();
            metadataPatient.Schema.Properties.Address.TypeInfo.DocumentLink.ConfigId = configurationId;
            metadataPatient.Schema.Properties.Address.TypeInfo.DocumentLink.DocumentId = addressDoc;
            metadataPatient.Schema.Properties.Address.TypeInfo.DocumentLink.Resolve = resolve;
            metadataPatient.Schema.Properties.Address.Caption = "Patient address";

            //address document schema
            metadataAddress.Schema = new DynamicWrapper();
            metadataAddress.Schema.Type = addressDoc;
            metadataAddress.Schema.Caption = addressDoc;
            metadataAddress.Schema.Properties = new DynamicWrapper();

            metadataAddress.Schema.Properties.City = new DynamicWrapper();
            metadataAddress.Schema.Properties.City.Type = "String";
            metadataAddress.Schema.Properties.City.Caption = "Address city";

            metadataAddress.Schema.Properties.Region = new DynamicWrapper();
            metadataAddress.Schema.Properties.Region.Type = "Object";
            metadataAddress.Schema.Properties.Region.TypeInfo = new DynamicWrapper();
            metadataAddress.Schema.Properties.Region.TypeInfo.DocumentLink = new DynamicWrapper();
            metadataAddress.Schema.Properties.Region.TypeInfo.DocumentLink.ConfigId = configurationId;
            metadataAddress.Schema.Properties.Region.TypeInfo.DocumentLink.DocumentId = regionDoc;
            metadataAddress.Schema.Properties.Region.TypeInfo.DocumentLink.Inline = inline;
            metadataAddress.Schema.Properties.Region.TypeInfo.DocumentLink.Resolve = resolve;

            metadataAddress.Schema.Properties.PreviousRegions = new DynamicWrapper();
            metadataAddress.Schema.Properties.PreviousRegions.Type = "Array";
            metadataAddress.Schema.Properties.PreviousRegions.Items = new DynamicWrapper();
            metadataAddress.Schema.Properties.PreviousRegions.Items.TypeInfo = new DynamicWrapper();
            metadataAddress.Schema.Properties.PreviousRegions.Items.TypeInfo.DocumentLink = new DynamicWrapper();
            metadataAddress.Schema.Properties.PreviousRegions.Items.TypeInfo.DocumentLink.ConfigId = configurationId;
            metadataAddress.Schema.Properties.PreviousRegions.Items.TypeInfo.DocumentLink.DocumentId = regionDoc;
            metadataAddress.Schema.Properties.PreviousRegions.Items.TypeInfo.DocumentLink.Inline = inline;

            //документ-регион
            metadataRegion.Schema = new DynamicWrapper();
            metadataRegion.Schema.Type = regionDoc;
            metadataRegion.Schema.Caption = regionDoc;
            metadataRegion.Schema.Properties = new DynamicWrapper();
            metadataRegion.Schema.Properties.Id = new DynamicWrapper();
            metadataRegion.Schema.Properties.Id.Type = "Uuid";
            metadataRegion.Schema.Properties.Id.Caption = "Unique identifier";

            metadataRegion.Schema.Properties.RegionName = new DynamicWrapper();
            metadataRegion.Schema.Properties.RegionName.Type = "String";
            metadataRegion.Schema.Properties.RegionName.Caption = "City region";

            managerDocument.MergeItem(metadataPatient);
            managerDocument.MergeItem(metadataAddress);
            managerDocument.MergeItem(metadataRegion);

            RestQueryApi.QueryPostNotify(null, configurationId);

            new UpdateApi(null).UpdateStore(configurationId);

            string firstItemId = null;

            for (int i = 0; i < 100; i++)
            {
                string parentUid = Guid.NewGuid().ToString();

                string childUid = Guid.NewGuid().ToString();

                if (i == 0)
                {
                    firstItemId = childUid;
                }

                string linkInChildObjectId = Guid.NewGuid().ToString();

                string linkInChildArrayId = Guid.NewGuid().ToString();

                //главный документ
                dynamic itemDoc1 = new DynamicWrapper();
                itemDoc1.Id = parentUid;
                itemDoc1.Name = "MainDocument";
                itemDoc1.Address = new DynamicWrapper();
                itemDoc1.Address.Id = childUid;
                itemDoc1.Address.DisplayName = "ЧЕЛЯБИНСК";

                //документ по ссылке
                dynamic itemDoc2 = new DynamicWrapper();
                itemDoc2.Id = childUid;
                itemDoc2.City = "ЧЕЛЯБИНСК";

                //документ по ссылке в ссылочном документе
                dynamic itemDoc3 = new DynamicWrapper();
                itemDoc3.Id = linkInChildObjectId;
                itemDoc3.RegionName = "УРАЛЬСКИЙ РЕГИОН";

                if (inline)
                {
                    itemDoc2.Region = itemDoc3;
                }
                else
                {
                    dynamic refRegion = new DynamicWrapper();
                    refRegion.Id = linkInChildObjectId;
                    refRegion.DisplayName = "УРАЛЬСКИЙ РЕГИОН";
                    itemDoc2.Region = refRegion;
                }

                //массив документов по ссылке в ссылочном документе
                dynamic itemDoc4 = new List<dynamic>();
                itemDoc4.Add(
                    new
                        {
                            Id = linkInChildArrayId,
                            RegionName = "УРАЛЬСКИЙ РЕГИОН"
                        }.ToDynamic());

                if (inline)
                {
                    itemDoc2.PreviousRegions = itemDoc4;
                }
                else
                {
                    dynamic refRegion = new DynamicWrapper();
                    refRegion.Id = linkInChildArrayId;
                    refRegion.DisplayName = "УРАЛЬСКИЙ РЕГИОН";
                    itemDoc2.PreviousRegions = new List<dynamic>();
                    itemDoc2.PreviousRegions.Add(refRegion);
                }

                new DocumentApi(null).SetDocument(configurationId, patientDoc, itemDoc1);
                new DocumentApi(null).SetDocument(configurationId, addressDoc, itemDoc2);

                if (!inline)
                {
                    new DocumentApi(null).SetDocument(configurationId, regionDoc, itemDoc3);
                    new DocumentApi(null).SetDocument(configurationId, regionDoc, itemDoc4[0]);
                }
            }

            //Then
            dynamic document = new DocumentApi(null).GetDocument(configurationId, patientDoc,
                                                                 null, 0, 20)
                                                    .FirstOrDefault();

            //повторно для исключения времени динамической инициализации CallSite
            Stopwatch watch = Stopwatch.StartNew();
            document = new DocumentApi(null).GetDocument(configurationId, patientDoc,
                                                         null, 0, 50)
                                            .FirstOrDefault();

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

            if (!inline)
            {
                document = new DocumentApi(null).GetDocument(configurationId, patientDoc,
                                                             f =>
                                                             f.AddCriteria(
                                                                 c =>
                                                                 c.Property("Address.Region.RegionName")
                                                                  .IsEquals("УРАЛЬСКИЙ РЕГИОН")), 0, 10)
                                                .FirstOrDefault();
                Assert.IsNotNull(document);
            }

            if (resolve)
            {
                Assert.IsNotNull(document);
                Assert.IsNotNull(document.Address.City);
                Assert.IsNotNull(document.Address.PreviousRegions);
            }
            else
            {
                Assert.IsNotNull(document);
                Assert.IsNotNull(document.Address);
                Assert.IsNull(document.Address.City);
                Assert.IsNull(document.Address.Region);
                Assert.IsNull(document.Address.PreviousRegions);
            }

            if (resolve && inline)
            {
                Assert.IsNotNull(document.Address.Region.RegionName);
            }

            Console.WriteLine(watch.Elapsed);
            Assert.True(watch.ElapsedMilliseconds < 150);
        }
    }
}