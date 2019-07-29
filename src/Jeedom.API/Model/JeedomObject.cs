using System;
using System.Collections.Generic;
using System.Text;

namespace Jeedom.API.Model
{
	public class JeedomObject
	{
		public string id { get; set; }
		public string name { get; set; }
		public string father_id { get; set; }
		public string isVisible { get; set; }
		public object position { get; set; }
		public Configuration configuration { get; set; }
		public Display display { get; set; }
		public object[] image { get; set; }
		public string img { get; set; }
		public List<EqLogic> eqLogics { get; set; }
	}
}
