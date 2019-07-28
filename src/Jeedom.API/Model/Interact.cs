using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Linq;

namespace Jeedom.API.Model
{
    [DataContract]
    public class Interact
    {

        [DataMember]
        public string id;

        [DataMember]
        public string interactDef_id;

        [DataMember]
        public string query;

        #region Private Methods

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Private Methods
    }
}