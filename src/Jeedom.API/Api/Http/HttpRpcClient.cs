using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Jeedom.API.Api.Http
{
	internal class HttpRpcClient
	{
		private string _path;

		public HttpRpcClient(string path)
		{
			_path = path;
		}

		public async Task<bool> SendRequest()
		{
			try
			{
				var uri = new Uri(RequestViewModel.config.Uri + _path);

				//var filter = new HttpBaseProtocolFilter();
				/*   if (config.IsSelfSigned == true)
				   {
					   filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
					   filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);
				   }*/

				var httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
				httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("Mozilla", "5.0").ToString()));
				httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("Firefox", "26.0").ToString()));

				var reponse = await httpClient.GetAsync(uri);
				httpClient.Dispose();
				return reponse.IsSuccessStatusCode;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}