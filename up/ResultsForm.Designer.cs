namespace up
{
    partial class ResultsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridView1;

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Columns.Add("Id", "ID");
            this.dataGridView1.Columns.Add("X0", "X0");
            this.dataGridView1.Columns.Add("Y0", "Y0");
            this.dataGridView1.Columns.Add("R", "R");
            this.dataGridView1.Columns.Add("K", "K");
            this.dataGridView1.Columns.Add("Direction", "Direction");
            this.dataGridView1.Columns.Add("Area", "Area");
            this.dataGridView1.Columns.Add("DateTime", "DateTime");
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;

            this.Controls.Add(this.dataGridView1);
            this.Text = "История расчетов";
            this.ClientSize = new System.Drawing.Size(800, 400);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
