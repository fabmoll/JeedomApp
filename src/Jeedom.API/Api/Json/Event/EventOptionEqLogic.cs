﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jeedom.API.Api.Json.Event
{
    [DataContract(Name = "option")]
    public class EventOptionEqLogic
    {
        [DataMember(Name = "eqLogic_id")]
        public string EqLogicId;
    }
}