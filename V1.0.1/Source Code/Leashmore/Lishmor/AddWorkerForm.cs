using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leashmore
{
    using Org.BouncyCastle.Ocsp;
    using System;
    using System.Data.SQLite;
    using System.Drawing;
    using System.Windows.Forms;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
    using System.Xml.Linq;
    using System.Security.Cryptography;

    public class AddWorkerForm : Form
    {
        private const int formWidth = 540;
        private const int formHeight = 440;
        private const int fixedColumnWidth = 160;
        private const int emptyColumnWidth = 40;
        private const int increasedHeight = 12;
        private const int buttonHeight = 40;
        private const int reducedButtonHeight = 36;

        private string[,] specializationArray; // Added specializationArray to store the 2D array

        public AddWorkerForm(string[,] specializationArray)
        {
            this.specializationArray = specializationArray; // Set specializationArray
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Size = new Size(formWidth, formHeight);
            this.Text = "Add Worker";
            this.BackColor = ColorTranslator.FromHtml("#3188dc");

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

            mainTable.Controls.Add(CreateLabel("Name:", 0), 0, 0);
            mainTable.Controls.Add(CreateTextBox(0), 1, 0);

            mainTable.Controls.Add(CreateLabel("Birthdate:", 1), 0, 1);
            mainTable.Controls.Add(CreateTextBox(1), 1, 1);

            mainTable.Controls.Add(CreateLabel("ID:", 0), 0, 2);
            mainTable.Controls.Add(CreateTextBox(0), 1, 2);

            mainTable.Controls.Add(CreateLabel("Phone Number:", 1), 0, 3);
            mainTable.Controls.Add(CreateTextBox(1), 1, 3);

            mainTable.Controls.Add(CreateLabel("Email:", 0), 0, 4);
            mainTable.Controls.Add(CreateTextBox(0), 1, 4);

            System.Windows.Forms.Label specializationLabel = new System.Windows.Forms.Label()
            {
                Text = "Specialization:",
                BackColor = ColorTranslator.FromHtml("#3188dc"),
                ForeColor = ColorTranslator.FromHtml("#eeeeee"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Width = fixedColumnWidth,
                Height = reducedButtonHeight
            };

            ComboBox specializationComboBox = new ComboBox()
            {
                BackColor = ColorTranslator.FromHtml("#202020"),
                ForeColor = ColorTranslator.FromHtml("#eeeeee"),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill,
                Height = reducedButtonHeight
            };

            // Populate ComboBox with values from the second column of specializationArray
            for (int i = 0; i < specializationArray.GetLength(0); i++)
            {
                specializationComboBox.Items.Add(specializationArray[i, 1]);
            }

            mainTable.Controls.Add(specializationLabel, 0, 5);
            mainTable.Controls.Add(specializationComboBox, 1, 5);

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

            mainTable.Controls.Add(buttonTable, 0, 6);
            mainTable.SetColumnSpan(buttonTable, 3);

            addRecordButton.Click += (sender, e) =>
            {
                // Retrieve text from each entry
                string nameText = ((TextBox)mainTable.GetControlFromPosition(1, 0)).Text;
                string birthdateText = ((TextBox)mainTable.GetControlFromPosition(1, 1)).Text;
                string idText = ((TextBox)mainTable.GetControlFromPosition(1, 2)).Text;
                string phoneNumberText = ((TextBox)mainTable.GetControlFromPosition(1, 3)).Text;
                string emailText = ((TextBox)mainTable.GetControlFromPosition(1, 4)).Text;

                int selectedIndex = specializationComboBox.SelectedIndex;
                if (selectedIndex != -1)
                {
                    string selectedId = specializationArray[selectedIndex, 0];
                    AddWorkerRecord(nameText, birthdateText, idText, phoneNumberText, emailText, selectedId);
                    this.Close();
                }
                else
                {
                    DialogResult result = Form1.ShowOrangeMessageBox("Specialization isn't selected. Worker will be left without it!");

                    if (result == DialogResult.Yes)
                    {
                        AddWorkerRecord(nameText, birthdateText, idText, phoneNumberText, emailText, "-1");
                        this.Close();
                    }
                    else if (result == DialogResult.No)
                    {
                    }
                    else
                    {
                        this.Close();
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
    static void AddWorkerRecord(string name, string birthdate, string id, string phoneNumber, string email, string specialization)
        {
            string recId = GenerateRandomString(14); // Generate random ID
            while (Form1.CheckIfRecordExists(recId, "Worker") == true) // Check if the record with that ID is already in the database. If true, then keep generating new IDs until DB tells that record with such ID isn't present
                recId = GenerateRandomString(14); // If record with the generated ID already exists, then generate new ID and check again
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Form1.SQLiteconnectionString))
                {
                    connection.Open();

                    string commandText = $@"INSERT INTO Worker 
                                       (Rec_id, Name, Birthdate, ID, Phone_number, Email, Specialization, ResultPaidGigs, HourlyPaidGigs, Payments) 
                                       VALUES 
                                       (@RecId, @Name, @Birthdate, @ID, @PhoneNumber, @Email, @Specialization, @ResultPaidGigs, @HourlyPaidGigs, @Payments)";

                    using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@RecId", recId);
                        command.Parameters.AddWithValue("@Name", Form1.Encrypt_string_with_aes_in_cbc(name));
                        command.Parameters.AddWithValue("@Birthdate", Form1.Encrypt_string_with_aes_in_cbc(birthdate));
                        command.Parameters.AddWithValue("@ID", Form1.Encrypt_string_with_aes_in_cbc(id));
                        command.Parameters.AddWithValue("@PhoneNumber", Form1.Encrypt_string_with_aes_in_cbc(phoneNumber));
                        command.Parameters.AddWithValue("@Email", Form1.Encrypt_string_with_aes_in_cbc(email));
                        command.Parameters.AddWithValue("@Specialization", specialization);
                        command.Parameters.AddWithValue("@ResultPaidGigs", null);
                        command.Parameters.AddWithValue("@HourlyPaidGigs", null);
                        command.Parameters.AddWithValue("@Payments", null);

                        // Execute the command
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
    }
}