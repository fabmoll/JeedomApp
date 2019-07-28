using Jeedom.API.Mvvm;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Jeedom.API.Model
{
	[DataContract]
    public class Command : INotifyPropertyChanged
    {
        #region Private Fields

        private double _DateTime;

        [DataMember(Name = "display")]
        private CommandDisplay _Display;

        [DataMember(Name = "eqLogic_id")]
        private string _EqLogic_id;

        private RelayCommand<object> _ExecCommand;

        [DataMember(Name = "id")]
        private string _Id;

        [DataMember(Name = "isVisible")]
        [JsonConverter(typeof(JsonConverters.BooleanJsonConverter))]
        private bool _IsVisible;

        [DataMember(Name = "logicalId")]
        private string _LogicalId;

        [DataMember(Name = "name")]
        private string _Name;

        private EqLogic _Parent;

        [DataMember(Name = "subType")]
        private string _SubType;

        [DataMember(Name = "type")]
        private string _Type;

        [DataMember(Name = "unite")]
        private string _Unite;

        private bool _Updating = false;

        [DataMember(Name = "currentValue")]
        private string _Value = "";

        private ParametersOption _OptionValue = new ParametersOption();

        #endregion Private Fields

        #region Public Properties

        public double DateTime { get { return _DateTime; } set { Set(ref _DateTime, value); } }

        public CommandDisplay Display { get { if (_Display == null) return new CommandDisplay(); else return _Display; } set { Set(ref _Display, value); } }

        public string EqLogic_id { get { return _EqLogic_id; } set { Set(ref _EqLogic_id, value); } }

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
                        CmdParameters.id = Id;
                        CmdParameters.name = Name;
                        CmdParameters.options = OptionValue;
                        await RequestViewModel.Instance.ExecuteCommand(this, CmdParameters);
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        await RequestViewModel.Instance.UpdateTask();

                        Updating = false;
                    }
                    catch (Exception) { }

        }
        public string Id { get { return _Id; } set { Set(ref _Id, value); } }

        public string LogicalId { get { return _LogicalId; } set { Set(ref _LogicalId, value); } }

        public bool IsVisible { get { return _IsVisible; } set { Set(ref _IsVisible, Convert.ToBoolean(value)); } }

        public String Name { get { return _Name; } set { Set(ref _Name, value); } }

        public string Type { get { return _Type; } set { Set(ref _Type, value); } }

        public string SubType { get { return _SubType; } set { Set(ref _SubType, value); } }

        public string Unite { get { return _Unite; } set { Set(ref _Unite, value); } }

        public bool Updating { get { return _Updating; } set { Set(ref _Updating, value); } }

        public string Value
        {
            get
            {
                if (_Value != "" && _Value != null)
                {
                    switch (_SubType)
                    {
                        case "numeric":
                            return _Value.Replace('.', ',');
                    }
                }
                return _Value;
            }

            set
            {
                Set(ref _Value, value);

                if (_Value != "" && _Value != null)
                {
                    switch (_SubType)
                    {
                        case "slider":
                            OptionValue.slider = Convert.ToDouble(_Value);
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