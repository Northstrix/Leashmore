using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Leashmore
{
    public class AddHourlyPaidGigForm : Form
    {
        private const int formWidth = 540;
        private const int formHeight = 428;
        private const int fixedColumnWidth = 160;
        private const int emptyColumnWidth = 40;
        private const int increasedHeight = 12;
        private const int buttonHeight = 40;
        private const int reducedButtonHeight = 36;

        public AddHourlyPaidGigForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Size = new Size(formWidth, formHeight);
            this.Text = "Add Hourly Paid Gig";
            this.BackColor = ColorTranslator.FromHtml("#161616");

            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 8, // Increased RowCount for the "Hours" field
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Absolute, fixedColumnWidth),
                    new ColumnStyle(SizeType.Percent, 100),
                    new ColumnStyle(SizeType.Absolute, emptyColumnWidth)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.AutoSize, increasedHeight),
                    new RowStyle(SizeType.AutoSize, increasedHeight),
                    new RowStyle(SizeType.AutoSize, increasedHeight),
                    new RowStyle(SizeType.AutoSize, increasedHeight),
                    new RowStyle(SizeType.AutoSize, increasedHeight),
                    new RowStyle(SizeType.AutoSize, increasedHeight),
                    new RowStyle(SizeType.AutoSize, increasedHeight),
                    new RowStyle(SizeType.AutoSize, reducedButtonHeight),
                }
            };

            this.Controls.Add(mainTable);

            mainTable.Controls.Add(CreateLabel("Title:", 0), 0, 0);
            mainTable.Controls.Add(CreateTextBox(0), 1, 0);

            mainTable.Controls.Add(CreateLabel("Start Date:", 1), 0, 1);
            mainTable.Controls.Add(CreateTextBox(1), 1, 1);

            mainTable.Controls.Add(CreateLabel("End Date:", 0), 0, 2);
            mainTable.Controls.Add(CreateTextBox(0), 1, 2);

            mainTable.Controls.Add(CreateLabel("Hours:", 1), 0, 3); // Added Hours Label
            mainTable.Controls.Add(CreateTextBox(1), 1, 3);         // Added TextBox for Hours

            mainTable.Controls.Add(CreateLabel($"Hourly Pay: {Form1.currency}", 0), 0, 4);
            mainTable.Controls.Add(CreateTextBox(0), 1, 4);

            mainTable.Controls.Add(CreateLabel("Description:", 1), 0, 5);
            mainTable.Controls.Add(CreateMultilineTextBox(1), 1, 5);

            TableLayoutPanel buttonTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Width = 200,
                ColumnCount = 2,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Percent, 50),
                    new ColumnStyle(SizeType.Percent, 50)
                }
            };

            Button addRecordButton = CreateButton("Add", "#28C9EF", ColorTranslator.FromHtml("#202020"), AnchorStyles.Right);
            Button cancelButton = CreateButton("Cancel", "#202020", ColorTranslator.FromHtml("#eeeeee"), AnchorStyles.Left);

            buttonTable.Controls.Add(addRecordButton, 0, 0);
            buttonTable.Controls.Add(cancelButton, 1, 0);

            mainTable.Controls.Add(buttonTable, 0, 7); // Adjusted Row Index for the buttonTable
            mainTable.SetColumnSpan(buttonTable, 3);

            addRecordButton.Click += (sender, e) =>
            {
                string titleText = GetTextBoxText(mainTable, 0);
                string startDateText = GetTextBoxText(mainTable, 1);
                string endDateText = GetTextBoxText(mainTable, 2);
                string hoursText = GetTextBoxText(mainTable, 3);
                string hourlyPayText = GetTextBoxText(mainTable, 4);
                string descriptionText = GetTextBoxText(mainTable, 5);

                if (!double.TryParse(hoursText, out double hoursValue) || !double.TryParse(hourlyPayText, out double hourlyPayValue))
                {
                    Form1.ShowErrorMessageBox("Invalid Input", "Hours and Hourly Pay must be valid numeric values.");
                }
                else
                {
                    if (hoursValue > 0 && hourlyPayValue > 0)
                    {
                        AddHourlyPaidGigRecord(titleText, startDateText, endDateText, hoursText.Replace(",", "."), hourlyPayText.Replace(",", "."), descriptionText);
                        this.Close();
                    }
                    else
                    {
                        Form1.ShowErrorMessageBox("Invalid Input", "Hours and Hourly Pay must be greater than zero.");
                    }
                }
            };

            cancelButton.Click += (sender, e) =>
            {
                Form1.ShowLeashmoreMessageBox("Operation Was Cancelled By User");
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
                Width = fixedColumnWidth,
                Height = reducedButtonHeight
            };

            return label;
        }

        private System.Windows.Forms.TextBox CreateTextBox(int row)
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                BackColor = row % 2 == 0 ? ColorTranslator.FromHtml("#202020") : ColorTranslator.FromHtml("#eeeeee"),
                ForeColor = row % 2 == 0 ? ColorTranslator.FromHtml("#eeeeee") : ColorTranslator.FromHtml("#202020"),
                Dock = DockStyle.Fill,
                Height = increasedHeight,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            return textBox;
        }

        private System.Windows.Forms.TextBox CreateMultilineTextBox(int row)
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                BackColor = row % 2 == 0 ? ColorTranslator.FromHtml("#202020") : ColorTranslator.FromHtml("#eeeeee"),
                ForeColor = row % 2 == 0 ? ColorTranslator.FromHtml("#eeeeee") : ColorTranslator.FromHtml("#202020"),
                Dock = DockStyle.Fill,
                Height = 110,  // Set the height to 300 pixels
                Font = new Font("Arial", 12, FontStyle.Bold),
                Multiline = true,  // Set the textbox to multiline
                ScrollBars = ScrollBars.Vertical  // Enable vertical scrollbar
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
                Height = buttonHeight,
                Margin = new Padding(8, 0, 8, 0),
                Anchor = anchor
            };

            return button;
        }

        private string GetTextBoxText(TableLayoutPanel mainTable, int rowIndex)
            {
                return ((TextBox)mainTable.GetControlFromPosition(1, rowIndex)).Text;
            }

            private void AddHourlyPaidGigRecord(string title, string startDate, string endDate, string hours, string hourlyPay, string description)
            {
                string recId = GenerateRandomString(16);
                while (Form1.CheckIfRecordExists(recId, "HourlyPaidGig"))
                    recId = GenerateRandomString(16);

                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(Form1.SQLiteconnectionString))
                    {
                        connection.Open();

                        string commandText = @"INSERT INTO HourlyPaidGig 
                                           (Rec_id, Title, Start_date, End_date, Hours, Hourly_pay, Description) 
                                           VALUES 
                                           (@RecId, @Title, @StartDate, @EndDate, @Hours, @HourlyPay, @Description)";

                        using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                        {
                            command.Parameters.AddWithValue("@RecId", recId);
                            command.Parameters.AddWithValue("@Title", Form1.Encrypt_string_with_aes_in_cbc(title));
                            command.Parameters.AddWithValue("@StartDate", Form1.Encrypt_string_with_aes_in_cbc(startDate));
                            command.Parameters.AddWithValue("@EndDate", Form1.Encrypt_string_with_aes_in_cbc(endDate));
                            command.Parameters.AddWithValue("@Hours", Form1.Encrypt_string_with_aes_in_cbc(hours));
                            command.Parameters.AddWithValue("@HourlyPay", Form1.Encrypt_string_with_aes_in_cbc(hourlyPay));
                            command.Parameters.AddWithValue("@Description", Form1.Encrypt_string_with_aes_in_cbc(description));

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Form1.ShowLeashmoreMessageBox("Record Added Successfully!");
                            }
                            else
                            {
                                Form1.ShowErrorMessageBox("Failed to Add Record", "");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Form1.ShowErrorMessageBox("Something went wrong with the database", $"Error: {ex.Message}");
                }
            }

            private static string GenerateRandomString(int length)
            {
                const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] randomBytes = new byte[length];
                    rng.GetBytes(randomBytes);

                    char[] chars = new char[length];
                    for (int i = 0; i < length; i++)
                    {
                        int index = randomBytes[i] % allowedChars.Length;
                        chars[i] = allowedChars[index];
                    }

                    return new string(chars);
                }
            }
        }
    }