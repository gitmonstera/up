using System;
using System.Windows.Forms;
using System.Data.SQLite;

namespace up
{
    public partial class ResultsForm : Form
    {
        private const string DbFileName = "results.db";

        public ResultsForm()
        {
            InitializeComponent();
            LoadResults();
        }

        private void LoadResults()
        {
            dataGridView1.Rows.Clear();
            using (var conn = new SQLiteConnection($"Data Source={DbFileName}"))
            {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT Id, X0, Y0, R, K, Direction, BiggerSegmentArea, DateTime FROM Calculations ORDER BY Id DESC", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(
                        reader.GetInt32(0),
                        reader.GetDouble(1),
                        reader.GetDouble(2),
                        reader.GetDouble(3),
                        reader.GetDouble(4),
                        reader.GetString(5),
                        reader.GetDouble(6).ToString("F6"),
                        reader.GetString(7)
                    );
                }
            }
        }
    }
}
