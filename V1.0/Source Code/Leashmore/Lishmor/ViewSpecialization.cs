using System;
using System.Drawing;
using System.Windows.Forms;

namespace Leashmore
{
    public class ViewSpecialization : Form
    {
        private const int formWidth = 400;
        private const int formHeight = 308;
        private const int labelHeight = 30;
        private const int textBoxHeight = 30;
        private const int multilineTextBoxHeight = 100;
        private const int buttonHeight = 40;

        private const int margin = 20; // Margin from left and right
        private const int topBottomMargin = 10; // Margin from top and bottom

        public ViewSpecialization(string[] data)
        {
            InitializeForm(data);
        }

        private void InitializeForm(string[] data)
        {
            this.Size = new Size(formWidth + 2 * margin, formHeight + 2 * topBottomMargin);
            this.Text = "View Specialization";
            this.BackColor = ColorTranslator.FromHtml("#041020");

            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Padding = new Padding(margin, topBottomMargin, margin, topBottomMargin)
            };

            this.Controls.Add(mainTable);

            mainTable.Controls.Add(CreateLabel("Title"));
            mainTable.Controls.Add(CreateTextBox(data.Length > 0 ? data[0] : ""));

            mainTable.Controls.Add(CreateLabel("Description"));
            mainTable.Controls.Add(CreateMultilineTextBox(data.Length > 1 ? data[1] : ""));

            TableLayoutPanel buttonTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };

            Button okButton = CreateButton("OK", "#00BD00", "#202020");

            okButton.Click += (sender, e) => this.Close();

            buttonTable.Controls.Add(okButton, 0, 0);

            mainTable.Controls.Add(buttonTable);
            mainTable.SetRow(buttonTable, 6);
            mainTable.SetRowSpan(buttonTable, 2);

            // Center the OK button
            buttonTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttonTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, okButton.Width));
            buttonTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttonTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            buttonTable.Controls.SetChildIndex(okButton, 0);

            this.Resize += (sender, e) =>
            {
                buttonTable.Location = new Point((mainTable.Width - buttonTable.Width) / 2, mainTable.Height - buttonTable.Height - 20);
            };
        }

        private System.Windows.Forms.Label CreateLabel(string labelText)
        {
            System.Windows.Forms.Label label = new System.Windows.Forms.Label()
            {
                Text = labelText,
                BackColor = ColorTranslator.FromHtml("#041020"),
                ForeColor = ColorTranslator.FromHtml("#b4E5EE"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Height = labelHeight,
            };

            return label;
        }

        private System.Windows.Forms.TextBox CreateTextBox(string text)
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                BackColor = ColorTranslator.FromHtml("#041020"),
                ForeColor = ColorTranslator.FromHtml("#b4E5EE"),
                Dock = DockStyle.Fill,
                Height = textBoxHeight,
                Font = new Font("Arial", 12, FontStyle.Regular),
                Text = text
            };

            return textBox;
        }

        private System.Windows.Forms.TextBox CreateMultilineTextBox(string text)
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                BackColor = ColorTranslator.FromHtml("#041020"),
                ForeColor = ColorTranslator.FromHtml("#b4E5EE"),
                Dock = DockStyle.Fill,
                Multiline = true,
                Height = multilineTextBoxHeight,
                Font = new Font("Arial", 12, FontStyle.Regular),
                Text = text
            };

            return textBox;
        }

        private Button CreateButton(string buttonText, string backColor, string foreColor)
        {
            Button button = new Button()
            {
                Text = buttonText,
                BackColor = ColorTranslator.FromHtml(backColor),
                ForeColor = ColorTranslator.FromHtml(foreColor),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Height = buttonHeight,
            };

            return button;
        }
    }
}
