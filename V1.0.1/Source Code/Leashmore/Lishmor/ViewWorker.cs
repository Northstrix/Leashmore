using System;
using System.Windows.Forms;
using System.Drawing; // Add this for Color

namespace Leashmore
{
    public partial class ViewWorker : Form
    {
        private TableLayoutPanel tableLayoutPanel;
        private Label[] labelControls;
        private Button okButton;

        public ViewWorker(params string[] labels)
        {
            InitializeForm(labels);
            this.Text = "View Full Worker Record";
        }

        private void InitializeForm(string[] labels)
        {
            // Create the table layout panel
            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;

            // Set the background color
            tableLayoutPanel.BackColor = ColorTranslator.FromHtml("#212121");
            tableLayoutPanel.Padding = new Padding(16, 16, 16, 16);

            // Set the height of each table row to 30 pixels
            for (int i = 0; i < labels.Length; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            }

            // Create label controls based on the input string array
            labelControls = new Label[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                labelControls[i] = new Label();
                labelControls[i].Text = labels[i];
                labelControls[i].Dock = DockStyle.Fill;

                // Set the foreground (text) color
                labelControls[i].ForeColor = ColorTranslator.FromHtml("#EEEEEE");

                // Set the text size to 14
                labelControls[i].Font = new Font(labelControls[i].Font.FontFamily, 14);

                // Set label 20 pixels from the left
                tableLayoutPanel.SetCellPosition(labelControls[i], new TableLayoutPanelCellPosition(0, i));

                tableLayoutPanel.Controls.Add(labelControls[i], 0, i);
            }

            // Add the "OK" button to the form
            okButton = new Button
            {
                Text = "OK",
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.ColorTranslator.FromHtml("#24DE9C"),
                ForeColor = System.Drawing.ColorTranslator.FromHtml("#202020"),
                Height = 40,
                Dock = DockStyle.Bottom,
                Width = 69,
                Margin = new Padding(0, 10, 0, 0),
                DialogResult = DialogResult.OK
            };

            okButton.Click += OkButton_Click;

            // Add controls to the form
            tableLayoutPanel.Controls.Add(okButton, 0, labels.Length);
            Controls.Add(tableLayoutPanel);

            // Set the initial form size
            this.Size = new System.Drawing.Size(720, 600);

            // Handle the form resize event
            this.Resize += ViewWorkerForm_Resize;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            // Add any code you want to execute when the "OK" button is clicked
            this.DialogResult = DialogResult.OK;
            this.Close(); // Close the form in this example
        }

        private void ViewWorkerForm_Resize(object sender, EventArgs e)
        {
            // Adjust the size of the table layout panel when the form is resized
            tableLayoutPanel.Size = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height);
        }
    }
}
