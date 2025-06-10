using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace up
{
    public partial class MainForm : Form
    {
        private AboutForm aboutForm = null;

        public MainForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            pictureBox1.Paint += pictureBox1_Paint;
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
            res.Text = "Идет расчет...\n Пожалуйста, подождите.";

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

            // Расчет площади большого сегмента
            bool isHorizontal = radioButton1.Checked;
            double biggerSegmentArea = CalculateBiggerSegmentArea(R, x0, y0, K, isHorizontal);

            res.Text = $"Площадь большого\n\t\t сегмента:\n {biggerSegmentArea:F6}";

            button1.Enabled = true;
        }

        private double CalculateBiggerSegmentArea(double R, double x0, double y0, double K, bool isHorizontalLine)
        {
            double d;

            if (isHorizontalLine)
            {
                d = Math.Abs(y0 - K);
            }
            else
            {
                d = Math.Abs(x0 - K);
            }

            double circleArea = Math.PI * R * R;

            if (d >= R)
            {
                // Линия не пересекает круг
                return circleArea;
            }

            double h = R - d;

            double smallerSegmentArea = R * R * Math.Acos((R - h) / R) - (R - h) * Math.Sqrt(2 * R * h - h * h);

            double biggerSegmentArea = circleArea - smallerSegmentArea;

            return biggerSegmentArea;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Не используем дополнительную отрисовку
        }
    }
}
