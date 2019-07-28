using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jeedom.API.Model
{
    [DataContract]
    public class CommandDisplay
    {
        [DataMember(Name = "icon")]
        public string icon { get; set; }

        [DataMember(Name = "generic_type")]
        private string _generic_type;

        public string generic_type

        {
            get
            {
                if (_generic_type == null)
                    return "NONE";
                else
                    return _generic_type;
            }

            set
            {
                _generic_type = value;
            }
        }

        [DataMember(Name = "hideOndashboard")]
        public string hideOndashboard { get; set; }

        [DataMember(Name = "hideOnmobile")]
        public string hideOnmobile { get; set; }

        [DataMember(Name = "doNotShowNameOnDashboard")]
        public string doNotShowNameOnDashboard { get; set; }

        [DataMember(Name = "doNotShowNameOnView")]
        public string doNotShowNameOnView { get; set; }

        [DataMember(Name = "doNotShowNameOnMobile")]
        public string doNotShowNameOnMobile { get; set; }

        [DataMember(Name = "doNotShowStatOnDashboard")]
        public string doNotShowStatOnDashboard { get; set; }

        [DataMember(Name = "doNotShowStatOnView")]
        public string doNotShowStatOnView { get; set; }

        [DataMember(Name = "doNotShowStatOnMobile")]
        public string doNotShowStatOnMobile { get; set; }

        [DataMember(Name = "forceReturnLineBefore")]
        public string forceReturnLineBefore { get; set; }

        [DataMember(Name = "forceReturnLineAfter")]
        public string forceReturnLineAfter { get; set; }

        //[DataMember(Name = "parameters")]
        //public string parameters { get; set; }
    }
}