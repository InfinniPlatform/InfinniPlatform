using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using DevExpress.XtraEditors.Controls;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Registers;
using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.MetadataDesigner.Views.Exchange;
using InfinniPlatform.MetadataDesigner.Views.Status;
using InfinniPlatform.Sdk.Dynamic;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.MetadataDesigner.Views.ViewModel
{
    public static class ViewModelExtension
    {
        private static readonly RestQueryApi RestQueryApi = null;
        private static readonly IEnumerable<ServiceType> ServiceTypes = ServiceType.GetRegisteredServiceTypes();

        private static readonly List<dynamic> RegisteredMigrationsList = new List<dynamic>
                                                                         {
                                                                             new
                                                                             {
                                                                                 Name = "UpdateStoreMigration",
                                                                                 Description = "Migration updates store mapping after changing configuration documents data schema",
                                                                                 IsUndoable = false,
                                                                                 ConfigurationId = string.Empty,
                                                                                 ConfigVersion = string.Empty
                                                                             }
                                                                         };

        /// <summary>
        /// Extension method to convert dynamic data to a DataTable. Useful for databinding.
        /// </summary>
        /// <param name="items"></param>
        /// <returns>A DataTable with the copied dynamic data.</returns>
        public static DataTable ToDataTable(this IEnumerable<dynamic> items)
        {
            var tempData = new List<Dictionary<string, dynamic>>();

            foreach (var item in items)
            {
                var result = new Dictionary<string, dynamic>();
                foreach (var property in item)
                {
                    result.Add(property.Key, property.Value);
                }

                tempData.Add(result);
            }

            var data = tempData.ToArray();

            if (!data.Any())
            {
                return null;
            }

            var dt = new DataTable();

            var columns = data.SelectMany(item => item.Select(it => it.Key)).Distinct().ToList();

            foreach (var key in columns)
            {
                dt.Columns.Add(key);
            }
            foreach (var d in data)
            {
                dt.Rows.Add(d.Select(p => JToken.FromObject(p.Value).ToString()).ToArray());
            }
            return dt;
        }

        public static IEnumerable<ContextTypeDescription> BuildContextTypes()
        {
            return new List<ContextTypeDescription>
                   {
                       new ContextTypeDescription
                       {
                           ContextTypeKind = ContextTypeKind.ApplyFilter
                       },
                       new ContextTypeDescription
                       {
                           ContextTypeKind = ContextTypeKind.ApplyMove
                       },
                       new ContextTypeDescription
                       {
                           ContextTypeKind = ContextTypeKind.ApplyResult
                       },
                       new ContextTypeDescription
                       {
                           ContextTypeKind = ContextTypeKind.SearchContext
                       },
                       new ContextTypeDescription
                       {
                           ContextTypeKind = ContextTypeKind.Upload
                       }
                   };
        }

        public static IEnumerable<ScriptUnitTypeDescription> BuildScriptUnitTypes()
        {
            return new List<ScriptUnitTypeDescription>();
        }

        public static List<string> BuildServiceTypesHeaders()
        {
            return ServiceTypes.Select(type => type.Name).ToList();
        }

        public static object BuildCompleteServiceType(string serviceTypeName)
        {
            return ServiceTypes.FirstOrDefault(st => st.Name == serviceTypeName);
        }

        public static IEnumerable<ExtensionPoint> BuildServiceTypeExtensionPointList(string serviceName)
        {
            dynamic type = ServiceTypes.FirstOrDefault(s => s.Name == serviceName);

            return type != null
                ? type.WorkflowExtensionPoints
                : Enumerable.Empty<ExtensionPoint>();
        }

        public static IEnumerable<ImageComboBoxItem> BuildImageComboBoxItemsString(this IEnumerable<string> items)
        {
            return items.Select(i => new ImageComboBoxItem(i, i));
        }

        public static IEnumerable<ScriptDescription> BuildGeneratorScripts(IEnumerable<SourceAssemblyInfo> assemblies)
        {
            //все генераторы содержат пока только ActionUnit на Move
            return BuildScripts(assemblies).Where(sc => sc.ContextTypeCode == ContextTypeKind.ApplyMove).ToList();
        }

        public static IEnumerable<ScriptDescription> BuildScripts(IEnumerable<SourceAssemblyInfo> assemblies)
        {
            return new List<ScriptDescription>();
        }

        public static DataTable BuildDocumentScenarios(string version, string configurationId, string documentId)
        {
            var scenarios = GetDocumentScenarios(configurationId, documentId);
            return ToDataTable(scenarios);
        }

        private static IEnumerable<dynamic> GetDocumentScenarios(string configurationId, string documentId)
        {
            return Enumerable.Empty<dynamic>();
        }

        public static string CheckGetView(string version, string configId, string documentId, string viewName, string viewType, string jsonBody)
        {
            var bodyMetadata = new
                               {
                                   Configuration = configId,
                                   MetadataObject = documentId,
                                   MetadataType = viewType,
                                   MetadataName = viewName
                               };

            dynamic dynamicBody = bodyMetadata.ToDynamic();

            dynamic parsedJson;
            try
            {
                parsedJson = jsonBody.ToDynamic();
            }
            catch
            {
                throw new ArgumentException("Не удалось распарсить JSON параметры запроса.");
            }

            if (parsedJson != null)
            {
                dynamicBody.Parameters = parsedJson;
            }


            var process = new StatusProcess();
            var result = string.Empty;
            process.StartOperation(() => { result = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getmanagedmetadata", null, dynamicBody).Content; });
            process.EndOperation();
            return result;
        }

        [Obsolete]
        public static IEnumerable<string> BuildValidationRuleWarningDescriptions(string version, string configId, string documentId)
        {
            return Enumerable.Empty<string>();
        }

        [Obsolete]
        public static IEnumerable<string> BuildValidationRuleErrorDescriptions(string version, string configId, string documentId)
        {
            return Enumerable.Empty<string>();
        }

        public static IEnumerable<HandlerDescription> BuildValidationHandlerDescriptions(string version, string configId, string documentId)
        {
            return new List<HandlerDescription>();
        }

        public static IEnumerable<HandlerDescription> BuildActionHandlerDescriptions(string version, string configId, string documentId)
        {
            return new List<HandlerDescription>();
        }

        public static object[] BuildMigrations()
        {
            return RegisteredMigrationsList.Select(m => (object)m.Name).ToArray();
        }

        public static dynamic BuildMigrationDetails(string version, string configurationId, string migrationName)
        {
            return RegisteredMigrationsList.FirstOrDefault(m => m.Name == migrationName);
        }

        public static RestQueryResponse RunMigration(string version, string configId, string migrationName, object[] parameters)
        {
            var body = new
                       {
                           MigrationName = migrationName,
                           ConfigurationName = configId,
                           Parameters = parameters
                       };

            var result = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "runmigration", null, body);
            return result;
        }

        public static RestQueryResponse RevertMigration(string version, string configId, string migrationName, object[] parameters)
        {
            var body = new
                       {
                           MigrationName = migrationName,
                           ConfigurationName = configId,
                           Parameters = parameters
                       };

            var result = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "revertmigration", null, body);

            return result;
        }

        public static object[] BuildVerifications()
        {
            return new object[] { };
        }

        public static dynamic BuildVerificationDetails(string verifiecationName)
        {
            var result = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getverifications", null, null);

            return result.ToDynamicList().FirstOrDefault(verification => verification.Name.ToString() == verifiecationName);
        }

        public static RestQueryResponse RunVerification(string version, string configId, string verificationName)
        {
            var body = new
                       {
                           VerificationName = verificationName,
                           ConfigurationName = configId
                       };

            var result = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "runverification", null, body);

            return result;
        }

        public static IEnumerable<string> BuildViewTypes()
        {
            return Enumerable.Empty<string>();
        }

        public static IEnumerable<string> BuildRegisterPeriods()
        {
            return RegisterPeriod.GetRegisterPeriods();
        }

        public static IEnumerable<string> BuildRegisterTypes()
        {
            return RegisterType.GetRegisterTypes();
        }

        public static IEnumerable<string> BuildRegisterPropertyTypes()
        {
            return RegisterPropertyType.GetRegisterPropertyTypes();
        }

        public static IEnumerable<string> BuildRegisterPropertyDataTypes()
        {
            return ((DataType[])Enum.GetValues(typeof(DataType)))
                .Where(dataType => dataType != DataType.Array &&
                                   dataType != DataType.Binary &&
                                   dataType != DataType.Object)
                .Select(dataType => dataType.ToString());
        }

        public static DataTable BuildStateTransitions(IEnumerable<dynamic> transitions)
        {
            var result = new List<DynamicWrapper>();
            foreach (var transition in transitions)
            {
                dynamic instance = new DynamicWrapper();
                instance.Id = transition.Id;
                instance.Name = transition.Name;
                instance.StateFrom = transition.StateFrom != null ? transition.StateFrom.Name : null;
                instance.CredentialsType = transition.CredentialsType;
                instance.ValidationPoint = transition.ValidationPoint != null ? transition.ValidationPoint.ScenarioId : null;
                instance.ActionPoint = transition.ActionPoint != null ? transition.ActionPoint.ScenarioId : null;
                instance.SuccessPoint = transition.SuccessPoint != null ? transition.SuccessPoint.ScenarioId : null;
                instance.FailPoint = transition.FailPoint != null ? transition.FailPoint.ScenarioId : null;
                instance.DeletePoint = transition.DeletePoint != null ? transition.DeletePoint.ScenarioId : null;
                instance.ValidationRuleWarning = transition.ValidationRuleWarning;
                instance.ValidationRuleError = transition.ValidationRuleError;
                instance.DeletingDocumentValidationRuleError = transition.DeletingDocumentValidationRuleError;
                result.Add(instance);
            }
            return result.ToDataTable();
        }

        public static object BuildValidationPointFromString(string validationPointHandlerName)
        {
            dynamic instance = new DynamicWrapper();
            instance.TypeName = "Validation";
            instance.ScenarioId = validationPointHandlerName;
            return instance;
        }

        public static object BuildActionPointFromString(string actionPointHandlerName)
        {
            dynamic instance = new DynamicWrapper();
            instance.TypeName = "Action";
            instance.ScenarioId = actionPointHandlerName;
            return instance;
        }

        public static object BuildSuccessPointFromString(string succcesPointHandlerName)
        {
            dynamic instance = new DynamicWrapper();
            instance.TypeName = "OnSuccess";
            instance.ScenarioId = succcesPointHandlerName;
            return instance;
        }

        public static object BuildRegisterPointFromString(string registerPointHandlerName, string documentDateProperty)
        {
            dynamic instance = new DynamicWrapper();
            instance.TypeName = "OnRegisterMove";
            instance.ScenarioId = registerPointHandlerName;
            instance.DocumentDateProperty = documentDateProperty;
            return instance;
        }

        public static object BuildFailPointFromString(string failPointHandlerName)
        {
            dynamic instance = new DynamicWrapper();
            instance.TypeName = "OnFail";
            instance.ScenarioId = failPointHandlerName;
            return instance;
        }

        public static object BuildDeletePointFromString(string deletePointHandlerName)
        {
            dynamic instance = new DynamicWrapper();
            instance.TypeName = "OnDelete";
            instance.ScenarioId = deletePointHandlerName;
            return instance;
        }

        public static string CreateRegisterDocuments(string version, string configId, string registerName)
        {
            dynamic documentMetadata = new DynamicWrapper();
            documentMetadata.Name = RegisterConstants.RegisterNamePrefix + registerName;
            documentMetadata.Id = Guid.NewGuid().ToString();
            documentMetadata.SearchAbility = 0; // SearchAbilityType.KeywordBasedSearch;
            documentMetadata.Description = string.Format("Storage for register {0} data", registerName);

            dynamic schemaProperties = new DynamicWrapper();

            dynamic registrarPropertyModel = new DynamicWrapper();
            registrarPropertyModel.Type = DataType.String.ToString();
            registrarPropertyModel.Caption = "Registrar";
            registrarPropertyModel.Description = "Регистратор";
            schemaProperties[RegisterConstants.RegistrarProperty] = registrarPropertyModel;

            dynamic registrarTypePropertyModel = new DynamicWrapper();
            registrarTypePropertyModel.Type = DataType.String.ToString();
            registrarTypePropertyModel.Caption = "Registrar type";
            registrarTypePropertyModel.Description = "Тип регистратора";
            schemaProperties[RegisterConstants.RegistrarTypeProperty] = registrarTypePropertyModel;

            dynamic documentDatePropertyModel = new DynamicWrapper();
            documentDatePropertyModel.Type = DataType.DateTime.ToString();
            documentDatePropertyModel.Caption = "Document date";
            documentDatePropertyModel.Description = "Дата регистрируемого документа";
            schemaProperties[RegisterConstants.DocumentDateProperty] = documentDatePropertyModel;

            dynamic entryTypePropertyModel = new DynamicWrapper();
            entryTypePropertyModel.Type = DataType.String.ToString();
            entryTypePropertyModel.Caption = "EntryType";
            entryTypePropertyModel.Description = "Тип записи регистра";
            schemaProperties[RegisterConstants.EntryTypeProperty] = entryTypePropertyModel;

            documentMetadata.Schema = new DynamicWrapper();
            documentMetadata.Schema.Type = "Object";
            documentMetadata.Schema.Caption = "Register document";
            documentMetadata.Schema.Description = "Register document schema";
            documentMetadata.Schema.Properties = schemaProperties;

            // Создаём документ для подсчета промежуточных итогов

            dynamic documentTotalMetadata = new DynamicWrapper();
            documentTotalMetadata.Name = RegisterConstants.RegisterTotalNamePrefix + registerName;
            documentMetadata.Id = Guid.NewGuid().ToString();
            documentMetadata.SearchAbility = 0; // SearchAbilityType.KeywordBasedSearch;
            documentMetadata.Description = string.Format("Storage for register {0} totals", registerName);

            dynamic schemaTotalProperties = new DynamicWrapper();

            dynamic documentCalculationDatePropertyModel = new DynamicWrapper();
            documentCalculationDatePropertyModel.Type = DataType.DateTime.ToString();
            documentCalculationDatePropertyModel.Caption = "Date of total calculation";
            documentCalculationDatePropertyModel.Description = "Дата регистрируемого документа";
            schemaTotalProperties[RegisterConstants.DocumentDateProperty] = documentCalculationDatePropertyModel;

            documentTotalMetadata.Schema = new DynamicWrapper();
            documentTotalMetadata.Schema.Type = "Object";
            documentTotalMetadata.Schema.Caption = "Register document for totals";
            documentTotalMetadata.Schema.Description = "Register document schema";
            documentTotalMetadata.Schema.Properties = schemaProperties;

            return RegisterConstants.RegisterNamePrefix + registerName;
        }

        public static dynamic GetRegisterDocumentSchema(string version, string configId, string registerName)
        {
            dynamic registerDocument = PackageMetadataLoader.GetDocument(configId, $"{RegisterConstants.RegisterNamePrefix}{registerName}");

            if (registerDocument != null)
            {
                return registerDocument.Schema;
            }

            return null;
        }

        public static void UpdateRegisterDocumentSchema(string version, string configId, string registerName, dynamic documentSchema)
        {
        }

        public static dynamic GetRegisterDocumentTotalSchema(string version, string configId, string registerName)
        {
            return null;
        }

        public static void UpdateRegisterDocumentTotalSchema(string version, string configId, string registerName, dynamic documentSchema)
        {
        }
    }


    /// <summary>
    /// Доступные типы сервисов. Регистрируются в классе InfinniPlatformHostFactory.cs.
    /// Захардкожено, чтобы не создавать лишних ссылок на проекты.
    /// Изменять типы сервисов не планируется - планируется удалить.
    /// </summary>
    internal class ServiceType
    {
        public ServiceType(string name, List<ExtensionPoint> workflowExtensionPoints)
        {
            Name = name;
            WorkflowExtensionPoints = workflowExtensionPoints;
        }

        public string Name { get; set; }
        public List<ExtensionPoint> WorkflowExtensionPoints { get; set; }

        public static IEnumerable<ServiceType> GetRegisteredServiceTypes()
        {
            return new List<ServiceType>
                   {
                       new ServiceType("applyevents", new List<ExtensionPoint>
                                                      {
                                                          new ExtensionPoint("FilterEvents", 4, "Document filter events context"),
                                                          new ExtensionPoint("Move", 2, "Document move context"),
                                                          new ExtensionPoint("GetResult", 8, "Document move result context")
                                                      }),
                       new ServiceType("applyjson", new List<ExtensionPoint>
                                                    {
                                                        new ExtensionPoint("FilterEvents", 4, "Document filter events context"),
                                                        new ExtensionPoint("Move", 2, "Document move context"),
                                                        new ExtensionPoint("GetResult", 8, "Document move result context")
                                                    }),
                       new ServiceType("search", new List<ExtensionPoint>
                                                 {
                                                     new ExtensionPoint("ValidateFilter", 16, "Document search context"),
                                                     new ExtensionPoint("SearchModel", 16, "Document search context")
                                                 }),
                       new ServiceType("upload", new List<ExtensionPoint>
                                                 {
                                                     new ExtensionPoint("Upload", 32, "File upload context")
                                                 }),
                       new ServiceType("urlencodeddata", new List<ExtensionPoint>
                                                         {
                                                             new ExtensionPoint("ProcessUrlEncodedData", 64, "Unknown context type")
                                                         }),
                       new ServiceType("aggregation", new List<ExtensionPoint>
                                                      {
                                                          new ExtensionPoint("Join", 16, "Document search context"),
                                                          new ExtensionPoint("TransformResult", 16, "Document search context")
                                                      })
                   };
        }
    }
}