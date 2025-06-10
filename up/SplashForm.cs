using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace up
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.IndianRed;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Width = 400;
            this.Height = 200;
            Label lbl = new Label
            {
                Text = "Monte Carlo App",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lbl);
        }
    }
}
