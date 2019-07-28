using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Jeedom.API.Model
{
    public class JdItem : INotifyPropertyChanged
    {
        [JsonProperty(PropertyName = "id")]
        protected string id;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id
        {
            get => id;
            set
            {
                id = value;
                NotifyPropertyChanged();
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}