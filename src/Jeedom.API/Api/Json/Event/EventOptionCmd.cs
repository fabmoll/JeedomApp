using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jeedom.API.Api.Json.Event
{
    [DataContract(Name = "option")]
    public class EventOptionCmd
    {
        [DataMember(Name = "cmd_id")]
        public string CmdId;

        [DataMember(Name = "value")]
        public string Value;
    }
}