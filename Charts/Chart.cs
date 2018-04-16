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

using System.Threading;

namespace Charts
{
    public partial class Chart : Form
    {
        private void Form1_Load(object sender, EventArgs e)
        {
            CommandManager.load();
            updateGraph = new Thread(updateChart);
            updateGraph.Start();
            
        }

        private string dateTimeFormat = "yyyy-MM-dd HH:mm";

        private static Dictionary<string, Thread> idFunctionThread = new Dictionary<string, Thread>();
        private static Dictionary<string, Series> idSeries = new Dictionary<string, Series>();
        private static Dictionary<string, List<PointModel>> idUpdate = new Dictionary<string, List<PointModel>>();
        private static Thread updateGraph;
        private readonly object syncLock = new object();
        private static bool alive = true;
        private void updateChart()
        {

            while (alive)
            {
                lock (syncLock)
                {
                    foreach (KeyValuePair<string, List<PointModel>> pair in idUpdate)
                    {
                        addPointsToChart(pair.Value, pair.Key);
                        pair.Value.Clear();
                    }
                   
                }
                Thread.Sleep(5000);
            }
            
        }
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

                series.Values = new ChartValues<ValuePointModel>();

            }
            series.PointGeometry = null;
            series.DataLabels = false;

            //series.PointGeometry = null;// ovo je za preformanse ne radi ohlc sa puno tacka
            //series.DataLabels = true;
            series.Title = displayName;
            cartesianChart1.Series.Add(series);
            cartesianChart1.DisableAnimations = false;

            
            idSeries.Add(id, series);
        }
        private bool isSeriesExist(string id)
        {
            return idSeries.ContainsKey(id);
        }
        private void removeSeries(string id)
        {
            idUpdate.Remove(id);
            idFunctionThread.Remove(id);
            idSeries.Remove(id);
        }

        private void addPointsToChart(List<PointModel> points, string id)
        {
            
             if (isSeriesExist(id))
            {
                idSeries[id].Values.AddRange(points);
               
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
            cartesianChart1.AxisY.Add(new Axis
            {
                LabelFormatter = val => Math.Round(val, 2).ToString() + "$",
                MinValue = 0
            });
            //The next code simulates data changes every 500 
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
                List<PointModel> points = DataHandler.JSONtoPoint(json, ref lastRefresh, isOhlc);
                addPointsToUpdateQueue(points, id);
                Console.WriteLine("Points added:" + points.Count.ToString());
                Console.WriteLine("lastRefresh:" + lastRefresh);
                Console.WriteLine("Thread sleeping for");
                Thread.Sleep(interval * 1000 * 60);// staviti 
            }
        }


        private void addPointsToUpdateQueue(List<PointModel> points, string id)
        {
            lock (syncLock)
            {
                if (idUpdate.ContainsKey(id))
                {
                    idUpdate[id].AddRange(points);
                } else {
                    idUpdate.Add(id, points);
                }
                
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
            alive = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btResetZoom_Click(object sender, EventArgs e)
        {
            cartesianChart1.AxisX[0].MinValue = double.NaN;
            cartesianChart1.AxisX[0].MaxValue = double.NaN;
            cartesianChart1.AxisY[0].MinValue = 0;
            cartesianChart1.AxisY[0].MaxValue = double.NaN;
        }
    }
}