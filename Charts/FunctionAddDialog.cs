using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Charts.CommandManager;

namespace Charts
{
    public partial class FunctionAddDialog : Form
    {
        Function f;
        List<string> symbols = new List<string>();
        List<string> market = new List<string>();
        int y = 0;
        int x = 0;
        public FunctionAddDialog()
        {
            InitializeComponent();
            CommandManager.load();
            foreach (string fun in CommandManager.getAvailableFunctions())
                this.comboBoxFun.Items.Add(fun);
            for (int i = 0; i < 12; i++)
            {
                comboBoxInterval.Items.Add((i * 5).ToString() + "sec");
            }
            comboBoxInterval.SelectedIndex = 1;
            comboBoxFun.SelectedIndex = 0;
            tbMarket.CharacterCasing = CharacterCasing.Upper;
            tbSym.CharacterCasing = CharacterCasing.Upper;

        }

        private void updateFunction(string selectedFun)
        {
            f = CommandManager.GetFunction(selectedFun);
            x = 3;
            y = 20;
            groupBoxReq.Controls.Clear();
            foreach (string reqPara in f.RequiredPara)
            {
                initParameterComponent(reqPara, groupBoxReq);
            }
            x = 3;
            y = 20;
            groupBoxOptional.Controls.Clear();
            foreach (string reqPara in f.OptionalPara)
            {
                initParameterComponent(reqPara, groupBoxOptional);
            }
        }

        private void initParameterComponent(string reqPara, GroupBox groupBox)
        {

            if (reqPara.ToLower().Contains("symbol") || reqPara.ToLower().Contains("market"))
            {
                addSymbolTable(reqPara);
                return;
            }

            Parameter par = CommandManager.getParameter(reqPara);


            Label labelName = new Label();
            labelName.Height = 17;
            labelName.Text = par.name;

            groupBox.Controls.Add(labelName);

            labelName.Location = new Point(x, y);

            y += labelName.Height;

            ComboBox cb = new ComboBox();
            cb.Name = par.name;
            ToolTip tp = new ToolTip();

            tp.SetToolTip(cb, par.tooltip); // ovo zameniti sa parametar tootlip

            foreach (string parameter in par.options)
            {
                cb.Items.Add(parameter);

            }
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.SelectedIndex = 0;
            cb.Size = comboBoxFun.Size;
            cb.Width = cb.Width;
            groupBox.Controls.Add(cb);

            cb.Location = new Point(x, y);

            y += cb.Height + 3;


        }

        private void addSymbolTable(string reqPara)
        {
            List<Tuple<string, string>> values;
            ListBox lb = listBoxSym;
            TextBox tb = tbSym;
            List<string> names = symbols;

            if (reqPara.ToLower().Contains("crypto"))
            {
                values = CommandManager.getCryptos();

            }
            else if (reqPara.ToLower().Contains("market"))
            {
                values = CommandManager.getCurrency();
                lb = listBoxMarket;
                tb = tbMarket;
                names = market;
                tbMarket.Enabled = true;
                listBoxMarket.Enabled = true;
            }
            else
            {
                tbMarket.Enabled = false;
                listBoxMarket.Enabled = false;
                values = CommandManager.getEquity();
            }
            names.Clear();
            foreach (Tuple<string, string> pair in values)
            {

                names.Add(pair.Item1 + ", " + pair.Item2);

            }

            lb.BeginUpdate();
            lb.DataSource = names;
            lb.EndUpdate();



        }

        private void FunctionAddDialog_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void comboBoxFun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)comboBoxFun.SelectedItem != null)
                updateFunction((string)comboBoxFun.SelectedItem);
        }

        private void tbSym_TextChanged(object sender, EventArgs e)
        {
            listBoxSym.BeginUpdate();
            var names = symbols.Where(item => item.ToLower().Contains(tbSym.Text.ToLower())).ToList();
            listBoxSym.DataSource = names;
            listBoxSym.EndUpdate();

        }

        private void tbMarket_TextChanged(object sender, EventArgs e)
        {
            listBoxMarket.BeginUpdate();
            var names = market.Where(item => item.ToLower().Contains(tbMarket.Text.ToLower())).ToList();
            listBoxMarket.DataSource = names;
            listBoxMarket.EndUpdate();
        }

        private void btDone_Click(object sender, EventArgs e)
        {
            if (!validateForms())
            {
                this.DialogResult = DialogResult.None;
                return;
            }

        }
        public bool isSeriesOhlc()
        {

            return f.isOHLC;
        }


        public string getSeriesDisplayName()
        {
            //OVO TREBA DODATI


            return tbSym.Text;
        }

        public int getInterval()
        {
            return Convert.ToInt32(comboBoxInterval.SelectedItem.ToString().Replace("sec", ""));
        }

        public Dictionary<string, string> generateCommand()
        {
            Dictionary<string, string> command = new Dictionary<string, string>();
            Function fun = CommandManager.GetFunction(comboBoxFun.SelectedItem.ToString());
            command.Add("function", fun.function);
            command.Add("symbol", tbSym.Text);
            if (tbMarket.Enabled)
            {
                command.Add("market", tbMarket.Text);
            }

            foreach (var item in groupBoxReq.Controls)
            {
                ComboBox cb = item as ComboBox;
                if (cb != null)
                    command.Add(cb.Name, cb.SelectedItem.ToString());
            }
            foreach (var item in groupBoxOptional.Controls)
            {
                ComboBox cb = item as ComboBox;
                if (cb != null)
                    command.Add(cb.Name, cb.SelectedItem.ToString());
            }

            return command;
        }

        Regex regex = new Regex(@"[A-Z0-9]{1,4}");
        private bool validateForms()
        {
            if (!isSymChoosen())
            {
                return false;
            }
            if (tbMarket.Enabled)
            {
                if (!isMarketChoosen())
                {
                    return false;
                }
            }

            return regex.Match(tbSym.Text).Success && (!tbMarket.Enabled || regex.Match(tbMarket.Text).Success);
        }

        private bool isMarketChoosen()
        {

            if (!regex.Match(tbMarket.Text).Success)
            {

                labelMarket.ForeColor = Color.Red;
                MessageBox.Show("You have to choose one item from list.");
                labelMarket.ForeColor = Color.Black;
                return false;
            }
            return true;
        }

        private bool isSymChoosen()
        {
            if (!regex.Match(tbSym.Text).Success)
            {
                labelSym1.ForeColor = Color.Red;
                MessageBox.Show("You have to choose one item from list.");
                labelSym1.ForeColor = Color.Black;
                return false;
            }

            return true;

        }


        // keys for listBoxSym and tb_sym

        private void listBoxSym_KeyUp(object sender, KeyEventArgs e)
        {
            this.tbSym.SelectionStart = tbSym.Text.Length;
        }
        private void listBoxSym_KeyDown(object sender, KeyEventArgs e)
        {
            this.tbSym.SelectionStart = tbSym.Text.Length;
        }

        private void listBoxSym_DoubleClick(object sender, EventArgs e)
        {
            int i = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).IndexOf(',');
            this.tbSym.Text = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).Substring(0, i);
        }

        private void listBoxSym_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int i = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).IndexOf(',');
            this.tbSym.Text = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).Substring(0, i);
        }

        private void tbSym_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Return)
            {
                bool found = false;
                Regex regex1 = new Regex("[ \t]");
                if (this.tbSym.Text.Equals(regex1) || this.tbSym.Text.Equals(""))
                {
                    if (this.listBoxSym.SelectedItem != null)
                    {
                        this.tbSym.Text = this.listBoxSym.SelectedItem.ToString();
                        found = true;
                    }
                }
                foreach (string name in this.listBoxSym.Items)
                {
                    if (this.tbSym.Text.Equals(name.Substring(0, name.IndexOf(','))))
                    {
                        found = true;
                        int i = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).IndexOf(',');
                        this.tbSym.Text = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).Substring(0, i);
                        break;
                    }
                }
                if (!found)
                {
                    labelSym1.ForeColor = Color.Red;
                    MessageBox.Show("Only offered values can be choosen.");
                    labelSym1.ForeColor = Color.Black;
                    return;
                }




            }
        }
        private void listBoxSym_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                bool found = false;
                Regex regex1 = new Regex("[ \t]");
                if (this.tbSym.Text.Equals(regex1) || this.tbSym.Text.Equals(""))
                {
                    if (this.listBoxSym.SelectedItem != null)
                    {
                        int j = this.listBoxSym.SelectedItem.ToString().IndexOf(',');
                        this.tbSym.Text = this.listBoxSym.SelectedItem.ToString().Substring(0, j);
                        found = true;
                    }
                }
                foreach (string name in this.listBoxSym.Items)
                {
                    if (this.tbSym.Text.Equals(name.Substring(0, name.IndexOf(','))))
                    {
                        found = true;
                        int i = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).IndexOf(',');
                        this.tbSym.Text = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).Substring(0, i);
                        break;
                    }
                }
                if (!found)
                {
                    labelSym1.ForeColor = Color.Red;
                    MessageBox.Show("Only offered values can be choosen.");
                    labelSym1.ForeColor = Color.Black;
                    return;
                }



            }
        }

        private void tbSym_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (this.listBoxSym.SelectedIndex >= this.listBoxSym.Items.Count - 1)
                {
                    this.listBoxSym.SelectedIndex = this.listBoxSym.Items.Count - 1;
                }
                else
                {
                    this.listBoxSym.SelectedIndex++;
                }

            }
            this.tbSym.SelectionStart = tbSym.Text.Length;
        }

        private void tbSym_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.tbSym.SelectionStart = tbSym.Text.Length;
                if (this.listBoxSym.SelectedIndex <= 0)
                {
                    this.listBoxSym.SelectedIndex = 0;
                }
                else
                {
                    this.listBoxSym.SelectedIndex--;
                }
            }
           

        }




        // keys for listBoxMarket and tbMarket
        private void tbMarket_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                this.tbMarket.SelectionStart = tbMarket.Text.Length;


                if (this.listBoxMarket.SelectedIndex >= this.listBoxMarket.Items.Count - 1)
                {
                    this.listBoxMarket.SelectedIndex = this.listBoxMarket.Items.Count - 1;
                }
                else
                {
                    this.listBoxMarket.SelectedIndex++;
                }
            }
        }

        private void tbMarket_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                this.tbMarket.SelectionStart = tbMarket.Text.Length;
                {
                    if (this.listBoxMarket.SelectedIndex <= 0)
                    {
                        this.listBoxMarket.SelectedIndex = 0;
                    }
                    else
                    {
                        this.listBoxMarket.SelectedIndex--;
                    }
                }
            }

        }

        

        private void listBoxMarket_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void listBoxMarket_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                bool found = false;
                Regex regex1 = new Regex("[ \t]");
                if (this.tbMarket.Text.Equals(regex1) || this.tbMarket.Text.Equals(""))
                {
                    if (this.listBoxMarket.SelectedItem != null)
                    {
                        int j = this.listBoxMarket.SelectedItem.ToString().IndexOf(',');
                        this.tbMarket.Text = this.listBoxMarket.SelectedItem.ToString().Substring(0, j);
                        found = true;
                    }
                }
                foreach (string name in this.listBoxMarket.Items)
                {
                    if (this.tbMarket.Text.Equals(name.Substring(0, name.IndexOf(','))))
                    {
                        found = true;
                        int i = this.listBoxMarket.GetItemText(listBoxMarket.SelectedItem).IndexOf(',');
                        this.tbMarket.Text = this.listBoxMarket.GetItemText(listBoxMarket.SelectedItem).Substring(0, i);
                        break;
                    }
                }
                if (!found)
                {
                    labelMarket.ForeColor = Color.Red;
                    MessageBox.Show("Only offered values can be choosen.");
                    labelMarket.ForeColor = Color.Black;
                    return;
                }
                
            }
        }

        private void listBoxMarket_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int i = this.listBoxMarket.GetItemText(listBoxMarket.SelectedItem).IndexOf(',');
            this.tbMarket.Text = this.listBoxMarket.GetItemText(listBoxMarket.SelectedItem).Substring(0, i);
        }

        private void tbMarket_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Return)
            {
                bool found = false;
                Regex regex1 = new Regex("[ \t]");
                if (this.tbMarket.Text.Equals(regex1) || this.tbMarket.Text.Equals(""))
                {
                    if (this.listBoxMarket.SelectedItem != null)
                    {
                        this.tbMarket.Text = this.listBoxMarket.SelectedItem.ToString();
                        found = true;
                    }
                }
                foreach (string name in this.listBoxMarket.Items)
                {
                    if (this.tbMarket.Text.Equals(name.Substring(0, name.IndexOf(','))))
                    {
                        found = true;
                        int i = this.listBoxMarket.GetItemText(listBoxMarket.SelectedItem).IndexOf(',');
                        this.tbMarket.Text = this.listBoxMarket.GetItemText(listBoxMarket.SelectedItem).Substring(0, i);
                        break;
                    }
                }
                if (!found)
                {
                    labelMarket.ForeColor = Color.Red;
                    MessageBox.Show("Only offered values can be choosen.");
                    labelMarket.ForeColor = Color.Black;
                    return;
                }
            }
        }

       
    }
 }
