using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charts
{



    abstract class PointModel : INotifyPropertyChanged
    {
      
        public System.DateTime DateTime { get; set; }

        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
    class OHLCPointModel : PointModel
    {
        public OHLCPointModel() { }
        public OHLCPointModel(double open, double high, double low, double close, DateTime time)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            DateTime = time;
        }


        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public override event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    class ValuePointModel : PointModel
    {

        public ValuePointModel(double value, DateTime time)
        {
            Value = value;
            DateTime = time;
        }

        private double _value;

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public override event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }



}
