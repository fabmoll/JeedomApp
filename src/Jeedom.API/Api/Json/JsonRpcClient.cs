using Jeedom.API.Api.Json.Event;
using Jeedom.API.Api.Json.Response;
using Jeedom.API.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Jeedom.API.Api.Json
{
	public class JsonRpcClient
	{
		static private int Id;
		private Error error;
		private Parameters parameters;
		private string rawResponse;

		public JsonRpcClient(Parameters parameters)
		{
			parameters = parameters;
		}

		public JsonRpcClient()
		{
			parameters = new Parameters();
		}

		public Error Error
		{
			get
			{ return error; }
		}

		public EventResult GetEvents()
		{
			try
			{
				JObject json = JObject.Parse(rawResponse);
				var event_result = new EventResult();
				var result = json["result"].Children();
				event_result.DateTime = json["result"]["datetime"].Value<double>();
				foreach (var e in json["result"]["result"].Children())
				{
					switch (e["name"].Value<string>())
					{
						case "cmd::update":
							var evcmd = JsonConvert.DeserializeObject<Event<EventOptionCmd>>(e.ToString());
							event_result.Result.Add(evcmd);
							break;

						case "eqLogic::update":
							var eveq = JsonConvert.DeserializeObject<Event<EventOptionEqLogic>>(e.ToString());
							event_result.Result.Add(eveq);
							break;

						default:
							var evdef = JsonConvert.DeserializeObject<JdEvent>(e.ToString());
							event_result.Result.Add(evdef);
							break;
					}
				}

				return event_result;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public T GetRequestResponseDeserialized<T>()
		{
			try
			{
				var resp = JsonConvert.DeserializeObject<T>(rawResponse);
				return resp;
			}
			catch (JsonException ex)
			{
				return default(T);
			}
			catch (Exception)
			{
				return default(T);
			}
		}

		public async Task<bool> SendRequest(string command)
		{
			parameters.apikey = RequestViewModel.config.ApiKey;

			try
			{
				rawResponse = await Request(command, parameters);

				var resp = JsonConvert.DeserializeObject<ResponseError>(rawResponse);
				error = resp.error;
				if (error == null)
					return true;
				else
					return false;
			}
			catch (Exception e)
			{
				error = new Error();
				error.code = "-1";
				error.message = "Une erreur s'est produite lors de l'exécution de votre requête !" + Environment.NewLine + e.Message;
				return false;
			}
		}

		public void SetParameters(Parameters parameters)
		{
			parameters = parameters;
		}

		/*private T DeserializeFromJson<T>(string dataToDeserialize)
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(dataToDeserialize));
            stream.Position = 0;
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            return (T)ser.ReadObject(stream);
        }

        private string SerializeToJson<T>(T objectToSerialize)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            ser.WriteObject(stream, objectToSerialize);
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);

            return sr.ReadToEnd();
        }*/

		private async Task<String> Request(string command, Parameters parameters)
		{
			var uri = new Uri(RequestViewModel.config.Uri + "core/api/jeeApi.php");

			//var filter = new HttpBaseProtocolFilter();
			//if (RequestViewModel.config.Address.ProtocolType == Network.Address.Protocol.SelfSigned)
			//{
			//    filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
			//    filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);
			//}

			HttpClient httpClient = new HttpClient();

			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			Request requete = new Request();

			requete.parameters = parameters;
			requete.method = command;
			requete.id = Interlocked.Increment(ref Id);
			var requeteJson = "request=" + JsonConvert.SerializeObject(requete);
			var content = new StringContent(requeteJson, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
			string serialized = string.Empty;

			var cancellationTokenSource = new CancellationTokenSource(2000); //timeout
			try
			{
				var response = await httpClient.PostAsync(uri, content);
				serialized = await response.Content.ReadAsStringAsync();
			}
			catch (TaskCanceledException)
			{
				//System.Diagnostics.Debug.WriteLine("JsonRPC Timeout");
			}
			catch (System.Runtime.InteropServices.COMException e)
			{
				//web exceptions such as 404, 401, 500 etc
				throw e;
			}

			httpClient.Dispose();

			return serialized;
		}
	}
}