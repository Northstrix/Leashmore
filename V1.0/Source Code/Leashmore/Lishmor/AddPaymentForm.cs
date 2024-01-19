using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Data.SQLite;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Leashmore
{
    public class AddPaymentForm : Form
    {
        private const int formWidth = 480;
        private const int formHeight = 168;
        private const int fixedColumnWidth = 160;
        private const int emptyColumnWidth = 40;
        private const int increasedHeight = 12;
        private const int buttonHeight = 40;
        private const int reducedButtonHeight = 36;

        public AddPaymentForm(string worker_name, double entitled_to)
        {
            InitializeForm(worker_name, entitled_to);
        }

        private void InitializeForm(string worker_name, double entitled_to)
        {
            this.Size = new Size(formWidth, formHeight);
            this.Text = $"Pay to {worker_name}";
            this.BackColor = ColorTranslator.FromHtml("#3188dc");

            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3,
                ColumnStyles =
                {
                    new ColumnStyle(SizeType.Absolute, fixedColumnWidth),
                    new ColumnStyle(SizeType.Percent, 100),
                    new ColumnStyle(SizeType.Absolute, emptyColumnWidth)
                },
                RowStyles =
                {
                    new RowStyle(SizeType.AutoSize, increasedHeight),
                    new RowStyle(SizeType.AutoSize, reducedButtonHeight),
                    new RowStyle(SizeType.AutoSize, buttonHeight)
                }
            };

            this.Controls.Add(mainTable);

            mainTable.Controls.Add(CreateLabel($"Amount: {Form1.currency}", 0), 0, 0);
            mainTable.Controls.Add(CreateTextBox(0), 1, 0);

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

            Button addRecordButton = new Button()
            {
                Text = "Add",
                BackColor = ColorTranslator.FromHtml("#24DE9C"),
                ForeColor = ColorTranslator.FromHtml("#202020"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Height = buttonHeight,
                Margin = new Padding(0, 0, 8, 0),
                Anchor = AnchorStyles.Right
            };

            Button cancelButton = new Button()
            {
                Text = "Cancel",
                BackColor = ColorTranslator.FromHtml("#FF6E29"),
                ForeColor = ColorTranslator.FromHtml("#eeeeee"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Height = buttonHeight,
                Margin = new Padding(8, 0, 0, 0),
                Anchor = AnchorStyles.Left
            };

            buttonTable.Controls.Add(addRecordButton, 0, 0);
            buttonTable.Controls.Add(cancelButton, 1, 0);

            mainTable.Controls.Add(buttonTable, 0, 2);
            mainTable.SetColumnSpan(buttonTable, 3);

            addRecordButton.Click += (sender, e) =>
            {
                string amountText = ((TextBox)mainTable.GetControlFromPosition(1, 0)).Text;

                if (!double.TryParse(amountText, out double pay))
                {
                    Form1.ShowErrorMessageBox("Invalid Input", "Payment amount must be a valid numeric value.");
                }
                else
                {
                    if (pay > 0)
                    {
                        if (pay > entitled_to)
                            Form1.ShowErrorMessageBox($"Can't Pay That Much to {worker_name}", $"{worker_name} is only entitled to {Form1.currency}{String.Format("{0:0.00}", entitled_to)}");
                        else
                        {
                            AddPaymentRecord(amountText.Replace(",", "."), worker_name);
                            this.Close();
                        }
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
                BackColor = ColorTranslator.FromHtml("#3188dc"),
                ForeColor = row % 2 == 0 ? ColorTranslator.FromHtml("#202020") : ColorTranslator.FromHtml("#eeeeee"),
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

        private static void AddPaymentRecord(string amount, string worker_name)
        {
            string recId = GenerateRandomString(16); // Generate random ID
            while (Form1.CheckIfRecordExists(recId, "Payment") == true) // Check if the record with that ID is already in the database. If true, then keep generating new IDs until DB tells that record with such ID isn't present
                recId = GenerateRandomString(16); // If record with the generated ID already exists, then generate new ID and check again
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Form1.SQLiteconnectionString))
                {
                    connection.Open();

                    string commandText = $@"INSERT INTO Payment (Rec_id, Amount) VALUES (@RecId, @Amount)";

                    using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@RecId", recId);
                        command.Parameters.AddWithValue("@Amount", Form1.Encrypt_string_with_aes_in_cbc(amount));

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Form1.selected_id = recId;
                            Form1.ShowLeashmoreMessageBox($"Paid {Form1.currency}{amount} to {worker_name}\nYou Should See the \"Record Updated Successfully\" Message Next");
                        }
                        else
                        {
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