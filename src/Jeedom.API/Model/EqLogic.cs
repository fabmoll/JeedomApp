using Jeedom.API.JsonConverters;
using Jeedom.API.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jeedom.API.Model
{
	public class EqLogic : JdItem
	{
		public string name { get; set; }
		public string logicalId { get; set; }
		public object generic_type { get; set; }
		public string object_id { get; set; }
		public string eqType_name { get; set; }
		public object eqReal_id { get; set; }
		[JsonConverter(typeof(BooleanJsonConverter))]
		public bool isVisible { get; set; }
		public string isEnable { get; set; }
		public EqLogicConfiguration configuration { get; set; }
		public string timeout { get; set; }
		public object category { get; set; }
		public EqLogicDisplay display { get; set; }
		public string order { get; set; }
		public string comment { get; set; }
		public string tags { get; set; }
		public object status { get; set; }
		[JsonProperty(PropertyName = "cmds")]
		public ObservableCollectionEx<Command> _Cmds;

		public double DateTime { get; set; }
		public bool Updating { get; set; }

		private RelayCommand<object> _ExecCommandByLogicalID;

		private RelayCommand<object> _ExecCommandByName;

		private RelayCommand<object> _ExecCommandByType;


		private JeedomObject _Parent;

		private bool _Updating;

		public ObservableCollection<Command> GetActionsCmds()
		{
			if (Cmds != null)
			{
				IEnumerable<Command> results = Cmds.Where(c => c.type == "action");
				return new ObservableCollection<Command>(results);
			}
			else
				return new ObservableCollection<Command>();
		}

		public ObservableCollection<Command> GetInformationsCmds()
		{
			if (Cmds != null)
			{
				IEnumerable<Command> results = Cmds.Where(c => c.type == "info");
				return new ObservableCollection<Command>(results);
			}
			else
				return new ObservableCollection<Command>();
		}

		public ObservableCollection<Command> GetVisibleCmds()
		{
			if (Cmds != null)
			{
				IEnumerable<Command> results = Cmds.Where(c => c.isVisible && c.logicalId != null);
				return new ObservableCollection<Command>(results);
			}
			else
				return new ObservableCollection<Command>();
		}

		private async Task ExecCommand(Command cmd)
		{
			if (cmd != null)
			{
				this.Updating = true;
				await RequestViewModel.Instance.ExecuteCommand(cmd);
				//await Task.Delay(TimeSpan.FromSeconds(3));
				await RequestViewModel.Instance.UpdateEqLogic(this);

				Updating = false;
			}
		}

		public ObservableCollectionEx<Command> Cmds
		{
			get
			{
				return _Cmds;
			}

			set
			{
				_Cmds = value;
				NotifyPropertyChanged();
				NotifyPropertyChanged("VisibleCmds");
			}
		}

		public int ColSpan
		{
			get
			{
				var _visibleCmds = GetVisibleCmds().Count();
				if (_visibleCmds > 3)
					return 6;
				else
					return _visibleCmds * 2;
			}
		}


		public int RowSpan
		{
			get
			{
				var _visibleCmds = GetVisibleCmds().Count();
				if (_visibleCmds > 3)
					return (int)Math.Ceiling((double)_visibleCmds / 3) * 2;
				else
					return 2;
			}
		}

		public ObservableCollection<Command> VisibleCmds
		{
			get
			{
				return GetVisibleCmds();
			}
		}

		public RelayCommand<object> ExecCommandByLogicalID
		{
			get
			{
				_ExecCommandByLogicalID = _ExecCommandByLogicalID ?? new RelayCommand<object>(async parameters =>
				{
					var cmd = Cmds.Where(c => c.logicalId.ToLower() == parameters.ToString().ToLower()).FirstOrDefault();
					if (cmd != null)
						await ExecCommand(cmd);
				});
				return _ExecCommandByLogicalID;
			}
		}

		/// <summary>
		/// Exécute une commande à partir de son "name"
		/// </summary>
		public RelayCommand<object> ExecCommandByName
		{
			get
			{
				_ExecCommandByName = _ExecCommandByName ?? new RelayCommand<object>(async parameters =>
				{
					try
					{
						var cmd = Cmds.Where(c => c.name.ToLower() == parameters.ToString().ToLower()).FirstOrDefault();
						if (cmd != null)
							await ExecCommand(cmd);
					}
					catch (Exception) { }
				});
				return _ExecCommandByName;
			}
		}

		/// <summary>
		/// Exécute une commande à partir de son "generic_type"
		/// </summary>
		public RelayCommand<object> ExecCommandByType
		{
			get
			{
				_ExecCommandByType = _ExecCommandByType ?? new RelayCommand<object>(async parameters =>
				{
					try
					{
						//var cmd = Cmds.Where(c => c.Display.First().generic_type == parameters.ToString()).FirstOrDefault();
						//if (cmd != null)
						//	await ExecCommand(cmd);
					}
					catch (Exception) { }
				});
				return _ExecCommandByType;
			}
		}

	}

}
