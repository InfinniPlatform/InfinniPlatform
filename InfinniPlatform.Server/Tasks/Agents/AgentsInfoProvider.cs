﻿using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Server.Settings;
using InfinniPlatform.Server.Tasks.Infinni.Node;

namespace InfinniPlatform.Server.Tasks.Agents
{
    /// <summary>
    /// Предоставляет информацию об агентах.
    /// </summary>
    public class AgentsInfoProvider
    {
        public AgentsInfoProvider(ServerSettings settings,
                                  IJsonObjectSerializer serializer)
        {
            _settings = settings;
            _serializer = serializer;
        }

        private readonly IJsonObjectSerializer _serializer;
        private readonly ServerSettings _settings;


        public List<AgentInfo> GetAgentsInfoList()
        {
            var agentsInfoText = File.ReadAllText(_settings.AgentsInfoFilePath);

            var agentsInfo = _serializer.Deserialize<List<AgentInfo>>(agentsInfoText);

            return agentsInfo;
        }

        public void AddInfo(IHttpRequest request)
        {
            var agentsInfoList = GetAgentsInfoList();

            agentsInfoList.Add(CreateAgentInfo(request));

            SaveAgentsList(agentsInfoList);
        }

        public void EditInfo(IHttpRequest request)
        {
            Guid id = request.Form.Id;

            var agentsInfoList = GetAgentsInfoList();

            agentsInfoList.RemoveAll(info => info.Id == id);
            agentsInfoList.Add(CreateAgentInfo(request));

            SaveAgentsList(agentsInfoList);
        }

        public void RemoveInfo(IHttpRequest request)
        {
            Guid id = request.Form.Id;

            var agentsInfoList = GetAgentsInfoList();

            agentsInfoList.RemoveAll(info => info.Id == id);

            SaveAgentsList(agentsInfoList);
        }


        private static AgentInfo CreateAgentInfo(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = HttpServiceHelper.ParseInt(request.Form.Port);
            string name = request.Form.Name;

            return new AgentInfo(name, address, port);
        }

        private void SaveAgentsList(IEnumerable<AgentInfo> agentsInfo)
        {
            File.WriteAllBytes(_settings.AgentsInfoFilePath, _serializer.Serialize(agentsInfo));
        }
    }
}