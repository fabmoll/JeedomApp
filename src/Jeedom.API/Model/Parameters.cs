using System.Runtime.Serialization;

namespace Jeedom.API.Model
{
    [DataContract]
    public class Parameters
    {
        [DataMember]
        public string login;
        [DataMember]
        public string password;
        [DataMember]
        public string twoFactorCode;
        [DataMember]
        public string apikey;
        [DataMember(IsRequired = false)]
        public string eqLogic_id;
        [DataMember]
        public string id;
        [DataMember]
        public string name;
        [DataMember]
        public string query;
        [DataMember]
        public string key;
        [DataMember]
        public string plugin;
        [DataMember]
        public string platform;
        [DataMember]
        public ParametersOption options;
        [DataMember(IsRequired = false)]
        public string state;
        [DataMember(IsRequired = false)]
        public string object_id;
        [DataMember(IsRequired = false)]
        public double datetime;
    }
}
