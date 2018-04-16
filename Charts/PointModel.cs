using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charts
{



    abstract class PointModel 
    {
      
        public System.DateTime DateTime { get; set; }

       
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

    }

    class ValuePointModel : PointModel
    {

        public ValuePointModel(double value, DateTime time)
        {
            Value = value;
            DateTime = time;
        }


        public double Value { get; set; }


    }



}
