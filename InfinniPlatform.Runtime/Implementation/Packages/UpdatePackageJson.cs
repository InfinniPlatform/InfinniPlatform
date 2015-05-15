using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Runtime.Implementation.Packages
{
	public sealed class UpdatePackageJson
	{
		public string JsonConfig { get; set; }

		public IEnumerable<UpdatePackage> Packages { get; set; }
	}
}
