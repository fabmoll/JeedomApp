using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jeedom.API.Api.Json.Event
{
    [DataContract]
    public class Event<T> : JdEvent
    {
        [DataMember(Name = "option")]
        public T Option;
    }
}