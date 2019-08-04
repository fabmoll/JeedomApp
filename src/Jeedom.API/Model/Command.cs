using Jeedom.API.JsonConverters;
using Jeedom.API.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Jeedom.API.Model
{
	public class Command : INotifyPropertyChanged
	{
		public string id { get; set; }
		public string logicalId { get; set; }
		public string generic_type { get; set; }
		public string eqType { get; set; }
		public string name { get; set; }
		public string order { get; set; }
		public string type { get; set; }
		public string subType { get; set; }
		public string eqLogic_id { get; set; }
		public string isHistorized { get; set; }
		public string unite { get; set; }
		public object configuration { get; set; }
		public object template { get; set; }
		public object display { get; set; }
		public string value { get; set; }
		[JsonConverter(typeof(BooleanJsonConverter))]
		public bool isVisible { get; set; }
		public object alert { get; set; }
		public object state { get; set; }

		public bool Updating { get; set; }
		public double DateTime { get; set; }


		private RelayCommand<object> _ExecCommand;
		private ParametersOption _OptionValue = new ParametersOption();


		#region Public Properties

		public object Display { get { if (display == null) return new object(); else return display; } set { display = value; } }

		public string EqLogic_id { get { return eqLogic_id; } set { eqLogic_id = value; } }

		public RelayCommand<object> ExecCommand
		{
			get
			{
				_ExecCommand = _ExecCommand ?? new RelayCommand<object>(parameters =>
				{
					SendValue();
				});
				return _ExecCommand;
			}
		}
		private async void SendValue()
		{
			try
			{
				Updating = true;
				Parameters CmdParameters = new Parameters();
				CmdParameters.id = id;
				CmdParameters.name = name;
				CmdParameters.options = OptionValue;
				await RequestViewModel.Instance.ExecuteCommand(this, CmdParameters);
				await Task.Delay(TimeSpan.FromSeconds(1));
				await RequestViewModel.Instance.UpdateTask();

				Updating = false;
			}
			catch (Exception) { }

		}

		public string Value
		{
			get
			{
				if (this.value != "" && this.value != null)
				{
					switch (subType)
					{
						case "numeric":
							return this.value.Replace('.', ',');
					}
				}
				return this.value;
			}

			set
			{
				Set(ref value, value);

				if (this.value != "" && this.value != null)
				{
					switch (subType)
					{
						case "slider":
							OptionValue.slider = Convert.ToDouble(this.value);
							SendValue();
							break;
						case "message":
							break;
						case "color":
							break;
					}
				}
			}
		}

		public ParametersOption OptionValue
		{
			get
			{
				return _OptionValue;
			}

			set
			{
				Set(ref _OptionValue, value);
			}
		}

		#endregion Public Properties

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (!Equals(storage, value))
			{
				storage = value;
				//System.Diagnostics.Debug.WriteLine("cmd update : " + Name + " " + propertyName);
				RaisePropertyChanged(propertyName);
			}
		}

		public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
		   PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		#endregion INotifyPropertyChanged

	}
}
