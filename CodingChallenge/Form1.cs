using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodingChallenge
{
    public partial class Form1 : Form
    {
        NumericUpDown numericUpDown1;
        DateTimePicker dateTimePicker1;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Button testbutton = new Button();
            testbutton.Text = "Change Thermostat Value";
            testbutton.Location = new Point(70, 70);
            testbutton.Size = new Size(100, 100);
            testbutton.Visible = true;
            testbutton.BringToFront();
            testbutton.Click += ChangeThermostatTemp;
            Controls.Add(testbutton);

            Button testbutton2 = new Button();
            testbutton2.Text = "Schedule Thermostat Change";
            testbutton2.Location = new Point(280, 70);
            testbutton2.Size = new Size(100, 100);
            testbutton2.Visible = true;
            testbutton2.BringToFront();
            testbutton2.Click += ScheduleChangeThermostatTemp;
            Controls.Add(testbutton2);

          
            numericUpDown1 = new NumericUpDown();
            numericUpDown1.Dock = System.Windows.Forms.DockStyle.Top;
            numericUpDown1.Value = 0;
            numericUpDown1.Maximum = 100;
            numericUpDown1.Minimum = 0;
            Controls.Add(numericUpDown1);


            
            dateTimePicker1 = new DateTimePicker();
            dateTimePicker1.MinDate = DateTime.Now;
            dateTimePicker1.MaxDate = new DateTime(2021, 6, 20);
            dateTimePicker1.CustomFormat = "MMM dd, yyyy - dddd - H mm";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Size = new Size(300,300);
            dateTimePicker1.Location = new Point(460,100);
            dateTimePicker1.ShowUpDown = true;
            Controls.Add(dateTimePicker1);

        }

        private void ChangeThermostatTemp(object sender, EventArgs e)
        {
            Action method;
            Controller.units[0].SetValue((float)numericUpDown1.Value);
         
        }
        private void ScheduleChangeThermostatTemp(object sender, EventArgs e)
        {
            Action method;
            method = () => Controller.units[0].SetValue((float)numericUpDown1.Value);
            Controller.units[0].ScheduleEvents(new ScheduledEvent(dateTimePicker1.Value, method));

        }
    }
}
