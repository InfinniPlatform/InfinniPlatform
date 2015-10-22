using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.RestfulApi.Installers
{
    public sealed class RestfulApiInstaller : MetadataConfigurationInstaller
    {
        public RestfulApiInstaller(IMetadataConfigurationProvider metadataConfigurationProvider, IScriptConfiguration actionConfiguration)
            : base(metadataConfigurationProvider, actionConfiguration)
        {
        }

        protected override void RegisterConfiguration(IMetadataConfiguration metadataConfiguration)
        {
            var actionUnits = metadataConfiguration.ScriptConfiguration;

            actionUnits.RegisterActionUnitDistributedStorage("indexexists", "ActionUnitIndexExists");
            actionUnits.RegisterActionUnitDistributedStorage("rebuildindex", "ActionUnitRebuildIndex");
            actionUnits.RegisterActionUnitDistributedStorage("getfromindex", "ActionUnitGetFromIndex");
            actionUnits.RegisterActionUnitDistributedStorage("getindexstorageinfo", "ActionUnitGetIndexStorageInfo");
            actionUnits.RegisterActionUnitDistributedStorage("insertindex", "ActionUnitInsertIndex");
            actionUnits.RegisterActionUnitDistributedStorage("insertindexwithtimestamp", "ActionUnitIndexWithTimeStamp");
            actionUnits.RegisterActionUnitDistributedStorage("getdocument", "ActionUnitGetDocument");
            actionUnits.RegisterActionUnitDistributedStorage("getnumberofdocuments", "ActionUnitGetNumberOfDocuments");
            actionUnits.RegisterActionUnitDistributedStorage("getconfigmetadata", "ActionUnitGetConfigMetadata");
            actionUnits.RegisterActionUnitDistributedStorage("getconfigmetadatalist", "ActionUnitGetConfigMetadataList");
            actionUnits.RegisterActionUnitDistributedStorage("getdocumentcrossconfig", "ActionUnitGetDocumentCrossConfig");
            actionUnits.RegisterActionUnitDistributedStorage("getbyquery", "ActionUnitGetByQuery");
            actionUnits.RegisterActionUnitDistributedStorage("getdocumentbyid", "ActionUnitGetDocumentById");

            actionUnits.RegisterActionUnitDistributedStorage("createsession", "ActionUnitCreateSession");
            actionUnits.RegisterActionUnitDistributedStorage("attachdocumentsession", "ActionUnitAttachDocumentSession");
            actionUnits.RegisterActionUnitDistributedStorage("detachdocumentsession", "ActionUnitDetachDocumentSession");
            actionUnits.RegisterActionUnitDistributedStorage("savesession", "ActionUnitSaveSession");
            actionUnits.RegisterActionUnitDistributedStorage("getsession", "ActionUnitGetSession");
            actionUnits.RegisterActionUnitDistributedStorage("removesession", "ActionUnitRemoveSession");
            actionUnits.RegisterActionUnitDistributedStorage("attachfile", "ActionUnitAttachFile");
            actionUnits.RegisterActionUnitDistributedStorage("detachfile", "ActionUnitDetachFile");

            actionUnits.RegisterActionUnitDistributedStorage("setdocument", "ActionUnitSetDocument");
            actionUnits.RegisterActionUnitDistributedStorage("successsetdocument", "ActionUnitSuccessSetDocument");
            actionUnits.RegisterActionUnitDistributedStorage("failsetdocument", "ActionUnitFailSetDocument");
            actionUnits.RegisterActionUnitDistributedStorage("successdeletedocument", "ActionUnitSuccessDeleteDocument");
            actionUnits.RegisterActionUnitDistributedStorage("filterauthdocument", "ActionUnitFilterAuthDocument");

            actionUnits.RegisterActionUnitDistributedStorage("simpleauth", "ActionUnitSimpleAuth");
            actionUnits.RegisterActionUnitDistributedStorage("complexauth", "ActionUnitComplexAuth");
            actionUnits.RegisterActionUnitDistributedStorage("applyaccess", "ActionUnitApplyAccess");
            actionUnits.RegisterActionUnitDistributedStorage("changepassword", "ActionUnitChangePassword");

            actionUnits.RegisterActionUnitDistributedStorage("signin", "ActionUnitSignIn");
            actionUnits.RegisterActionUnitDistributedStorage("signin", "ActionUnitSignIn");
            actionUnits.RegisterActionUnitDistributedStorage("signout", "ActionUnitSignOut");

            actionUnits.RegisterActionUnitDistributedStorage("adduser", "ActionUnitAddUser");
            actionUnits.RegisterActionUnitDistributedStorage("adduserrole", "ActionUnitAddUserRole");
            actionUnits.RegisterActionUnitDistributedStorage("addrole", "ActionUnitAddRole");
            actionUnits.RegisterActionUnitDistributedStorage("addclaim", "ActionUnitAddClaim");
            actionUnits.RegisterActionUnitDistributedStorage("removeclaim", "ActionUnitRemoveClaim");
            actionUnits.RegisterActionUnitDistributedStorage("getclaim", "ActionUnitGetClaim");

            actionUnits.RegisterActionUnitDistributedStorage("removeuser", "ActionUnitRemoveUser");
            actionUnits.RegisterActionUnitDistributedStorage("removeuserrole", "ActionUnitRemoveUserRole");
            actionUnits.RegisterActionUnitDistributedStorage("removerole", "ActionUnitRemoveRole");
            actionUnits.RegisterActionUnitDistributedStorage("removeacl", "ActionUnitRemoveAcl");

            actionUnits.RegisterActionUnitDistributedStorage("getacl", "ActionUnitGetAcl");
            actionUnits.RegisterActionUnitDistributedStorage("getroles", "ActionUnitGetRoles");
            actionUnits.RegisterActionUnitDistributedStorage("getuserroles", "ActionUnitGetUserRoles");
            actionUnits.RegisterActionUnitDistributedStorage("getusers", "ActionUnitGetUsers");
            actionUnits.RegisterActionUnitDistributedStorage("getuser", "ActionUnitGetUser");

            actionUnits.RegisterActionUnitDistributedStorage("updateroles", "ActionUnitUpdateUserRoles");
            actionUnits.RegisterActionUnitDistributedStorage("setdefaultacl", "ActionUnitSetDefaultAcl");
            actionUnits.RegisterActionUnitDistributedStorage("grantadminacl", "ActionUnitGrantAdminAcl");

            actionUnits.RegisterActionUnitDistributedStorage("updateacl", "ActionUnitUpdateAcl");

            actionUnits.RegisterActionUnitDistributedStorage("adminauth", "ActionUnitAdminAuth");
            actionUnits.RegisterActionUnitDistributedStorage("documentauth", "ActionUnitDocumentAuth");

            actionUnits.RegisterActionUnitDistributedStorage("uploadbinarycontent", "ActionUnitUploadBinaryContent");
            actionUnits.RegisterActionUnitDistributedStorage("downloadbinarycontent", "ActionUnitDownloadBinaryContent");

            actionUnits.RegisterActionUnitDistributedStorage("createdocument", "ActionUnitCreateDocument");
            actionUnits.RegisterActionUnitDistributedStorage("deletedocument", "ActionUnitDeleteDocument");
            actionUnits.RegisterActionUnitDistributedStorage("deletedocuments", "ActionUnitDeleteDocuments");

            actionUnits.RegisterActionUnitDistributedStorage("status", "ActionUnitRestIsReady");
            actionUnits.RegisterActionUnitDistributedStorage("getdocumentfaststorage", "ActionUnitGetDocumentFastStorage");

            actionUnits.RegisterValidationUnitDistributedStorage("setdocumentvalidationwarning", "ValidationUnitSetDocumentWarning");
            actionUnits.RegisterValidationUnitDistributedStorage("deletedocumentvalidationerror", "ValidationUnitDeleteDocumentError");
            actionUnits.RegisterValidationUnitDistributedStorage("setdocumentvalidationerror", "ValidationUnitSetDocumentError");

            actionUnits.RegisterActionUnitDistributedStorage("setanonimouscredentials", "ActionUnitSetAnonimousCredentials");
            actionUnits.RegisterActionUnitDistributedStorage("setcredentials", "ActionUnitSetCredentials");

            actionUnits.RegisterActionUnitDistributedStorage("filterupdateevents", "ActionUnitProcessUpdateEvents");

            metadataConfiguration.RegisterWorkflow("index", "indexexists",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "indexexists")))));

            metadataConfiguration.RegisterWorkflow("index", "rebuildindex",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "rebuildindex")))));

            metadataConfiguration.RegisterWorkflow("index", "getfromindex",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getfromindex")))));
            metadataConfiguration.RegisterWorkflow("index", "insertindex",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "insertindex")))));
            metadataConfiguration.RegisterWorkflow("index", "insertindexwithtimestamp",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "insertindexwithtimestamp")))));


            metadataConfiguration.RegisterWorkflow("configuration", "getconfigmetadata",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getconfigmetadata"))
                    )));

            metadataConfiguration.RegisterWorkflow("configuration", "getconfigmetadatalist",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getconfigmetadatalist"))
                    )));

            metadataConfiguration.RegisterWorkflow("configuration", "getdocumentcrossconfig",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getdocumentcrossconfig"))
                        .WithSimpleAuthorization
                        (() =>
                            actionUnits
                                .GetAction(
                                    "documentauth"))
                        .OnSuccess(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "filterauthdocument"))
                    )));

            metadataConfiguration.RegisterWorkflow("configuration", "filterauthdocument",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "filterauthdocument")))));


            metadataConfiguration.RegisterWorkflow("configuration", "getbyquery",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getbyquery")))));


            metadataConfiguration.RegisterWorkflow("authorization", "applyaccess",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithSimpleAuthorization
                        (() =>
                            actionUnits
                                .GetAction(
                                    "adminauth"))
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "applyaccess"))
                        .OnSuccess(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "updateacl"))
                    )));


            metadataConfiguration.RegisterWorkflow("authorization", "signin",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "signin"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "signout",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "signout"))
                    )));


            metadataConfiguration.RegisterWorkflow("authorization", "changepassword",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "changepassword"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "adduser",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "adduser"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "adduserrole",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "adduserrole"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "addrole",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "addrole"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "addclaim",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "addclaim"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "removeclaim",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "removeclaim"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "getclaim",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getclaim"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "removeuser",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "removeuser"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "removeuserrole",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "removeuserrole"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "removerole",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "removerole"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "removeacl",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "removeacl"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "getuserroles",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getuserroles"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "getroles",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getroles"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "grantadminacl",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "grantadminacl"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "setdefaultacl",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "setdefaultacl"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "getusers",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getusers"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "getuser",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getuser"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "updateacl",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "updateacl"))
                    )));

            metadataConfiguration.RegisterWorkflow("authorization", "updateroles",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        //.WithSimpleAuthorization(() => actionUnits.GetAction("simpleauth"))
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "updateroles"))
                    )));


            metadataConfiguration.RegisterWorkflow("authorization", "getacl",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        //.WithSimpleAuthorization(() => actionUnits.GetAction("simpleauth"))
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getacl"))
                    )));


            metadataConfiguration.RegisterWorkflow("authorization", "simpleauth",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "simpleauth"))
                    )));

            metadataConfiguration.RegisterWorkflow("configuration", "downloadbinarycontent",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "downloadbinarycontent"))
                    )));

            metadataConfiguration.RegisterWorkflow("configuration", "uploadbinarycontent",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "uploadbinarycontent"))
                    )));


            metadataConfiguration.RegisterWorkflow("configuration", "getdocument", f => f
                .FlowWithoutState(wc => wc.Move(ws => ws.WithAction(() => actionUnits.GetAction("getdocument"))
                                                        .WithSimpleAuthorization(() => actionUnits.GetAction("documentauth"))
                                                        .WithComplexAuthorization(() => actionUnits.GetAction("complexauth"))
                                                        .OnSuccess(() => actionUnits.GetAction("filterauthdocument"))
                                                        .OnCredentials(() => actionUnits.GetAction("setcredentials"))
                    )));

            metadataConfiguration.RegisterWorkflow("configuration", "getnumberofdocuments",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(() => actionUnits.GetAction("getnumberofdocuments"))
                        .WithSimpleAuthorization(() => actionUnits.GetAction("documentauth"))
                        .WithComplexAuthorization(() => actionUnits.GetAction("complexauth"))
                        .OnSuccess(() => actionUnits.GetAction("filterauthdocument"))
                        .OnCredentials(() => actionUnits.GetAction("setcredentials"))
                    )));

            metadataConfiguration.RegisterWorkflow("configuration", "getdocumentbyid",
                f => f.FlowWithoutState(wc => wc
                    .Move(
                        ws =>
                            ws.WithAction(
                                () =>
                                    actionUnits.GetAction(
                                        "getdocumentbyid")))));

            metadataConfiguration.RegisterWorkflow("configuration", "filterupdateevents",
                f => f.FlowWithoutState(wc => wc
                    .Move(
                        ws =>
                            ws.WithAction(
                                () =>
                                    actionUnits.GetAction(
                                        "filterupdateevents")))));

            metadataConfiguration.RegisterWorkflow("configuration", "setdocument",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithValidationWarning
                        (() =>
                            actionUnits
                                .GetValidator(
                                    "setdocumentvalidationwarning"))
                        .WithValidationError
                        (() =>
                            actionUnits
                                .GetValidator(
                                    "setdocumentvalidationerror"))
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "setdocument"))
                        .WithSimpleAuthorization
                        (() =>
                            actionUnits
                                .GetAction(
                                    "documentauth"))
                        .WithComplexAuthorization
                        (() =>
                            actionUnits
                                .GetAction(
                                    "complexauth"))
                        .OnSuccess(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "successsetdocument"))
                        .OnFail(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "failsetdocument"))
                        .OnCredentials(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "setcredentials"))
                    )));


            metadataConfiguration.RegisterWorkflow("configuration", "createdocument",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "createdocument"))
                        .WithSimpleAuthorization
                        (() =>
                            actionUnits
                                .GetAction(
                                    "documentauth"))
                        .WithComplexAuthorization
                        (() =>
                            actionUnits
                                .GetAction(
                                    "complexauth"))
                        .OnCredentials(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "setcredentials"))
                    )));

            metadataConfiguration.RegisterWorkflow("configuration", "deletedocument",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithValidationError(() => actionUnits.GetValidator("deletedocumentvalidationerror"))
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "deletedocument"))
                        .OnDelete(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "successdeletedocument"))
                        .WithSimpleAuthorization
                        (() =>
                            actionUnits
                                .GetAction(
                                    "documentauth"))
                        .WithComplexAuthorization
                        (() =>
                            actionUnits
                                .GetAction(
                                    "complexauth"))
                        .OnCredentials(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "setcredentials"))
                    )));


            metadataConfiguration.RegisterWorkflow("configuration", "status",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "status")))));
            metadataConfiguration.RegisterWorkflow("configuration", "getindexstorageinfo",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(() => actionUnits.GetAction("getindexstorageinfo")))));

            metadataConfiguration.RegisterWorkflow("configuration", "createsession",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "createsession")))));

            metadataConfiguration.RegisterWorkflow("configuration", "savesession",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "savesession")))));

            metadataConfiguration.RegisterWorkflow("configuration", "attachdocumentsession",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "attachdocumentsession")))));

            metadataConfiguration.RegisterWorkflow("configuration", "attachfile",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "attachfile")))));

            metadataConfiguration.RegisterWorkflow("configuration", "detachfile",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "detachfile")))));

            metadataConfiguration.RegisterWorkflow("configuration", "detachdocumentsession",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "detachdocumentsession")))));

            metadataConfiguration.RegisterWorkflow("configuration", "getsession",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "getsession")))));

            metadataConfiguration.RegisterWorkflow("configuration", "removesession",
                f => f.FlowWithoutState(wc => wc
                    .Move(ws => ws
                        .WithAction(
                            () =>
                                actionUnits
                                    .GetAction(
                                        "removesession")))));
        }

        protected override void RegisterServices(IServiceRegistrationContainer servicesConfiguration)
        {
            servicesConfiguration.AddRegistration("index", "ApplyJson", reg => reg
                .RegisterHandlerInstance(
                    "indexexists",
                    instance =>
                        instance.RegisterExtensionPoint(
                            "GetResult", "indexexists"))
                .RegisterHandlerInstance(
                    "rebuildindex",
                    instance =>
                        instance.RegisterExtensionPoint(
                            "GetResult", "rebuildindex"))
                .RegisterHandlerInstance(
                    "getfromindex",
                    instance =>
                        instance.RegisterExtensionPoint(
                            "GetResult", "getfromindex"))
                .RegisterHandlerInstance(
                    "insertindex",
                    instance =>
                        instance.RegisterExtensionPoint(
                            "GetResult", "insertindex"))
                .RegisterHandlerInstance(
                    "insertindexwithtimestamp",
                    instance =>
                        instance.RegisterExtensionPoint(
                            "GetResult",
                            "insertindexwithtimestamp"))
                .SetResultHandler(
                    HttpResultHandlerType.BadRequest)
                );

            servicesConfiguration.AddRegistration("authorization", "ApplyJson", reg => reg
                .RegisterHandlerInstance(
                    "applyaccess",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "applyaccess"))
                .RegisterHandlerInstance(
                    "simpleauth",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "simpleauth"))
                .RegisterHandlerInstance(
                    "adduser",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "adduser"))
                .RegisterHandlerInstance(
                    "addrole",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "addrole"))
                .RegisterHandlerInstance(
                    "adduserrole",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "adduserrole"))
                .RegisterHandlerInstance(
                    "addclaim",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "addclaim"))
                .RegisterHandlerInstance(
                    "removeclaim",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "removeclaim"))
                .RegisterHandlerInstance(
                    "getclaim",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "getclaim"))
                .RegisterHandlerInstance(
                    "setdefaultacl",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "setdefaultacl"))
                .RegisterHandlerInstance(
                    "grantadminacl",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "grantadminacl"))
                .RegisterHandlerInstance(
                    "removeuser",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "removeuser"))
                .RegisterHandlerInstance(
                    "removerole",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "removerole"))
                .RegisterHandlerInstance(
                    "removeacl",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "removeacl"))
                .RegisterHandlerInstance(
                    "removeuserrole",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "removeuserrole"))
                .RegisterHandlerInstance(
                    "changepassword",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "changepassword"))
                .RegisterHandlerInstance(
                    "getusers",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "getusers"))
                .RegisterHandlerInstance(
                    "getuser",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "getuser"))
                .RegisterHandlerInstance(
                    "getuserroles",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "getuserroles"))
                .RegisterHandlerInstance(
                    "getroles",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "getroles"))
                .RegisterHandlerInstance(
                    "getacl",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "getacl"))
                .RegisterHandlerInstance(
                    "updateroles",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "updateroles"))
                .RegisterHandlerInstance(
                    "updateacl",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "updateacl"))
                .RegisterHandlerInstance(
                    "signout",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "signout"))
                .SetResultHandler(
                    HttpResultHandlerType
                        .BadRequest)
                );
            servicesConfiguration.AddRegistration("authorization", "ApplyJson", reg => reg
                .RegisterHandlerInstance(
                    "signin",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move", "signin"))
                .SetResultHandler(
                    HttpResultHandlerType
                        .SignIn));


            servicesConfiguration.AddRegistration("configuration", "Upload", reg => reg
                .RegisterHandlerInstance(
                    "attachfile",
                    insance =>
                        insance
                            .RegisterExtensionPoint(
                                "Upload",
                                "attachfile")));

            servicesConfiguration.AddRegistration("configuration", "ApplyJson", reg => reg
                .RegisterHandlerInstance(
                    "status",
                    insance => insance
                        .RegisterExtensionPoint
                        ("GetResult",
                            "status"))
                .RegisterHandlerInstance("getindexstorageinfo", insance => insance.RegisterExtensionPoint("GetResult", "getindexstorageinfo"))
                .RegisterHandlerInstance("getnumberofdocuments", insance => insance
                    .RegisterExtensionPoint("Move", "getnumberofdocuments"))
                .RegisterHandlerInstance(
                    "createsession",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "createsession"))
                .RegisterHandlerInstance(
                    "savesession",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "savesession"))
                .RegisterHandlerInstance(
                    "attachdocumentsession",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "attachdocumentsession"))
                .RegisterHandlerInstance(
                    "detachdocumentsession",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "detachdocumentsession"))
                .RegisterHandlerInstance(
                    "detachfile",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "detachfile"))
                .RegisterHandlerInstance(
                    "getsession",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "getsession"))
                .RegisterHandlerInstance(
                    "removesession",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "removesession"))
                .RegisterHandlerInstance(
                    "getdocument",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "getdocument"))
                .RegisterHandlerInstance(
                    "getdocumentbyid",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "getdocumentbyid"))
                .RegisterHandlerInstance(
                    "getconfigmetadata",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "getconfigmetadata"))
                .RegisterHandlerInstance(
                    "getconfigmetadatalist",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "getconfigmetadatalist"))
                .RegisterHandlerInstance(
                    "getdocumentcrossconfig",
                    insance => insance
                        .RegisterExtensionPoint
                        ("Move",
                            "getdocumentcrossconfig"))
                .RegisterHandlerInstance(
                    "getbyquery",
                    instance => instance
                        .RegisterExtensionPoint
                        ("GetResult",
                            "getbyquery"))
                .RegisterHandlerInstance(
                    "setdocument",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "setdocument"))
                .RegisterHandlerInstance(
                    "createdocument",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "createdocument"))
                .RegisterHandlerInstance(
                    "deletedocument",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("Move",
                                "deletedocument"))
                .SetResultHandler(
                    HttpResultHandlerType
                        .BadRequest)
                );

            servicesConfiguration.AddRegistration("configuration", "ApplyEvents", instance => instance
                .RegisterHandlerInstance
                ("updatedocument",
                    handler => handler
                        .RegisterExtensionPoint
                        ("FilterEvents",
                            "filterupdateevents")
                        .RegisterExtensionPoint
                        ("Move",
                            "setdocument"))
                .SetResultHandler(
                    HttpResultHandlerType
                        .BadRequest));

            servicesConfiguration.AddRegistration("configuration", "Upload", reg => reg
                .RegisterHandlerInstance(
                    "uploadbinarycontent",
                    insance =>
                        insance
                            .RegisterExtensionPoint(
                                "Upload",
                                "uploadbinarycontent")));


            servicesConfiguration.AddRegistration("configuration", "UrlEncodedData", reg => reg
                .RegisterHandlerInstance
                ("downloadbinarycontent",
                    insance =>
                        insance
                            .RegisterExtensionPoint
                            ("ProcessUrlEncodedData",
                                "downloadbinarycontent"))
                .SetResultHandler(
                    HttpResultHandlerType
                        .ByteContent));
        }
    }
}