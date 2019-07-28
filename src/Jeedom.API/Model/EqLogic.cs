using Jeedom.API.Mvvm;
using Newtonsoft.Json;

//using System.Diagnostics;

namespace Jeedom.API.Model
{
    public partial class EqLogic : JdItem
    {
        [JsonProperty(PropertyName = "cmds")]
        private ObservableCollectionEx<Command> _Cmds;

        private double _DateTime;

        [JsonProperty(PropertyName = "display")]
        private EqLogicDisplay _Display;

        [JsonProperty(PropertyName = "eqType_name")]
        private string _EqTypeName;

        private RelayCommand<object> _ExecCommandByLogicalID;

        private RelayCommand<object> _ExecCommandByName;

        private RelayCommand<object> _ExecCommandByType;

        [JsonProperty(PropertyName = "isEnable")]
        [JsonConverter(typeof(JsonConverters.BooleanJsonConverter))]
        private bool _IsEnable;

        [JsonProperty(PropertyName = "isVisible")]
        [JsonConverter(typeof(JsonConverters.BooleanJsonConverter))]
        private bool _IsVisible = true;

        [JsonProperty(PropertyName = "logicalId")]
        private string _LogicalId;

        [JsonProperty(PropertyName = "name")]
        private string _Name;

        [JsonProperty(PropertyName = "object_id")]
        private string _ObjectId;

        private JdObject _Parent;

        private bool _Updating;

        public EqLogic()
        {
        }
    }
}