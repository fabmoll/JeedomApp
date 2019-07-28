using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jeedom.API.Api.Json.Event
{
    [DataContract]
    public class EventResult
    {
        [DataMember(Name = "datetime")]
        public double DateTime;

        [DataMember(Name = "result")]
        public List<JdEvent> Result = new List<JdEvent>();
    }
}