using System;
using Autofac;

namespace InfinniPlatform.Hosting.WebApi.Implementation
{
	public static class Common
	{
		public static ContainerBuilder BuilderContainerBuilder(Action<ContainerBuilder> register)
		{
			var builder = new ContainerBuilder();
			if (register != null)
			{
				register(builder);
			}
			return builder;
		}

		public static IContainer Complete(this ContainerBuilder containerBuilder)
		{
			return containerBuilder.Build();
		}

		public static IContainer CreateContainer()
		{
			return BuilderContainerBuilder(container => { }).Complete();
		}
	}
}