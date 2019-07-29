using System;
using System.Collections.Generic;
using System.Text;

namespace Jeedom.API.Model
{
	public class RootJeedomObjects
	{
		public string jsonrpc { get; set; }
		public int id { get; set; }
		public List<JeedomObject> result { get; set; }
	}
}
