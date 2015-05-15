using System.Text;

namespace InfinniPlatform.Esia.Implementation
{
	static class StringBuilderExtensions
	{
		public static StringBuilder AppendAttribute(this StringBuilder target, string name, string value)
		{
			return target.Append(" ").Append(name).Append("=\"").Append(value).Append("\"");
		}
	}
}