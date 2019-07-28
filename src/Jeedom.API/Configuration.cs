using System;

namespace Jeedom.API
{
	public class Configuration
	{
		public static string Host
		{ get; set; }

		public static string Path
		{ get; set; }

		public static string DnsUri
		{ get; set; }

		public static bool IsSelfSigned
		{ get; set; }

		public static bool UseSSL
		{ get; set; }

		public static string ApiKey
		{ get; set; }

		public static Uri Uri
		{
			get
			{
				var uri = new UriBuilder(UseSSL ? "https" : "http", Host, UseSSL ? 443 : 80, Path);
				return uri.Uri;
			}
		}
	}
}