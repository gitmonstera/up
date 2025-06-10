using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

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

            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection($"Data Source={DbFileName}");
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Calculations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                X0 REAL,
                Y0 REAL,
                R REAL,
                K REAL,
                Direction TEXT,
                BiggerSegmentArea REAL,
                DateTime TEXT
            );
            ";
            tableCmd.ExecuteNonQuery();
        }

        private void SaveCalculation(double x0, double y0, double R, double K, string direction, double area)
        {
            using var connection = new SqliteConnection($"Data Source={DbFileName}");
            connection.Open();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
            @"
            INSERT INTO Calculations (X0, Y0, R, K, Direction, BiggerSegmentArea, DateTime)
            VALUES ($x0, $y0, $r, $k, $direction, $area, $datetime);
            ";
            insertCmd.Parameters.AddWithValue("$x0", x0);
            insertCmd.Parameters.AddWithValue("$y0", y0);
            insertCmd.Parameters.AddWithValue("$r", R);
            insertCmd.Parameters.AddWithValue("$k", K);
            insertCmd.Parameters.AddWithValue("$direction", direction);
            insertCmd.Parameters.AddWithValue("$area", area);
            insertCmd.Parameters.AddWithValue("$datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            insertCmd.ExecuteNonQuery();
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

        private async void buttonShowResults_Click(object sender, EventArgs e)
        {
            var resultsForm = new ResultsForm();
            resultsForm.Show(this);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(xBox.Text, out double x0) ||
                !double.TryParse(yBox.Text, out double y0) ||
                !double.TryParse(RBox.Text, out double R) ||
                !double.TryParse(KBox.Text, out double K))
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения.");
                return;
            }

            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Выберите направление: Горизонталь или Вертикаль.");
                return;
            }

            button1.Enabled = false;
            res.Text = "Идет расчет... Пожалуйста, подождите.";

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            await Task.Run(() =>
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);

                    float scale = 50f;
                    float offsetX = pictureBox1.Width / 2f;
                    float offsetY = pictureBox1.Height / 2f;

                    // Оси
                    Pen axisPen = new Pen(Color.Black, 1);
                    g.DrawLine(axisPen, 0, offsetY, pictureBox1.Width, offsetY);
                    g.DrawLine(axisPen, offsetX, 0, offsetX, pictureBox1.Height);

                    // Круг
                    Pen circlePen = new Pen(Color.Blue, 2);
                    float circleX = offsetX + (float)x0 * scale - (float)R * scale;
                    float circleY = offsetY - (float)y0 * scale - (float)R * scale;
                    float diameter = (float)(2 * R * scale);
                    g.DrawEllipse(circlePen, circleX, circleY, diameter, diameter);

                    // Линия
                    Pen linePen = new Pen(Color.Red, 2);
                    if (radioButton1.Checked)
                    {
                        float yLine = offsetY - (float)K * scale;
                        g.DrawLine(linePen, 0, yLine, pictureBox1.Width, yLine);
                    }
                    else
                    {
                        float xLine = offsetX + (float)K * scale;
                        g.DrawLine(linePen, xLine, 0, xLine, pictureBox1.Height);
                    }
                }
            });

            pictureBox1.Image = bmp;

            bool isHorizontal = radioButton1.Checked;
            double biggerSegmentArea = CalculateBiggerSegmentArea(R, x0, y0, K, isHorizontal);

            res.Text = $"Площадь большого сегмента: {biggerSegmentArea:F6}";

            // Сохраняем в базу
            string direction = isHorizontal ? "Горизонталь" : "Вертикаль";
            SaveCalculation(x0, y0, R, K, direction, biggerSegmentArea);

            button1.Enabled = true;
        }

        private double CalculateBiggerSegmentArea(double R, double x0, double y0, double K, bool isHorizontalLine)
        {
            double d = isHorizontalLine ? Math.Abs(y0 - K) : Math.Abs(x0 - K);
            double circleArea = Math.PI * R * R;

            if (d >= R)
            {
                return circleArea;
            }

            double h = R - d;

            double smallerSegmentArea = R * R * Math.Acos((R - h) / R) - (R - h) * Math.Sqrt(2 * R * h - h * h);

            return circleArea - smallerSegmentArea;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Не используем дополнительную отрисовку
        }
    }
}
