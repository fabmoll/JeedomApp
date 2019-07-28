using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jeedom.API.Model
{
    [DataContract]
    public class CustomParameters
    {
        [DataMember(Name = "JeedomAppTemplate")]
        public string JeedomAppTemplate { get; set; }

        [DataMember(Name = "JeedomAppRowSpan")]
        public int JeedomAppRowSpan { get; set; }

        [DataMember(Name = "JeedomAppColSpan")]
        public int JeedomAppColSpan { get; set; }
    }

    [DataContract]
    public class EqLogicDisplay
    {
        /* [DataMember(Name = "hideOndashboard")]
         private bool _hideOndashboard=false;
         public bool hideOndashboard {
             get
             {
                 return _hideOndashboard;
             }
             set
             {
                 if(value!=null)
                     _hideOndashboard=Convert.ToBoolean(value);
             }
         }

         [DataMember(Name = "hideOnmobile")]
         private bool _hideOnmobile = false;
         public bool hideOnmobile
         {
             get
             {
                 return _hideOnmobile;
             }
             set
             {
                 if (value != null)
                     _hideOnmobile = Convert.ToBoolean(value);
             }
         }

         [DataMember(Name = "doNotShowNameOnDashboard")]
         private bool _doNotShowNameOnDashboard = false;
         public bool doNotShowNameOnDashboard
         {
             get
             {
                 return _doNotShowNameOnDashboard;
             }
             set
             {
                 if(value!=null)
                 _doNotShowNameOnDashboard = Convert.ToBoolean(value);
             }
         }

         [DataMember(Name = "doNotShowNameOnView")]
         private bool _doNotShowNameOnView = false;
         public bool doNotShowNameOnView
         {
             get
             {
                     return _doNotShowNameOnView;
             }
             set
             {
                 if (value != null)
                 _doNotShowNameOnView = Convert.ToBoolean(value);
             }
         }

         [DataMember(Name = "doNotShowNameOnMobile")]
         private bool _doNotShowNameOnMobile = false;
         public bool doNotShowNameOnMobile
         {
             get
             {
                 return _doNotShowNameOnMobile;
             }
             set
             {
                 if (value != null)
                     _doNotShowNameOnMobile = Convert.ToBoolean(value);
             }
         }

         [DataMember(Name = "doNotShowObjectNameOnView")]
         private bool _doNotShowObjectNameOnView = false;
         public bool doNotShowObjectNameOnView
         {
             get
             {
                 return _doNotShowObjectNameOnView;
             }
             set
             {
                 if (value != null)
                     _doNotShowObjectNameOnView = Convert.ToBoolean(value);
             }
         }

         [DataMember(Name = "doNotDisplayBatteryLevelOnDashboard")]
         private bool _doNotDisplayBatteryLevelOnDashboard = false;
         public bool doNotDisplayBatteryLevelOnDashboard
         {
             get
             {
                 return _doNotDisplayBatteryLevelOnDashboard;
             }
             set
             {
                 if (value != null)
                     _doNotDisplayBatteryLevelOnDashboard = Convert.ToBoolean(value);
             }
         }

         [DataMember(Name = "doNotDisplayBatteryLevelOnView")]
         private bool _doNotDisplayBatteryLevelOnView = false;
         public bool doNotDisplayBatteryLevelOnView
         {
             get
             {
                 return _doNotDisplayBatteryLevelOnView;
             }
             set
             {
                 if (value != null)
                     _doNotDisplayBatteryLevelOnView = Convert.ToBoolean(value);
             }
         }

         [DataMember(Name = "parameters")]
         public CustomParameters customParameters { get; set; }*/
        [DataMember(Name = "width")]
        public string width { get; set; }
        [DataMember(Name = "height")]
        public string height { get; set; }
    }
}