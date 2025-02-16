using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game_of_life2
{
    public partial class Form1 : Form
    {
        private NumericUpDown inWidth;
        private NumericUpDown inHeight;
        private NumericUpDown numOfCells;
        private NumericUpDown liftimeTicks;

        private int x;
        private int y;
        private int num;
        private int ticks;

        public Form1()
        {
            InitializeComponent();
            Text = "Menü";
            inWidth = new NumericUpDown
            {
                Width = 60,
                Top = 20,
                Left = 150,
                Minimum = 20,
                Maximum = 100,
                Value = 20,
                Increment = 10,
                Parent = this
            };

            inHeight = new NumericUpDown
            {
                Width = 60,
                Top = 50,
                Left = 150,
                Minimum = 20,
                Maximum = 100,
                Increment = 10,
                Parent = this
            };
            inHeight.Value = 20;

            numOfCells = new NumericUpDown
            {
                Width = 60,
                Top = 80,
                Left = 150,
                Minimum = 5,
                Maximum = Math.Round((inHeight.Maximum + inWidth.Maximum) / 2, 0),
                Parent = this
            };
            numOfCells.Value = 10;

            liftimeTicks = new NumericUpDown
            {
                Width = 60,
                Top = 110,
                Left = 150,
                Minimum = 0,
                Maximum = 10,
                Parent = this
            };
            liftimeTicks.Value = 2;

            MakeMenu();
        }

        private void MakeMenu()
        {
            this.Size = new Size(230, 350);
            TextBox widthTb = new TextBox
            {
                Text = "Az élettér szélessége:",
                Width = 120,
                Top = 20,
                Left = 20,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Parent = this
            };



            TextBox heightTb = new TextBox
            {
                Text = "Az élettér magassága:",
                Width = 120,
                Top = 50,
                Left = 20,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Parent = this
            };

            TextBox CellNumTb = new TextBox
            {
                Text = "Elhelyezendő sejtek:",
                Width = 120,
                Top = 80,
                Left = 20,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Parent = this
            };

            TextBox TicksTb = new TextBox
            {
                Text = "Sejtek túlélő képessége:",
                Width = 120,
                Top = 110,
                Left = 20,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Parent = this
            };

            Button send = new Button
            {
                Width = 50,
                Height = 30,
                Top = 280,
                Left = 80,
                Text = "Indítás",
                Parent = this
            };

            send.Click += Start;
        }

        private void Start(object sender, EventArgs e)
        {
            this.Visible = false;
            x = Convert.ToInt16(inWidth.Value);
            y = Convert.ToInt16(inHeight.Value);
            num = Convert.ToInt16(numOfCells.Value);
            ticks = Convert.ToInt16(liftimeTicks.Value);

            //function validaton?

            FormGame formGame = new FormGame(x, y, num, ticks);
            formGame.Visible = false;
            if (formGame.ShowDialog() == DialogResult.OK)
            {
                this.Visible = true;
                formGame.Close();
            }
        }
    }
}
