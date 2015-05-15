using System.Threading.Tasks;

using Microsoft.Owin;
using Microsoft.Owin.Security;

using InfinniPlatform.Esia.Implementation.StateData;

namespace InfinniPlatform.Esia.Middleware
{
	static class Constants
	{
		public const string DefaultAuthenticationType = "Esia";

		public static readonly PathString DefaultCallbackPath = new PathString("/signin-esia");

		public static readonly ISecureDataFormat<AuthenticationProperties> DefaultStateDataFormat = new DefaultStateDataFormat();

		public static readonly Task NullTask = Task.FromResult<object>(null);
	}
}