using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Data.SQLite;
using Org.BouncyCastle.Utilities.Encoders;

namespace Leashmore
{
    public class ViewResultPaidGigForm : Form
    {
        private const int FormWidth = 540;
        private const int FormHeight = 392;
        private const int FixedColumnWidth = 160;
        private const int EmptyColumnWidth = 40;
        private const int IncreasedHeight = 12;
        private const int ButtonHeight = 40;
        private const int ReducedButtonHeight = 36;

        private TableLayoutPanel mainTable;

        public ViewResultPaidGigForm(string title, string startDate, string endDate, string payment, string description)
        {
            InitializeForm(title, startDate, endDate, payment, description);
        }

        private void InitializeForm(string title, string startDate, string endDate, string payment, string description)
        {
            this.Size = new Size(FormWidth, FormHeight);
            this.Text = "View Result Paid Gig";
            this.BackColor = ColorTranslator.FromHtml("#161616");

            mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 7,
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
                    new RowStyle(SizeType.AutoSize, ReducedButtonHeight),
                    new RowStyle(SizeType.AutoSize, ButtonHeight)
                }
            };

            this.Controls.Add(mainTable);

            mainTable.Controls.Add(CreateLabel("Title:", 0), 0, 0);
            mainTable.Controls.Add(CreateTextBox(title, 0), 1, 0);

            mainTable.Controls.Add(CreateLabel("Start Date:", 1), 0, 1);
            mainTable.Controls.Add(CreateTextBox(startDate, 1), 1, 1);

            mainTable.Controls.Add(CreateLabel("End Date:", 0), 0, 2);
            mainTable.Controls.Add(CreateTextBox(endDate, 0), 1, 2);

            mainTable.Controls.Add(CreateLabel($"Payment for Gig: {Form1.currency}", 1), 0, 3);
            mainTable.Controls.Add(CreateTextBox(payment, 1), 1, 3);

            mainTable.Controls.Add(CreateLabel("Description:", 0), 0, 4);
            mainTable.Controls.Add(CreateMultilineTextBox(description, 0), 1, 4);

            TableLayoutPanel buttonTable = CreateButtonTable();

            Button okButton = CreateButton("OK", ColorTranslator.FromHtml("#202020"), ColorTranslator.FromHtml("#eeeeee"), false);

            buttonTable.Controls.Add(okButton, 0, 0);

            mainTable.Controls.Add(buttonTable, 0, 6);
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

        private System.Windows.Forms.TextBox CreateTextBox(string text, int row)
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

        private System.Windows.Forms.TextBox CreateMultilineTextBox(string text, int row)
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

        private TableLayoutPanel CreateButtonTable()
        {
            TableLayoutPanel buttonTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Width = 200,
                ColumnCount = 1,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 100)
                }
            };

            return buttonTable;
        }

        private Button CreateButton(string text, Color backColor, Color foreColor, bool isAddButton)
        {
            Button button = new Button()
            {
                Text = text,
                BackColor = backColor,
                ForeColor = foreColor,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Height = isAddButton ? ButtonHeight : ReducedButtonHeight,
                Margin = isAddButton ? new Padding(0, 0, 8, 0) : new Padding(8, 0, 0, 0),
                Anchor = isAddButton ? AnchorStyles.Right : AnchorStyles.Left
            };

            return button;
        }
    }
}
