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
        int counterMinute = 1;
        public FunctionAddDialog()
        {
            InitializeComponent();
            CommandManager.load();
            listBoxSym.SelectionMode = SelectionMode.One;
            
            foreach (string fun in CommandManager.getAvailableFunctions())
                this.comboBoxFun.Items.Add(fun);
            for (int i = 1; i < 14; i++)
            {
                comboBoxInterval.Items.Add((counterMinute).ToString() + " min");
                counterMinute = i * 5;
                
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
            labelName.Text = par.displayName;

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
                listBoxMarket.DataSource = null;

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
            lb.Update();


        }

        private void FunctionAddDialog_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void comboBoxFun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)comboBoxFun.SelectedItem != null)
            {
                updateFunction((string)comboBoxFun.SelectedItem);
                listBoxMarket.BeginUpdate();
                var names = market.Where(item => item.ToLower().Contains(tbMarket.Text.ToLower())).ToList();
                listBoxMarket.DataSource = names;
                listBoxMarket.EndUpdate();
            }
        }

        private void tbSym_TextChanged(object sender, EventArgs e)
        {
            listBoxSym.BeginUpdate();
            var names = symbols.Where(item => item.ToLower().Contains(tbSym.Text.ToLower())).ToList();
            listBoxSym.DataSource = names;
            listBoxSym.EndUpdate();
            labelSym1.ForeColor = isSymChoosen() ? Color.Goldenrod : Color.Red; // NADJI BOJU --DONE

        }

        private void tbMarket_TextChanged(object sender, EventArgs e)//
        {
            listBoxMarket.BeginUpdate();
            var names = market.Where(item => item.ToLower().Contains(tbMarket.Text.ToLower())).ToList();
            listBoxMarket.DataSource = names;
            listBoxMarket.EndUpdate();
            labelMarket.ForeColor = isMarketChoosen() ? Color.Goldenrod : Color.Red; // Dodato da boji u crveno i kad MARKET nije dobar


        }

        private void btDone_Click(object sender, EventArgs e)  //PROBLEM!!! Postoje valute sa preko 4 simbola
        {
            if (!validateForms())
            {
                this.DialogResult = DialogResult.None;
                MessageBox.Show("Symbol and market field can only contain alphanumerical values, 1-8 characters long. Please choose the correct one.", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
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
            return tbSym.Text + " "+ comboBoxFun.SelectedItem.ToString();
        }

        public int getInterval()
        {
            return Convert.ToInt32(comboBoxInterval.SelectedItem.ToString().Replace("min", ""));
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

        Regex regex = new Regex("^[A-Z0-9]{1,4}$");
        Regex regexMarket = new Regex("^[A-Z]{1,4}$");
        private bool validateForms()
        {
            bool allGood = true;
            List<String> chosen = new List<string>();
            if (!isSymChoosen())
            {
                allGood = false;
                chosen.Add( "symbol");
                
            }
            if(!isMarketChoosen())
            {
                allGood = false;
                chosen.Add("market");
            }

            if (!allGood)
            {
                string msg = "You have to chose: ";
                
                foreach (string text in chosen)
                {
                    msg += text + " ";
                }

              
            } 
            return isSymChoosen() && isMarketChoosen();



        }

        private bool isMarketChoosen()
        {
            bool jbm = regexMarket.IsMatch(tbMarket.Text); 

            return !tbMarket.Enabled || regexMarket.IsMatch(tbMarket.Text);
        }

        private bool isSymChoosen()
        {
            
            return regex.IsMatch(tbSym.Text);

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
                        this.tbSym.Text = this.listBoxSym.SelectedItem.ToString().Substring(0, j);//skinuti ovo substringovanje, nek pise sve
                        found = true;
                    }
                    else
                    {
                        int i = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).IndexOf(',');
                        this.tbSym.Text = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).Substring(0, i);
                    }
                }
                /*
                foreach (string name in this.listBoxSym.Items)
                {
                    if (this.tbSym.Text.Equals(name.Substring(0, this.tbSym.Text.Length)))
                    {
                        found = true;
                        int i = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).IndexOf(',');
                        this.tbSym.Text = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).Substring(0, i);
                        break;
                    }
                }          */      
            }
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

                        int j = this.listBoxSym.SelectedItem.ToString().IndexOf(',');
                        this.tbSym.Text = this.listBoxSym.SelectedItem.ToString().Substring(0, j);
                        found = true;
                    }
                }
                else if(this.tbSym.Text.Length < 5 && this.tbSym.Text.Length > 0)
                {
                    if((this.listBoxSym.GetItemText(listBoxSym.SelectedItem)).Length <= 0)
                    {
                        this.tbSym.Text = this.tbSym.Text;
                    }
                    else
                    {
                        int i = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).IndexOf(',');
                        this.tbSym.Text = this.listBoxSym.GetItemText(listBoxSym.SelectedItem).Substring(0, i);
                    }
                    
                }
                else
                {
                    labelSym1.ForeColor = Color.Red;
                    MessageBox.Show("You can only enter 4 characters. Please choose the correct one.", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    labelSym1.ForeColor = Color.Goldenrod;
                    found = false;
                    return;
                }


            }
        }        

        private void tbSym_KeyDown(object sender, KeyEventArgs e)
        {
            // TO:DO uraditi i za dugme na gore
            // TO:DO uraditi sve ovo i za tbMarket
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
            this.tbMarket.SelectionStart = tbMarket.Text.Length;
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
                else if (this.tbMarket.Text.Length < 5)
                {
                    foreach (string name in this.listBoxMarket.Items)
                    {
                        if (name.ToLower().Contains(this.tbMarket.Text.ToLower()))
                        {
                            found = true;
                            int i = this.listBoxMarket.GetItemText(listBoxMarket.SelectedItem).IndexOf(',');
                            this.tbMarket.Text = this.listBoxMarket.GetItemText(listBoxMarket.SelectedItem).Substring(0, i);
                            break;
                        }
                    }
                }
                if (!found)
                {
                    labelMarket.ForeColor = Color.Red;
                    MessageBox.Show("You can choose only offered values from the list. Please choose the correct one.", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    labelMarket.ForeColor = Color.Goldenrod;
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
                        int j = this.listBoxMarket.ToString().IndexOf(',');
                        this.tbMarket.Text = this.listBoxMarket.ToString().Substring(0, j);
                        found = true;
                    }                  
                }
                else if(this.tbMarket.Text.Length < 5)
                {
                    foreach (string name in this.listBoxMarket.Items)
                    {
                        if (name.ToLower().Contains(this.tbMarket.Text.ToLower()))
                        {
                            found = true;
                            int i = this.listBoxMarket.GetItemText(listBoxMarket.SelectedItem).IndexOf(',');
                            this.tbMarket.Text = this.listBoxMarket.GetItemText(listBoxMarket.SelectedItem).Substring(0, i);
                            break;
                        }
                        
                    }                                          
                }
                if(!found)
                {
                    labelMarket.ForeColor = Color.Red;
                    MessageBox.Show("You can choose only offered values from the list. Please choose the correct one.", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    labelMarket.ForeColor = Color.Goldenrod;
                    found = false;
                    return;
                }
            }
        }

        private void labelInterval_Click(object sender, EventArgs e)
        {

        }
    }
}



