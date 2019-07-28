using System;
using System.ComponentModel;
namespace Jeedom.API.Model
{
    public class ParametersOption : INotifyPropertyChanged
    {
        public ParametersOption()
        {
        }
        private double _slider=0;
        public double slider
        {
            get
            {
                return _slider;
            }

            set
            {

                    _slider = value;
                NotifyPropertyChanged();
            }
        }
        private string _title;
        public string title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }
        public string _message;
        public string message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
                NotifyPropertyChanged();
            }
        }
        //public SolidColorBrush _color;
        //public SolidColorBrush color
        //{
        //    get
        //    {
        //        return _color;
        //    }

        //    set
        //    {
        //        _color = value;
        //        NotifyPropertyChanged();
        //    }
        //}
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}