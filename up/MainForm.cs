using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace up
{
    public partial class MainForm : Form
    {
        private AboutForm aboutForm = null;
        private const string DbFileName = "results.db";

        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            pictureBox1.Paint += pictureBox1_Paint;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(DbFileName))
                SQLiteConnection.CreateFile(DbFileName);

            using (var conn = new SQLiteConnection($"Data Source={DbFileName}"))
            {
                conn.Open();
                var cmd = new SQLiteCommand(@"
                    CREATE TABLE IF NOT EXISTS Calculations (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        X0 REAL,
                        Y0 REAL,
                        R REAL,
                        K REAL,
                        Direction TEXT,
                        BiggerSegmentArea REAL,
                        DateTime TEXT
                    )", conn);
                cmd.ExecuteNonQuery();
            }
        }

        private void SaveCalculation(double x0, double y0, double R, double K, string direction, double area)
        {
            using (var connection = new SQLiteConnection($"Data Source={DbFileName}"))
            {
                connection.Open();
                var insertCmd = new SQLiteCommand(@"
                    INSERT INTO Calculations (X0, Y0, R, K, Direction, BiggerSegmentArea, DateTime)
                    VALUES (@x0, @y0, @r, @k, @direction, @area, @datetime);
                ", connection);

                insertCmd.Parameters.AddWithValue("@x0", x0);
                insertCmd.Parameters.AddWithValue("@y0", y0);
                insertCmd.Parameters.AddWithValue("@r", R);
                insertCmd.Parameters.AddWithValue("@k", K);
                insertCmd.Parameters.AddWithValue("@direction", direction);
                insertCmd.Parameters.AddWithValue("@area", area);
                insertCmd.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                insertCmd.ExecuteNonQuery();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(xBox.Text, out double x0) ||
                !double.TryParse(yBox.Text, out double y0) ||
                !double.TryParse(RBox.Text, out double R) ||
                !double.TryParse(KBox.Text, out double K))
            {
                MessageBox.Show("Введите корректные числовые значения.");
                return;
            }

            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Выберите направление.");
                return;
            }

            button1.Enabled = false;
            res.Text = "Расчет...";

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            await Task.Run(() =>
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    float scale = 50f;
                    float offsetX = pictureBox1.Width / 2f;
                    float offsetY = pictureBox1.Height / 2f;

                    g.DrawLine(Pens.Black, 0, offsetY, pictureBox1.Width, offsetY);
                    g.DrawLine(Pens.Black, offsetX, 0, offsetX, pictureBox1.Height);

                    float circleX = offsetX + (float)x0 * scale - (float)R * scale;
                    float circleY = offsetY - (float)y0 * scale - (float)R * scale;
                    float diameter = (float)(2 * R * scale);
                    g.DrawEllipse(Pens.Blue, circleX, circleY, diameter, diameter);

                    if (radioButton1.Checked)
                    {
                        float yLine = offsetY - (float)K * scale;
                        g.DrawLine(Pens.Red, 0, yLine, pictureBox1.Width, yLine);
                    }
                    else
                    {
                        float xLine = offsetX + (float)K * scale;
                        g.DrawLine(Pens.Red, xLine, 0, xLine, pictureBox1.Height);
                    }
                }
            });

            pictureBox1.Image = bmp;

            bool isHorizontal = radioButton1.Checked;
            double area = CalculateBiggerSegmentArea(R, x0, y0, K, isHorizontal);
            res.Text = $"Площадь: {area:F6}";

            string direction = isHorizontal ? "Горизонталь" : "Вертикаль";
            SaveCalculation(x0, y0, R, K, direction, area);

            button1.Enabled = true;
        }

        private double CalculateBiggerSegmentArea(double R, double x0, double y0, double K, bool isHorizontalLine)
        {
            double d = isHorizontalLine ? Math.Abs(y0 - K) : Math.Abs(x0 - K);
            double circleArea = Math.PI * R * R;

            if (d >= R)
                return circleArea;

            double h = R - d;
            double smallSegment = R * R * Math.Acos((R - h) / R) - (R - h) * Math.Sqrt(2 * R * h - h * h);
            return circleArea - smallSegment;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) { }

        private void button3_Click(object sender, EventArgs e)
        {
            ResultsForm resultsForm = new ResultsForm();
            resultsForm.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (aboutForm == null || aboutForm.IsDisposed)
            {
                aboutForm = new AboutForm();
                aboutForm.FormClosed += (s, args) => aboutForm = null;
                aboutForm.Show(this);
            }
            else
            {
                aboutForm.BringToFront();
            }
        }
    }
}