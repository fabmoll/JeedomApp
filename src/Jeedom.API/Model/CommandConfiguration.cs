using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
namespace Jeedom.API.Model
{
    [DataContract]
    public class CommandConfiguration
    {

        [DataMember(Name = "minValue")]
        private double _minValue=0;

        public double minValue

        {
            get
            {
                return _minValue;
            }

            set
            {
                _minValue = Convert.ToDouble(value);
            }
        }
        [DataMember(Name = "maxValue")]
        private double _maxValue=255;

        public double maxValue

        {
            get
            {
                return _maxValue;
            }

            set
            {
                _maxValue = Convert.ToDouble(value);
            }
        }

    }
}