using Guna.UI2.WinForms;
using System.Text.Json;
using Label = System.Windows.Forms.Label;

namespace FoodMachine
{
    public partial class Form1 : Form
    {
        string? path = "foods.txt";
        double enteredAmount = 0;
        bool radioButtonChecker = false;
        
        int value;

        List<Button>? buttons;
        List<RadioButton>? radioButtons;
        List<Label>? labels;
        List<Guna2CircleButton>? money;

        List<Food>? foodsList;
        //List<Food>? foods = new List<Food>()
        //    {
        //        new Food("Coca-Cola", 0.60, 99),
        //        new Food("Pepsi", 1, 80),
        //        new Food("Fanta", 0.8, 50),
        //        new Food("Sirab", 0.75, 100),
        //        new Food("Snickers", 1.65, 20),
        //        new Food("Albeni", 0.5, 32),
        //        new Food("Tutku", 0.3, 30),
        //        new Food("Mars", 0.4, 12),
        //        new Food("Bounty", 1.2, 9)
        //    };


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr = new(path!);
            foodsList = JsonSerializer.Deserialize<List<Food>>(sr.ReadToEnd()!);
            sr.Close();

            buttons = new List<Button>
            {
                btnCola,
                btnPepsi,
                btnFanta,
                btnSirab,
                btnSnickers,
                btnAlbeni,
                btnTutku,
                btnMars,
                btnBounty
            };

            labels = new()
            {
                priceCola,
                pricePepsi,
                priceFanta,
                priceSirab,
                priceSnickers,
                priceAlbeni,
                priceTutku,
                priceMars,
                priceBounty
            };

            radioButtons = new()
            {
                radioButton1,
                radioButton2,
                radioButton3,
                radioButton4,
                radioButton5,
                radioButton6,
                radioButton7,
                radioButton8,
                radioButton9
            };

            money = new()
            {
                btn10q,
                btn20q,
                btn50q,
                btn1m,
                btn5m,
                btn10m
            };


            byte count = 0;
            int p, q;

            foreach (Food i in foodsList!)
            {
                buttons![count].Text = i.Name;

                p = (int)((i.Price * 100) - (i.Price * 100 % 100));
                q = (int)(i.Price * 100 % 100);

                if (p > 0 && q != 0)
                    labels![count].Text = (p / 100).ToString() + " man " + q.ToString() + " q";
                else if (p > 0 && q == 0)
                    labels![count].Text = (p / 100).ToString() + " man ";
                else
                    labels![count].Text = q.ToString() + " q";


                radioButtons![count].Text = i.Count.ToString();
                count++;
            }
        }

        void btn_clicked(object sender, EventArgs e)
        {
            var btn = sender as Guna2CircleButton;

            if (btn != null)
            {
                if (btn?.Name == btn10q.Name)
                    enteredAmount += 0.1;
                else if (btn?.Name == btn20q.Name)
                    enteredAmount += 0.2;
                else if (btn?.Name == btn50q.Name)
                    enteredAmount += 0.5;
                else if (btn?.Name == btn1m.Name)
                    enteredAmount += 1;
                else if (btn?.Name == btn5m.Name)
                    enteredAmount += 5;
                else if (btn?.Name == btn10m.Name)
                    enteredAmount += 10;

                int p, q;
                p = (int)((enteredAmount * 100) - (enteredAmount * 100 % 100));
                q = (int)(enteredAmount * 100 % 100);

                if (p > 0 && q != 0)
                    textBox1.Text = (p / 100).ToString() + " man " + q.ToString() + " q";
                else if (p > 0 && q == 0)
                    textBox1.Text = (p / 100).ToString() + " man ";
                else
                    textBox1.Text = q.ToString() + " q";


                if (textBox1.Text != "Enter money")
                    btnCleaner.Enabled = true;


                string b = string.Empty;
                int val, item;

                if (radioButtonChecker)
                {
                    for (int i = 0; i < label1.Text.Length; i++)
                        if (Char.IsDigit(label1.Text[i]))
                            b += label1.Text[i];

                    item = Convert.ToInt32(label2.Text);
                    val = Convert.ToInt32(b);
                    if ((enteredAmount * 100) >= val && item > 0)
                    {
                        btnPay.Enabled = true;
                        value = val;
                    }
                    else
                        btnPay.Enabled = false;
                }
            }
        }

        private void btnCleaner_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Enter money";
            btnCleaner.Enabled = false;
            btnPay.Enabled = false;
            enteredAmount = 0;
        }


        private void Guna2RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            byte count = 0;

            foreach (RadioButton item in radioButtons!)
            {
                if (item.Checked)
                {
                    button1.BackColor = buttons![count].BackColor;
                    button1.Text = buttons[count].Text;
                    button1.Font = buttons[count].Font;
                    label1.Text = "Price is " + labels![count].Text;
                    label2.Text = radioButtons![count].Text;
                    radioButtonChecker = true;
                    break;
                }
                item.Checked = false;
                count++;
            }

            if (radioButtonChecker)
                foreach (var i in money!)
                    i.Enabled = true;
        }

        private async void pay_Click(object sender, EventArgs e)
        {
            if (btnPay.Enabled)
            {
                enteredAmount *= 100;
                enteredAmount -= value;
                enteredAmount /= 100;
                int p, q, count = 0;
                byte countItems = 0;

                p = (int)((enteredAmount * 100) - (enteredAmount * 100 % 100));
                q = (int)(enteredAmount * 100 % 100);


                foreach (var i in radioButtons!)
                {
                    if (i.Checked)
                    {
                        countItems = byte.Parse(i.Text);
                        i.Text = (--countItems).ToString();
                        if (foodsList is not null)
                            foodsList[count].Count = countItems;
                        break;
                    }
                    count++;
                }

                using (StreamWriter sw = new StreamWriter(path!))
                {
                    sw.WriteLine(JsonSerializer.Serialize(foodsList, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }));
                    sw.Close();
                }

                if (p > 0 && q != 0)
                    label3.Text = "Residue is " + (p / 100).ToString() + " man " + q.ToString() + " q";
                else if (p > 0 && q == 0)
                    label3.Text = "Residue is " + (p / 100).ToString() + " man ";
                else
                    label3.Text = "Residue is " + q.ToString() + " q";

                btnPay.Enabled = false;

                await Task.Delay(2500);

                MessageBox.Show("Operation successfully completed...)", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                label3.Text = string.Empty;
                label1.Text = string.Empty;

                button1.Text = string.Empty;
                button1.BackColor = Color.Transparent;

                textBox1.Text = "Enter money";
                btnCleaner.Enabled = false;

                value = 0;
                radioButtonChecker = false;
                radioButtons![count].Checked = radioButtonChecker;
                enteredAmount = 0;
                foreach (var i in money!)
                    i.Enabled = false;
            }
        }
    }
}