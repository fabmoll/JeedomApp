using System.Runtime.Serialization;

namespace Jeedom.API.Model
{
    [DataContract]
    public class Error
    {
        [DataMember]
        public string code;

        [DataMember]
        public string message;
    }
}