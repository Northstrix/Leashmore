using System;
using System.Windows.Forms;

namespace Leashmore
{
    public partial class ViewAllPayments : Form
    {
        private string[,] paymentData;

        public ViewAllPayments(string[,] data)
        {
            InitializeComponent(data);
            this.Text = "View All Payments";
        }

        private void InitializeComponent(string[,] data)
        {
            this.SuspendLayout();

            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 600); // Set initial form size
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#242424");

            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Padding = new System.Windows.Forms.Padding(10),
                AutoSize = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None // Set cell border style to None
            };

            // Set the row styles for the TableLayoutPanel
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // First row with 100% height
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Second row with 40px height

            this.Controls.Add(mainTable);

            paymentData = data;

            // Create and add DataGridView
            DataGridView dataGridView = new DataGridView
            {
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#161616"), // Updated background color
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE"),
                Dock = System.Windows.Forms.DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                ReadOnly = true,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                CellBorderStyle = DataGridViewCellBorderStyle.None, // Set cell border style to None
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = System.Drawing.ColorTranslator.FromHtml("#161616"), // Set cell background color
                    ForeColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE"), // Set cell foreground color
                    Font = new System.Drawing.Font("Arial", 12) // Set text size for the table to 12
                },
                RowHeadersVisible = false // Hide row headers
            };

            // Customize the header style
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.ColorTranslator.FromHtml("#24DE9C"), // Color scheme of the button
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#202020")
            };
            dataGridView.ColumnHeadersDefaultCellStyle = headerStyle;

            // Add columns to DataGridView
            dataGridView.Columns.Add("PaymentAmountColumn", "Payment Amount");
            dataGridView.Columns.Add("RecipientColumn", "Recipient");

            // Set column widths
            dataGridView.Columns["PaymentAmountColumn"].Width = (int)(dataGridView.Width * 0.24);
            dataGridView.Columns["RecipientColumn"].Width = (int)(dataGridView.Width * 0.76);

            mainTable.Controls.Add(dataGridView, 0, 0);

            // Add data to DataGridView
            for (int i = 0; i < data.GetLength(0); i++)
            {
                dataGridView.Rows.Add(data[i, 0], data[i, 1]);
            }

            // Create and add OK button
            Button okButton = new Button
            {
                Text = "OK",
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.ColorTranslator.FromHtml("#24DE9C"),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#202020"),
                Height = 40,
                Dock = System.Windows.Forms.DockStyle.Bottom,
                Margin = new System.Windows.Forms.Padding(0, 10, 0, 0),
                DialogResult = DialogResult.OK
            };

            okButton.FlatAppearance.BorderSize = 0; // Remove button border

            mainTable.Controls.Add(okButton, 0, 1);

            // Enable form resizing
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;

            this.ResumeLayout(false);
        }
    }
}
