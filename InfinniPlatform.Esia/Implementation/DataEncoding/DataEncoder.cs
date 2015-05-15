using System;

namespace InfinniPlatform.Esia.Implementation.DataEncoding
{
	sealed class DataEncoder : IDataEncoder
	{
		public string EncodeBytes(byte[] data)
		{
			if (data != null)
			{
				return Uri.EscapeDataString(Convert.ToBase64String(data));
			}

			return null;
		}

		public string EncodeString(string data)
		{
			if (data != null)
			{
				return Uri.EscapeDataString(data);
			}

			return null;
		}

		public byte[] DecodeBytes(string data)
		{
			if (data != null)
			{
				return Convert.FromBase64String(Uri.UnescapeDataString(data));
			}

			return null;
		}

		public string DecodeString(string data)
		{
			if (data != null)
			{
				return Uri.UnescapeDataString(data);
			}

			return null;
		}
	}
}