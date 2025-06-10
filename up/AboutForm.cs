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
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            string aboutText =
            @"О программе

Наименование программы: Monte Carlo App
Версия: 1.0
Разработчик: Антон Карташов
Дата выпуска: 10 июня 2025 г.

Назначение программы:
Программа предназначена для численного решения математических задач методом Монте-Карло с высокой точностью. Позволяет рассчитывать значения функции по заданным параметрам и визуализировать результаты в горизонтальном или вертикальном направлении.

Функциональные возможности:
- Ввод начальных параметров: координаты X0, Y0, коэффициенты R, K.
- Выбор направления расчёта: горизонтальное или вертикальное.
- Визуализация результатов в виде графика.
- Сохранение и загрузка результатов.
- Форма с информацией «О программе».

Требования к системе:
- ОС: Microsoft Windows 7 и выше
- Платформа: .NET Framework 4.7.2 и выше
- Минимальные системные требования: 2 ГБ ОЗУ, 500 МБ свободного места на диске

Автор и контактные данные:
Антон Карташов
E-mail: anton.kartashov@example.com
Телефон: +7 (123) 456-78-90

Правила использования и лицензия:
Программа распространяется бесплатно для некоммерческого использования. Копирование и распространение без согласия автора запрещены.";


            TextBox textBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Text = aboutText,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(textBox);


            this.ClientSize = new Size(470, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }
    }
}
