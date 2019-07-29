using System;
using System.Collections.Generic;
using System.Text;

namespace Jeedom.API.Model
{
	public class Mode
	{
		public string name { get; set; }
		public string icon { get; set; }
		public Inaction[] inAction { get; set; }
		public object[] outAction { get; set; }
	}
}
