﻿using System.Threading.Tasks;

using InfinniPlatform.Agent.InfinniNode;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Agent.RestApi
{
    /// <summary>
    /// REST-сервис для взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public class NodeHttpService : IHttpService
    {
        public NodeHttpService(IConnector connector)
        {
            _connector = connector;
        }

        private readonly IConnector _connector;


        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "node";

            builder.Post["install"] = InstallApp;
            builder.Post["uninstall"] = UninstallApp;
            builder.Post["init"] = InitApp;
            builder.Post["start"] = StartApp;
            builder.Post["stop"] = StopApp;
            builder.Post["restart"] = RestartApp;
            builder.Post["apps"] = GetInstalledAppsInfo;

            builder.Get["apps"] = GetInstalledAppsInfo;
        }

        private async Task<object> InstallApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;
            string version = httpRequest.Form.Version;
            string instance = httpRequest.Form.Instance;
            string source = httpRequest.Form.Source;
            bool? allowPrerelease = httpRequest.Form.AllowPrerelease;

            var processResult = await _connector.InstallApp(appName, version, instance, source, allowPrerelease);

            return processResult;
        }

        private async Task<object> UninstallApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _connector.UninstallApp(appName);

            return processResult;
        }

        private async Task<object> InitApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _connector.InitApp(appName);

            return processResult;
        }

        private async Task<object> StartApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _connector.StartApp(appName);

            return processResult;
        }

        private async Task<object> StopApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _connector.StopApp(appName);

            return processResult;
        }

        private async Task<object> RestartApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _connector.RestartApp(appName);

            return processResult;
        }

        private async Task<object> GetInstalledAppsInfo(IHttpRequest httpRequest)
        {
            return await _connector.GetInstalledAppsInfo();
        }
    }
}