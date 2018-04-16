using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts; //Core of the library
using LiveCharts.Wpf; //The WPF controls
using LiveCharts.WinForms;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Geared;
using System.Threading;

namespace Charts
{
    public partial class Chart : Form
    {
        private void Form1_Load(object sender, EventArgs e)
        {
            CommandManager.load();
           
            
        }

        private string dateTimeFormat = "yyyy-MM-dd HH:mm";

        private static Dictionary<string, Thread> idFunctionThread = new Dictionary<string, Thread>();
        private static Dictionary<string, Series> idSeries = new Dictionary<string, Series>();

        private void createNewSeries(bool isOHLC, string id, string displayName)
        {

            if (isSeriesExist(id))
            {
                return;
            }
            Series series;
           
            if (isOHLC)
            {
               
                series = new OhlcSeries();
                series.Values = new ChartValues<OHLCPointModel>();
            }
            else
            { 
                series = new LineSeries();
                
                series.Values = new GearedValues<ValuePointModel>();
                ((GearedValues<ValuePointModel>)series.Values).Quality = Quality.Medium;

            }
            

            //series.PointGeometry = null;// ovo je za preformanse ne radi ohlc sa puno tacka
            //series.DataLabels = true;
            series.Title = displayName;
            cartesianChart1.Series.Add(series);


            idSeries.Add(id, series);
        }
        private bool isSeriesExist(string id)
        {
            return idSeries.ContainsKey(id);
        }
        private void removeSeries(string id)
        {
            idFunctionThread.Remove(id);
            idSeries.Remove(id);
        }

        private void addPointToChart(PointModel point, string id)
        {
            if (isSeriesExist(id))
            {
                idSeries[id].Values.Add(point);
               
            }

        }
        private string convertTimeToY(double value)
        {
            if (value < 0)
            {
                value = 0;
            }
            try
            {
                return new System.DateTime(TimeSpan.FromMinutes(value).Ticks).ToString(dateTimeFormat);
            }
            catch (Exception)
            {

                return "Server error";
            }
            
        }

        public Chart()
        {
            InitializeComponent();
            cartesianChart1.Series = new SeriesCollection();

            var mapper = Mappers.Financial<OHLCPointModel>()
                 .X((value, index) => TimeSpan.FromTicks(value.DateTime.Ticks).TotalMinutes)
                .Open(value => value.Open)
                .High(value => value.High)
                .Low(value => value.Low)
                .Close(value => value.Close);

            var mapperXY = Mappers.Xy<ValuePointModel>()
               .X((value) => TimeSpan.FromTicks(value.DateTime.Ticks).TotalMinutes)
               .Y(value => value.Value);

            LiveCharts.Charting.For<ValuePointModel>(mapperXY, SeriesOrientation.Horizontal);
            LiveCharts.Charting.For<OHLCPointModel>(mapper, SeriesOrientation.Horizontal);

            cartesianChart1.AxisX.Add(new Axis
            {
                DisableAnimations = false,
                LabelFormatter = value => convertTimeToY(value),
                Title = "Vreme"
            });

            //The next code simulates data changes every 500 ms

            cartesianChart1.Zoom = ZoomingOptions.X;

        }
        
        private void updatedGraph(object o)
        {
            Console.WriteLine("Updating graph thread started");
            CommandManager cm = new CommandManager();
            object[] arry = (object[])o;
            Dictionary<string, string> command = (Dictionary<string, string>)(arry[0]);
            int interval = 0;
            bool isOhlc = false;

            if (arry[1] is bool)
            {
                isOhlc = (bool)arry[1];
    
            }

            if (arry[2] is int)
            {
                interval = (int)arry[2];

            }


            string id = CommandManager.getId(command);
           
            string lastRefresh = "";

            
            while (isSeriesExist(id))//sta se desava sa grafom poruku pirkazivati na tabu i poslenji refresh
            {
                Console.WriteLine("Thread sober");
                string json = cm.excuteCommand(command);
                lastRefresh = "2018-04-16 15:59";
                List<PointModel> points = DataHandler.JSONtoPoint(json, ref lastRefresh, isOhlc);
                Console.WriteLine("Points added:" + points.Count.ToString());
                Console.WriteLine("lastRefresh:" + lastRefresh);
                foreach (PointModel p in points)
                {
                    addPointToChart(p, id);
                }
                Console.WriteLine("Thread sleeping for");
                Thread.Sleep(interval * 1000 * 60);// staviti 
            }
        }


        private void btOdabirGrafa_Click(object sender, EventArgs e)
        {
            FunctionAddDialog d = new FunctionAddDialog();
            d.ShowDialog();
            if(d.DialogResult== DialogResult.OK)
            {
                CommandManager cm = new CommandManager();
                Dictionary<string, string> command = d.generateCommand();
                string id = CommandManager.getId(command);
                bool isOhlc = d.isSeriesOhlc();
                int interval = d.getInterval();
                createNewSeries(isOhlc, id, d.getSeriesDisplayName());
                Thread newThread = new Thread(updatedGraph);
                object[] arry = new object[3];
                arry[0] = command;
                arry[1] = isOhlc;
                arry[2] = interval;
                newThread.Start(arry);
                
                
            }
        }


        private void Chart_FormClosed(object sender, FormClosedEventArgs e)
        {
           
            idFunctionThread.Clear();
            idSeries.Clear();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}