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
using System.Diagnostics;
using System.IO;

namespace Charts
{
    public partial class Chart : Form
    {

        private void deserialize(string path)
        {
            StreamReader sr = new StreamReader(path);
            string line=null;
            if((line = sr.ReadLine()) != null)
            {
                string[] size = line.Split(',');
                Width = Convert.ToInt32(size[0]);
                Height = Convert.ToInt32(size[1]);
            }

            if ((line = sr.ReadLine()) != null)
            {
                string[] size = line.Split(',');
                int x = Convert.ToInt32(size[0]);
                int y = Convert.ToInt32(size[1]);
                Location = new Point(x, y);
            }
            while ((line = sr.ReadLine()) != null)
            {
                string id = line;
              
                int interval = 5;
                string[] help = id.Split('&');
                string displayName = "";
                string fun = "";
                foreach(string h in help)
                {
                    if (h.Contains("symbol"))
                    {
                        displayName = h.Split('=')[1];
                    }
                    if (h.Contains("function"))
                    {
                        fun = h.Split('=')[1];
                    }
                }
                bool isOhlc = CommandManager.GetFunctionByfunctionString(fun).isOHLC;
                createNewSeries(isOhlc, id, displayName);
                Thread newThread = new Thread(updatedGraph);
                object[] arry = new object[3];
                arry[0] = id;
                arry[1] = isOhlc;
                arry[2] = interval;
                newThread.Start(arry);
            }
            sr.Close();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            CommandManager.load();
            updateGraph = new Thread(updateChart);
            updateGraph.Start();
            tabControl1.TabPages.Clear();

            createFolder();
            string targetDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Charts", "Load");
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            bool doneone = false;
            foreach (string fileName in fileEntries)
            {
                if (doneone == true)
                {
                    Process.Start(Path.Combine(Application.StartupPath, "Charts.exe"));
                    break;
                }
                deserialize(fileName);
                doneone = true;
                File.Delete(fileName);
            }
               



        }

        private string dateTimeFormat = "yyyy-MM-dd HH:mm";

        private static Dictionary<string, Thread> idFunctionThread = new Dictionary<string, Thread>();
        private static Dictionary<string, Series> idSeries = new Dictionary<string, Series>();
        private static Dictionary<string, List<PointModel>> idUpdate = new Dictionary<string, List<PointModel>>();
        private static Dictionary<string, TabPage> idTabPage = new Dictionary<string, TabPage>();
        private static Dictionary<string, RichTextBox> idTextBox = new Dictionary<string, RichTextBox>();
        private static Thread updateGraph;
        private readonly object syncLock = new object();
        private static bool alive = true;
        private static bool isWarrningShow = false;
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
                Thread.Sleep(1000);
            }
            
        }



        private void btRemovGraph_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            removeSeries(clickedButton.Name);
        }



        private void createTabPage(string id, string displayName)
        {
            TabPage page = new TabPage();
            idTabPage.Add(id, page);
            page.Text = displayName;
            page.BackColor = this.BackColor;

            RichTextBox rb = new RichTextBox();
            rb.Multiline = true;
            rb.ScrollBars = RichTextBoxScrollBars.Vertical;
            rb.ReadOnly = true;
            rb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            rb.Dock = (System.Windows.Forms.DockStyle)(DockStyle.Top | DockStyle.Fill);

            page.Controls.Add(rb);

            idTextBox.Add(id, rb);

            Button button = new Button();
            button.BackColor = btOdabirGrafa.BackColor;
            button.UseVisualStyleBackColor = false;
            button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            button.Name = id;
            button.Text = "Delete graph";
            button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            button.Dock = DockStyle.Bottom;
            

            page.Controls.Add(button);
            button.Click += new System.EventHandler(this.btRemovGraph_Click);

           
            tabControl1.TabPages.Add(page);
            tabControl1.Visible = true;
            
            
        }

        private void createNewSeries(bool isOHLC, string id, string displayName)
        {

            if (isSeriesExist(id))
            {
                return;
            }
            Series series;
            createTabPage(id, displayName);


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

            series.Title = displayName;
            cartesianChart1.Series.Add(series);
            cartesianChart1.DisableAnimations = false;

            
            idSeries.Add(id, series);// idSEries.Count = 1 mesdz boks radite na svoju odgovornost 
        }
        private bool isSeriesExist(string id)
        {
            return idSeries.ContainsKey(id);
        }
        private void removeSeries(string id)
        {
            idUpdate.Remove(id);
            idFunctionThread.Remove(id);
            cartesianChart1.Series.Remove(idSeries[id]);
            idSeries.Remove(id);
            tabControl1.TabPages.Remove(idTabPage[id]);
            idTabPage.Remove(id);
            if (idTabPage.Count == 0)
            {
                tabControl1.Visible = false;
            }
            idTextBox.Remove(id);           
        }

        private void addPointsToChart(List<PointModel> points, string id)
        {
            
             if (isSeriesExist(id))
            {
                idSeries[id].Values.AddRange(points);
               
            }

        }

        private void addMessageToSeries(string id, string message)
        {

            if (InvokeRequired)
            {
                this.Invoke(new Action<string, string>(addMessageToSeries), new object[] { id, message });
                return;
            }
            RichTextBox tb = idTextBox[id];
            tb.Text += "\n[" + DateTime.Now.ToString("HH:mm:ss") + "]::  " + message;
            tb.SelectionStart = tb.Text.Length - 1;
            tb.SelectionLength = 0;
            tb.ScrollToCaret();

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
                Title = "Vreme",
                MinRange = 5
            });
            cartesianChart1.AxisY.Add(new Axis
            {
                LabelFormatter = val => Math.Round(val, 2).ToString() + "$"
               
            });
            //The next code simulates data changes every 500 
            cartesianChart1.Zoom = ZoomingOptions.X;

        }
        
        private void updatedGraph(object o)
        {
            Console.WriteLine("Updating graph thread started");
            CommandManager cm = new CommandManager();
            object[] arry = (object[])o;
            string command = ( string)(arry[0]);
            int interval = 0;
            string message = "";
            bool isOhlc = false;

            if (arry[1] is bool)
            {
                isOhlc = (bool)arry[1];
    
            }

            if (arry[2] is int)
            {
                interval = (int)arry[2];

            }


            string id = command;
           
            string lastRefresh = "";

            DateTime time;
            while (true)//sta se desava sa grafom poruku pirkazivati na tabu i poslenji refresh
            {
                Console.WriteLine("Thread sober");
                addMessageToSeries(id, "Loading graph...");
                string json = cm.excuteCommand(command,ref message);
                addMessageToSeries(id, message);
                List<PointModel> points = DataHandler.JSONtoPoint(json, ref lastRefresh, isOhlc);

                addPointsToUpdateQueue(points, id);
                Console.WriteLine("Points added:" + points.Count.ToString());
                Console.WriteLine("lastRefresh:" + lastRefresh);
                Console.WriteLine("Thread sleeping for");
                time = DateTime.Now;
                
                while (true)
                {
                    if (!isSeriesExist(id))
                        return;
                    TimeSpan span = DateTime.Now - time;
                    double totalMinutes = span.TotalMinutes;
                    time = DateTime.Now;
                    if (totalMinutes >= interval)
                    {
                        break;
                    }
                    Thread.Sleep(5000);
                }
               
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
            if(idSeries.Count == 1 && !isWarrningShow)
            {
                isWarrningShow = true;
                MessageBox.Show("Using more then one graph is in development, using it may effect program's performance", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
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
                arry[0] = id;
                arry[1] = isOhlc;
                arry[2] = interval;
                newThread.Start(arry);
 
            }
            d.Dispose();
        }


    

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btResetZoom_Click(object sender, EventArgs e)
        {
            cartesianChart1.AxisX[0].MinValue = double.NaN;
            cartesianChart1.AxisX[0].MaxValue = double.NaN;
            cartesianChart1.AxisY[0].MinValue = double.NaN;
            cartesianChart1.AxisY[0].MaxValue = double.NaN;
        }
      
        private void btNewInstance_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Application.StartupPath, "Charts.exe"));
        }

        private void btCloseAll_Click(object sender, EventArgs e)
        {
 
            Process[] processes = Process.GetProcessesByName("Charts");
            foreach(Process p in processes)
            {
                p.CloseMainWindow();
            }
        }




        private string serializeForm()
        {
            string rets = "";
            rets += Width + "," + Height + Environment.NewLine;
            rets += Location.X + "," + Location.Y + Environment.NewLine;
            foreach(string key in idTabPage.Keys)
            {
                rets += key + Environment.NewLine;
            }
            return rets;
        }


        private void Chart_FormClosed(object sender, FormClosedEventArgs e)
        {

            idFunctionThread.Clear();
            idSeries.Clear();
            alive = false;
            DialogResult result = MessageBox.Show("Do you want to save this form, so it reopens next time u run this app?", "Save", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Guid guid = Guid.NewGuid();
                string fileName = guid.ToString();
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Charts", "Load");
               while(File.Exists(Path.Combine(path, "Charts", fileName)))
                {
                     guid = Guid.NewGuid();
                     fileName = guid.ToString();
                     
                }
                path = Path.Combine(path, fileName);
                  
                System.IO.File.WriteAllText(path, serializeForm());
               
            }
        }

        private bool createFolder()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Charts\Load\";
            Console.WriteLine("The directory was created successfully at {0}.", path);
            try
            {
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                }

                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                Console.WriteLine("The directory was created successfully at {0}.", path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Nece da napravi folder :(");
                return false;
            }
            return true;

        }
    }

}