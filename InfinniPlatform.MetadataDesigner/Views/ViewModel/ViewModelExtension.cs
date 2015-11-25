using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

using DevExpress.XtraEditors.Controls;

using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.MetadataDesigner.Views.Exchange;
using InfinniPlatform.MetadataDesigner.Views.Status;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Register;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.MetadataDesigner.Views.ViewModel
{
    public static class ViewModelExtension
    {
        private static readonly IEnumerable<ServiceType> ServiceTypes = ServiceType.GetRegisteredServiceTypes();

        private static readonly List<dynamic> RegisteredMigrationsList = new List<dynamic>
                                                                         {
                                                                             new
                                                                             {
                                                                                 Name = "UpdateStoreMigration",
                                                                                 Description = "Migration updates store mapping after changing configuration documents data schema",
                                                                                 IsUndoable = false,
                                                                                 ConfigurationId = string.Empty,
                                                                                 ConfigVersion = string.Empty,
                                                                                 Parameters = new MigrationParameter[0]
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

        public static DataTable BuildConfigurationHeaders(this object configurationId)
        {
            var result = new List<DynamicWrapper>();
            var configs = configurationId.ToEnumerable();
            foreach (var config in configs)
            {
                dynamic item = new DynamicWrapper();
                item.Name = config.Name;
                item.Caption = config.Caption;
                item.Description = config.Description;
                result.Add(item);
            }
            return result.ToDataTable();
        }

        public static IEnumerable<dynamic> LoadAssemblies(string version, string configurationId)
        {
            var reader = ManagerFactoryConfiguration.BuildConfigurationMetadataReader();

            var config = reader.GetItem(configurationId);
            return DynamicWrapperExtensions.ToEnumerable(config.Assemblies);
        }

        public static DataTable BuildDocumentHeaders(dynamic documents, string configName)
        {
            var result = new List<DynamicWrapper>();
            var items = DynamicWrapperExtensions.ToEnumerable(documents);
            foreach (var document in items)
            {
                dynamic item = new DynamicWrapper();
                item.Id = document.Id;
                item.Name = document.Name;
                item.Caption = document.Caption;
                item.Description = document.Description;
                result.Add(item);
            }
            return result.ToDataTable();
        }

        public static IEnumerable<ContextTypeDescription> BuildContextTypes()
        {
            return new List<ContextTypeDescription>
                   {
                       new ContextTypeDescription
                       {
                           ContextTypeKind = ContextTypeKind.ApplyFilter,
                           Description = ContextTypeKind.ApplyFilter.GetContextTypeDisplayByKind()
                       },
                       new ContextTypeDescription
                       {
                           ContextTypeKind = ContextTypeKind.ApplyMove,
                           Description = ContextTypeKind.ApplyMove.GetContextTypeDisplayByKind()
                       },
                       new ContextTypeDescription
                       {
                           ContextTypeKind = ContextTypeKind.ApplyResult,
                           Description = ContextTypeKind.ApplyResult.GetContextTypeDisplayByKind()
                       },
                       new ContextTypeDescription
                       {
                           ContextTypeKind = ContextTypeKind.SearchContext,
                           Description = ContextTypeKind.SearchContext.GetContextTypeDisplayByKind()
                       },
                       new ContextTypeDescription
                       {
                           ContextTypeKind = ContextTypeKind.Upload,
                           Description = ContextTypeKind.Upload.GetContextTypeDisplayByKind()
                       }
                   };
        }

        public static IEnumerable<ScriptUnitTypeDescription> BuildScriptUnitTypes()
        {
            return new List<ScriptUnitTypeDescription>
                   {
                       new ScriptUnitTypeDescription
                       {
                           ScriptUnitType = ScriptUnitType.Action,
                           Description = "Action"
                       },
                       new ScriptUnitTypeDescription
                       {
                           ScriptUnitType = ScriptUnitType.Validator,
                           Description = "Validator"
                       }
                   };
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

        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            //file is not locked
            return false;
        }

        public static void LoadModules(string modules, string baseDirectory)
        {
            var loadedModules = modules.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(m => m.Trim());

            foreach (var filename in loadedModules)
            {
                try
                {
                    var fileDll = Path.Combine(baseDirectory, filename + ".dll");
                    var fileExe = Path.Combine(baseDirectory, filename + ".exe");

                    if (File.Exists(fileDll))
                    {
                        var destinationFile = Path.Combine(Directory.GetCurrentDirectory(), filename + ".dll");
                        if (!File.Exists(destinationFile))
                        {
                            File.WriteAllBytes(destinationFile, File.ReadAllBytes(fileDll));
                        }
                        else
                        {
                            var fileInfo = new FileInfo(destinationFile);

                            if (!IsFileLocked(fileInfo))
                            {
                                File.WriteAllBytes(destinationFile, File.ReadAllBytes(fileDll));
                            }
                        }
                    }

                    if (File.Exists(fileExe))
                    {
                        var destinationFile = Path.Combine(Directory.GetCurrentDirectory(), filename + ".exe");
                        if (!File.Exists(destinationFile))
                        {
                            File.WriteAllBytes(destinationFile, File.ReadAllBytes(fileExe));
                        }
                        else
                        {
                            var fileInfo = new FileInfo(destinationFile);

                            if (!IsFileLocked(fileInfo))
                            {
                                File.WriteAllBytes(destinationFile, File.ReadAllBytes(fileExe));
                            }
                        }
                    }
                }
                catch (BadImageFormatException)
                {
                    // Not a valid assembly, move on
                }
            }
        }

        public static IEnumerable<ScriptDescription> BuildScripts(IEnumerable<SourceAssemblyInfo> assemblies)
        {
            var result = new List<ScriptDescription>();
            if (assemblies != null)
            {
                foreach (var assembly in assemblies.Where(a => a.Assembly != null))
                {
                    var scriptInfoProvider = new ScriptInfoProvider(assembly.Assembly);

                    var referencedAssemblies = string.Join(",", assembly.Assembly.GetReferencedAssemblies().Select(r => r.Name));

                    var assemblyPath = Path.GetDirectoryName(assembly.AssemblyFileName);

                    LoadModules(referencedAssemblies, assemblyPath);

                    var infoList = scriptInfoProvider.GetScriptMethodsInfo();
                    foreach (var o in infoList)
                    {
                        result.Add(new ScriptDescription
                                   {
                                       ContextTypeCode = (ContextTypeKind)o.ContextTypeCode,
                                       MethodName = o.MethodName,
                                       TypeName = o.TypeName
                                   });
                    }
                }
            }
            return result;
        }

        public static DataTable BuildDocumentScenarios(string version, string configurationId, string documentId)
        {
            var scenarios = GetDocumentScenarios(configurationId, documentId);
            return ToDataTable(scenarios);
        }

        private static IEnumerable<dynamic> GetDocumentScenarios(string configurationId, string documentId)
        {
            var reader = new ManagerFactoryDocument(configurationId, documentId).BuildScenarioMetadataReader();
            var scenarios =
                reader.GetItems().ToEnumerable();
            return scenarios;
        }

        public static DataTable BuildDocumentServices(string version, string configurationId, string documentId)
        {
            var reader = new ManagerFactoryConfiguration(configurationId).BuildDocumentMetadataReader();
            IEnumerable<dynamic> services = DynamicWrapperExtensions.ToEnumerable(reader.GetItem(documentId).Services);

            var serviceInstances = new List<DynamicWrapper>();
            var serviceManager = new ManagerFactoryDocument(configurationId, documentId).BuildServiceMetadataReader();
            foreach (var service in services)
            {
                dynamic serviceFull = serviceManager.GetItem(service.Name);

                if (serviceFull == null)
                {
                    Console.WriteLine(string.Format("Not found service item \"{0}\"", service.Name));
                    continue;
                }
                dynamic ins = new DynamicWrapper();
                ins.Id = serviceFull.Id;
                ins.Name = serviceFull.Name;
                ins.Type = serviceFull.Type.Name;
                serviceInstances.Add(ins);
            }
            return ToDataTable(serviceInstances);
        }

        public static object GetDocumentProcessByName(string version, string configurationId, string documentId, string processName)
        {
            var managerProcess = new ManagerFactoryDocument(configurationId, documentId).BuildProcessMetadataReader();

            return managerProcess.GetItem(processName);
        }

        public static IEnumerable<ProcessDescription> GetDocumentProcessesList(string version, string configurationId, string documentId)
        {
            var managerProcess = new ManagerFactoryDocument(configurationId, documentId).BuildProcessMetadataReader();

            var processes = managerProcess.GetItems();

            var processesViewModel = new List<ProcessDescription>();
            foreach (var process in processes)
            {
                var ins = new ProcessDescription();
                ins.Id = process.Id;
                ins.Name = process.Name;
                ins.Caption = process.Caption;
                processesViewModel.Add(ins);
            }
            return processesViewModel;
        }

        public static IEnumerable<GeneratorDescription> GetGenerators(string version, string configurationId, string documentId)
        {
            var reader = new ManagerFactoryDocument(configurationId, documentId).BuildGeneratorMetadataReader();
            var managerGenerators = reader.GetItems();

            var fullGeneratorMetadata = new List<dynamic>();
            foreach (var managerGenerator in managerGenerators)
            {
                var generator = reader.GetItem(managerGenerator.Name);
                if (generator == null)
                {
                    Console.WriteLine(string.Format("Not found generator item \"{0}\"", managerGenerator.Name));
                    continue;
                }
                fullGeneratorMetadata.Add(generator);
            }

            var generatorList = new List<GeneratorDescription>();
            foreach (var generator in fullGeneratorMetadata)
            {
                var descr = new GeneratorDescription();
                descr.Name = generator.Name;
                descr.MetadataType = generator.MetadataType;
                generatorList.Add(descr);
            }
            return generatorList;
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
            var result = new List<HandlerDescription>();

            var scenarios = PackageMetadataLoader.GetScenarios(configId, documentId);

            foreach (dynamic scenario in scenarios)
            {
                if (scenario != null)
                {
                    if ((ScriptUnitType)scenario.ScriptUnitType == ScriptUnitType.Validator)
                    {
                        var handler = new HandlerDescription
                                      {
                                          HandlerCaption = scenario.Caption,
                                          HandlerId = scenario.Id
                                      };
                        result.Add(handler);
                    }
                }
            }

            return result;
        }

        public static IEnumerable<HandlerDescription> BuildActionHandlerDescriptions(string version, string configId, string documentId)
        {
            var result = new List<HandlerDescription>();

            var scenarios = PackageMetadataLoader.GetScenarios(configId, documentId);

            foreach (dynamic scenario in scenarios)
            {
                if (scenario != null)
                {
                    if ((ScriptUnitType)scenario.ScriptUnitType == ScriptUnitType.Action)
                    {
                        var handler = new HandlerDescription
                                      {
                                          HandlerCaption = scenario.Caption,
                                          HandlerId = scenario.Id
                                      };
                        result.Add(handler);
                    }
                }
            }

            return result;
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
            RestQueryApi.QueryPostNotify(configId);

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
            RestQueryApi.QueryPostNotify(configId);

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
            RestQueryApi.QueryPostNotify(configId);

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
            return ViewType.GetViewTypes();
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
            var managerDocument = new ManagerFactoryConfiguration(configId).BuildDocumentManager();

            dynamic documentMetadata = managerDocument.CreateItem(RegisterConstants.RegisterNamePrefix + registerName);
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

            managerDocument.MergeItem(documentMetadata);


            // Создаём документ для подсчета промежуточных итогов

            dynamic documentTotalMetadata = managerDocument.CreateItem(RegisterConstants.RegisterTotalNamePrefix + registerName);
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

            managerDocument.MergeItem(documentTotalMetadata);

            var registerInfoDocument = new
                                       {
                                           Id = registerName
                                       };

            new DocumentApi().SetDocument(configId, configId + RegisterConstants.RegistersCommonInfo, registerInfoDocument);

            new UpdateApi().ForceReload(configId);

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
            var managerDocument = new ManagerFactoryConfiguration(configId).BuildDocumentManager();
            var registerDocument = managerDocument.MetadataReader.GetItem(RegisterConstants.RegisterNamePrefix + registerName);

            registerDocument.Schema = documentSchema;

            managerDocument.MergeItem(registerDocument);

            // UpdateApi.ForceReload(configId);
        }

        public static dynamic GetRegisterDocumentTotalSchema(string version, string configId, string registerName)
        {
            var managerDocument = new ManagerFactoryConfiguration(configId).BuildDocumentManager();
            var registerDocumentTotal = managerDocument.MetadataReader.GetItem(RegisterConstants.RegisterTotalNamePrefix + registerName);

            if (registerDocumentTotal != null)
            {
                return registerDocumentTotal.Schema;
            }

            return null;
        }

        public static void UpdateRegisterDocumentTotalSchema(string version, string configId, string registerName, dynamic documentSchema)
        {
            var managerDocument = new ManagerFactoryConfiguration(configId).BuildDocumentManager();
            var registerDocumentTotal = managerDocument.MetadataReader.GetItem(RegisterConstants.RegisterTotalNamePrefix + registerName);

            registerDocumentTotal.Schema = documentSchema;

            managerDocument.MergeItem(registerDocumentTotal);

            // UpdateApi.ForceReload(configId);
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
                       new ServiceType("apiapplyjson", new List<ExtensionPoint>
                                                       {
                                                           new ExtensionPoint("FilterEvents", 4, "Document filter events context"),
                                                           new ExtensionPoint("Move", 2, "Document move context"),
                                                           new ExtensionPoint("GetResult", 8, "Document move result context")
                                                       }),
                       new ServiceType("notify", new List<ExtensionPoint>()),
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