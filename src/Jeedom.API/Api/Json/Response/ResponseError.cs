using Jeedom.API.Model;
using System.Runtime.Serialization;

namespace Jeedom.API.Api.Json.Response
{
    [DataContract]
    public class ResponseError
    {
        [DataMember]
        public string id;

        [DataMember]
        public string jsonrpc;

        [DataMember]
        public Error error;
    }
}