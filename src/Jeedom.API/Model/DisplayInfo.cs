using System.Runtime.Serialization;

namespace Jeedom.API.Model
{
    [DataContract]
    public class DisplayInfo
    {
        [DataMember]
        public string Icon;

        [DataMember]
        public string TagColor;

        [DataMember]
        public string TagTextColor;
    }
}