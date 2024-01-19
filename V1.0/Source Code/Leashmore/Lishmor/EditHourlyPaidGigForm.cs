using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Leashmore
{
    public class EditHourlyPaidGigForm : Form
    {
        private const int FormWidth = 540;
        private const int FormHeight = 448;
        private const int FixedColumnWidth = 160;
        private const int EmptyColumnWidth = 40;
        private const int IncreasedHeight = 12;
        private const int ButtonHeight = 40;
        private const int ReducedButtonHeight = 36;

        private string selectedId;

        public EditHourlyPaidGigForm(string selectedId, string title, string startDate, string endDate, string hours, string hourlyPay, string description)
        {
            this.selectedId = selectedId;
            InitializeForm(title, startDate, endDate, hours, hourlyPay, description);
        }

        private void InitializeForm(string title, string startDate, string endDate, string hours, string hourlyPay, string description)
        {
            this.Size = new Size(FormWidth, FormHeight);
            this.Text = "Edit Hourly Paid Gig";
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

                if (!double.TryParse(GetTextBoxText(mainTable, 3), out double hoursValue) || !double.TryParse(GetTextBoxText(mainTable, 4), out double hourlyPayValue))
                {
                    Form1.ShowErrorMessageBox("Invalid Input", "Hours and Hourly Pay must be valid numeric values.");
                }
                else
                {
                    if (hoursValue > 0 && hourlyPayValue > 0)
                    {
                        UpdateHourlyPaidGigRecord(selectedId, GetTextBoxText(mainTable, 0), GetTextBoxText(mainTable, 1), GetTextBoxText(mainTable, 2), GetTextBoxText(mainTable, 3).Replace(",", "."), GetTextBoxText(mainTable, 4).Replace(",", "."), GetTextBoxText(mainTable, 5));
                        this.Close();
                    }
                    else
                    {
                        Form1.ShowErrorMessageBox("Invalid Input", "Hours and Hourly Pay must be greater than zero.");
                    }
                }

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
                Font = new Font("Arial", 12, FontStyle.Bold)
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
                ScrollBars = ScrollBars.Vertical
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

        private string GetTextBoxText(TableLayoutPanel mainTable, int rowIndex)
        {
            return ((TextBox)mainTable.GetControlFromPosition(1, rowIndex)).Text;
        }

        private void UpdateHourlyPaidGigRecord(string selectedId, string title, string startDate, string endDate, string hours, string hourlyPay, string description)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Form1.SQLiteconnectionString))
                {
                    connection.Open();

                    string commandText = @"UPDATE HourlyPaidGig 
                                           SET Title = @Title, 
                                               Start_date = @StartDate, 
                                               End_date = @EndDate, 
                                               Hours = @Hours, 
                                               Hourly_pay = @HourlyPay, 
                                               Description = @Description 
                                           WHERE Rec_id = @RecId";

                    using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@RecId", selectedId);
                        command.Parameters.AddWithValue("@Title", Form1.Encrypt_string_with_aes_in_cbc(title));
                        command.Parameters.AddWithValue("@StartDate", Form1.Encrypt_string_with_aes_in_cbc(startDate));
                        command.Parameters.AddWithValue("@EndDate", Form1.Encrypt_string_with_aes_in_cbc(endDate));
                        command.Parameters.AddWithValue("@Hours", Form1.Encrypt_string_with_aes_in_cbc(hours));
                        command.Parameters.AddWithValue("@HourlyPay", Form1.Encrypt_string_with_aes_in_cbc(hourlyPay));
                        command.Parameters.AddWithValue("@Description", Form1.Encrypt_string_with_aes_in_cbc(description));

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Form1.ShowLeashmoreMessageBox("Record Updated Successfully!");
                        }
                        else
                        {
                            Form1.ShowErrorMessageBox("Failed to Update Record", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Form1.ShowErrorMessageBox("Something went wrong with the database", $"Error: {ex.Message}");
            }
        }
    }
}
