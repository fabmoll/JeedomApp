using System.Runtime.Serialization;

namespace Jeedom.API.Model
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string id;

        [DataMember]
        public string date;

        [DataMember]
        public string logicalId;

        [DataMember]
        public string plugin;

        [DataMember]
        public string message;

        [DataMember]
        public string action;

        public string Text
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
    }
}