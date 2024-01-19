using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Data.SQLite;
using Org.BouncyCastle.Utilities.Encoders;

namespace Leashmore
{
    public class EditResultPaidGigForm : Form
    {
        private const int formWidth = 540;
        private const int formHeight = 392;
        private const int fixedColumnWidth = 160;
        private const int emptyColumnWidth = 40;
        private const int increasedHeight = 12;
        private const int buttonHeight = 40;
        private const int reducedButtonHeight = 36;
        private string selectedId;

        public EditResultPaidGigForm(string selectedId, string title, string startDate, string endDate, string payment, string description)
        {
            this.selectedId = selectedId;
            InitializeForm(title, startDate, endDate, payment, description);
        }

        private void InitializeForm(string title, string startDate, string endDate, string payment, string description)
        {
            this.Size = new Size(formWidth, formHeight);
            this.Text = "Edit Result Paid Gig";
            this.BackColor = ColorTranslator.FromHtml("#161616");

            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 7,
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
                    new RowStyle(SizeType.AutoSize, reducedButtonHeight),
                    new RowStyle(SizeType.AutoSize, buttonHeight)
                }
            };

            this.Controls.Add(mainTable);

            mainTable.Controls.Add(CreateLabel("Title:", 0), 0, 0);
            mainTable.Controls.Add(CreateTextBox(0, title), 1, 0);

            mainTable.Controls.Add(CreateLabel("Start Date:", 1), 0, 1);
            mainTable.Controls.Add(CreateTextBox(1, startDate), 1, 1);

            mainTable.Controls.Add(CreateLabel("End Date:", 0), 0, 2);
            mainTable.Controls.Add(CreateTextBox(0, endDate), 1, 2);

            mainTable.Controls.Add(CreateLabel($"Payment for Gig: {Form1.currency}", 1), 0, 3);
            mainTable.Controls.Add(CreateTextBox(1, payment), 1, 3);

            mainTable.Controls.Add(CreateLabel("Description:", 0), 0, 4);
            mainTable.Controls.Add(CreateMultilineTextBox(0, description), 1, 4);

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

            Button editRecordButton = new Button()
            {
                Text = "Edit",
                BackColor = ColorTranslator.FromHtml("#28C9EF"),
                ForeColor = ColorTranslator.FromHtml("#202020"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Height = buttonHeight,
                Margin = new Padding(0, 0, 8, 0),
                Anchor = AnchorStyles.Right
            };

            Button cancelButton = new Button()
            {
                Text = "Cancel",
                BackColor = ColorTranslator.FromHtml("#202020"),
                ForeColor = ColorTranslator.FromHtml("#eeeeee"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Height = buttonHeight,
                Margin = new Padding(8, 0, 0, 0),
                Anchor = AnchorStyles.Left
            };

            buttonTable.Controls.Add(editRecordButton, 0, 0);
            buttonTable.Controls.Add(cancelButton, 1, 0);

            mainTable.Controls.Add(buttonTable, 0, 6);
            mainTable.SetColumnSpan(buttonTable, 3);

            // Set the values to the text entries
            ((TextBox)mainTable.GetControlFromPosition(1, 0)).Text = title;
            ((TextBox)mainTable.GetControlFromPosition(1, 1)).Text = startDate;
            ((TextBox)mainTable.GetControlFromPosition(1, 2)).Text = endDate;
            ((TextBox)mainTable.GetControlFromPosition(1, 3)).Text = payment;
            ((TextBox)mainTable.GetControlFromPosition(1, 4)).Text = description;

            editRecordButton.Click += (sender, e) =>
            {
                string editedTitle = ((TextBox)mainTable.GetControlFromPosition(1, 0)).Text;
                string editedStartDate = ((TextBox)mainTable.GetControlFromPosition(1, 1)).Text;
                string editedEndDate = ((TextBox)mainTable.GetControlFromPosition(1, 2)).Text;
                string editedPayment = ((TextBox)mainTable.GetControlFromPosition(1, 3)).Text;
                string editedDescription = ((TextBox)mainTable.GetControlFromPosition(1, 4)).Text;

                if (!double.TryParse(editedPayment, out double pay))
                {
                    Form1.ShowErrorMessageBox("Invalid Input", "Payment amount must be a valid numeric value.");
                }
                else
                {
                    if (pay > 0)
                    {
                        EditResultPaidGigRecord(selectedId, editedTitle, editedStartDate, editedEndDate, editedPayment.Replace(",", "."), editedDescription);
                        this.Close();
                    }
                    else
                    {
                        Form1.ShowErrorMessageBox("Invalid Input", "Payment amount must be greater than zero.");
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

        private System.Windows.Forms.TextBox CreateTextBox(int row, string text)
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                Text = text,
                BackColor = row % 2 == 0 ? ColorTranslator.FromHtml("#202020") : ColorTranslator.FromHtml("#eeeeee"),
                ForeColor = row % 2 == 0 ? ColorTranslator.FromHtml("#eeeeee") : ColorTranslator.FromHtml("#202020"),
                Dock = DockStyle.Fill,
                Height = increasedHeight,
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

        private void EditResultPaidGigRecord(string selectedId, string title, string startDate, string endDate, string payment, string description)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Form1.SQLiteconnectionString))
                {
                    connection.Open();

                    string commandText = $@"UPDATE ResultPaidGig 
                                            SET Title = @Title, 
                                                Start_date = @StartDate, 
                                                End_date = @EndDate, 
                                                Payment_for_gig = @Payment, 
                                                Description = @Description 
                                            WHERE Rec_id = @RecId";

                    using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@RecId", selectedId);
                        command.Parameters.AddWithValue("@Title", Form1.Encrypt_string_with_aes_in_cbc(title));
                        command.Parameters.AddWithValue("@StartDate", Form1.Encrypt_string_with_aes_in_cbc(startDate));
                        command.Parameters.AddWithValue("@EndDate", Form1.Encrypt_string_with_aes_in_cbc(endDate));
                        command.Parameters.AddWithValue("@Payment", Form1.Encrypt_string_with_aes_in_cbc(payment));
                        command.Parameters.AddWithValue("@Description", Form1.Encrypt_string_with_aes_in_cbc(description));

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Form1.ShowLeashmoreMessageBox("Record Edited Successfully!");
                        }
                        else
                        {
                            Form1.ShowErrorMessageBox("Failed to Edit Record", "");
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
