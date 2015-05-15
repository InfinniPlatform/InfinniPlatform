﻿namespace InfinniPlatform.Api.LocalRouting
{
	public interface ILocalHostServer
	{
		void RegisterStartConfiguration(string configurationId);
		void RegisterAssembly(string configId, string assemblyName);
		void Start();
		void Stop();
	}
}