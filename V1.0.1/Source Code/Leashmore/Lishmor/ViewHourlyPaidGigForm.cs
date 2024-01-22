using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Leashmore
{
    public class ViewHourlyPaidGigForm : Form
    {
        private const int FormWidth = 540;
        private const int FormHeight = 448;
        private const int FixedColumnWidth = 160;
        private const int EmptyColumnWidth = 40;
        private const int IncreasedHeight = 12;
        private const int ButtonHeight = 40;
        private const int ReducedButtonHeight = 36;

        public ViewHourlyPaidGigForm(string title, string startDate, string endDate, string hours, string hourlyPay, string description)
        {
            InitializeForm(title, startDate, endDate, hours, hourlyPay, description);
        }

        private void InitializeForm(string title, string startDate, string endDate, string hours, string hourlyPay, string description)
        {
            this.Size = new Size(FormWidth, FormHeight);
            this.Text = "View Hourly Paid Gig";
            this.BackColor = ColorTranslator.FromHtml("#161616");

            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 8,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Absolute, FixedColumnWidth),
                    new ColumnStyle(SizeType.Percent, 100),
                    new ColumnStyle(SizeType.Absolute, EmptyColumnWidth)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.AutoSize, IncreasedHeight),
                    new RowStyle(SizeType.AutoSize, IncreasedHeight),
                    new RowStyle(SizeType.AutoSize, IncreasedHeight),
                    new RowStyle(SizeType.AutoSize, IncreasedHeight),
                    new RowStyle(SizeType.AutoSize, IncreasedHeight),
                    new RowStyle(SizeType.AutoSize, IncreasedHeight),
                    new RowStyle(SizeType.AutoSize, IncreasedHeight),
                    new RowStyle(SizeType.AutoSize, ReducedButtonHeight),
                }
            };

            this.Controls.Add(mainTable);

            mainTable.Controls.Add(CreateLabel("Title:", 0), 0, 0);
            mainTable.Controls.Add(CreateTextBox(0, title), 1, 0);

            mainTable.Controls.Add(CreateLabel("Start Date:", 1), 0, 1);
            mainTable.Controls.Add(CreateTextBox(1, startDate), 1, 1);

            mainTable.Controls.Add(CreateLabel("End Date:", 0), 0, 2);
            mainTable.Controls.Add(CreateTextBox(0, endDate), 1, 2);

            mainTable.Controls.Add(CreateLabel("Hours:", 1), 0, 3);
            mainTable.Controls.Add(CreateTextBox(1, hours), 1, 3);

            mainTable.Controls.Add(CreateLabel($"Hourly Pay: {Form1.currency}", 0), 0, 4);
            mainTable.Controls.Add(CreateTextBox(0, hourlyPay), 1, 4);

            mainTable.Controls.Add(CreateLabel("Description:", 1), 0, 5);
            mainTable.Controls.Add(CreateMultilineTextBox(1, description), 1, 5);

            TableLayoutPanel buttonTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Width = 200,
                ColumnCount = 1,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 100),
                }
            };

            Button okButton = CreateButton("OK", "#28C9EF", ColorTranslator.FromHtml("#202020"), AnchorStyles.Bottom);

            buttonTable.Controls.Add(okButton, 0, 0);

            mainTable.Controls.Add(buttonTable, 0, 7);
            mainTable.SetColumnSpan(buttonTable, 3);

            okButton.Click += (sender, e) =>
            {
                this.Close();
            };

            this.Resize += (sender, e) =>
            {
                buttonTable.Location = new Point((mainTable.Width - buttonTable.Width) / 2, mainTable.Height - buttonTable.Height - 20);
            };
        }

        private System.Windows.Forms.Label CreateLabel(string labelText, int row)
        {
            System.Windows.Forms.Label label = new System.Windows.Forms.Label()
            {
                Text = labelText,
                BackColor = ColorTranslator.FromHtml("#161616"),
                ForeColor = row % 2 == 0 ? ColorTranslator.FromHtml("#eeeeee") : ColorTranslator.FromHtml("#28C9EF"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Width = FixedColumnWidth,
                Height = ReducedButtonHeight
            };

            return label;
        }

        private System.Windows.Forms.TextBox CreateTextBox(int row, string text)
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                Text = text,
                BackColor = row % 2 == 0 ? ColorTranslator.FromHtml("#202020") : ColorTranslator.FromHtml("#eeeeee"),
                ForeColor = row % 2 == 0 ? ColorTranslator.FromHtml("#eeeeee") : ColorTranslator.FromHtml("#202020"),
                Dock = DockStyle.Fill,
                Height = IncreasedHeight,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ReadOnly = true
            };

            return textBox;
        }

        private System.Windows.Forms.TextBox CreateMultilineTextBox(int row, string text)
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                Text = text,
                BackColor = row % 2 == 0 ? ColorTranslator.FromHtml("#202020") : ColorTranslator.FromHtml("#eeeeee"),
                ForeColor = row % 2 == 0 ? ColorTranslator.FromHtml("#eeeeee") : ColorTranslator.FromHtml("#202020"),
                Dock = DockStyle.Fill,
                Height = 110,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            return textBox;
        }

        private Button CreateButton(string text, string backColor, Color foreColor, AnchorStyles anchor)
        {
            Button button = new Button()
            {
                Text = text,
                BackColor = ColorTranslator.FromHtml(backColor),
                ForeColor = foreColor,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Height = ButtonHeight,
                Margin = new Padding(8, 0, 8, 0),
                Anchor = anchor
            };

            return button;
        }
    }
}
