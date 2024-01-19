using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leashmore
{
    public class EditWorkerForm : Form
    {
        private const int formWidth = 560;
        private const int formHeight = 440;
        private const int fixedColumnWidth = 170;
        private const int emptyColumnWidth = 40;
        private const int increasedHeight = 12;
        private const int buttonHeight = 40;
        private const int reducedButtonHeight = 36;

        private string[,] specializationArray; // Added specializationArray to store the 2D array
        private string recId; // Record ID to be edited

        public EditWorkerForm(string recId, string[,] specializationArray, string name, string birthdate, string id, string phoneNumber, string email, string specialization)
        {
            this.recId = recId;
            this.specializationArray = specializationArray; // Set specializationArray
            InitializeForm(name, birthdate, id, phoneNumber, email, specialization);
        }

        private void InitializeForm(string name, string birthdate, string id, string phoneNumber, string email, string specialization)
        {
            this.Size = new Size(formWidth, formHeight);
            this.Text = "Edit Worker";
            this.BackColor = ColorTranslator.FromHtml("#131313"); // Set background color

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
            mainTable.Controls.Add(CreateLabel("Old Specialization:", 1), 0, 5);
            mainTable.Controls.Add(CreateLabel(specialization, 1), 1, 5);

            ((TextBox)mainTable.GetControlFromPosition(1, 0)).Text = name;
            ((TextBox)mainTable.GetControlFromPosition(1, 1)).Text = birthdate;
            ((TextBox)mainTable.GetControlFromPosition(1, 2)).Text = id;
            ((TextBox)mainTable.GetControlFromPosition(1, 3)).Text = phoneNumber;
            ((TextBox)mainTable.GetControlFromPosition(1, 4)).Text = email;


            System.Windows.Forms.Label specializationLabel = new System.Windows.Forms.Label()
            {
                Text = "New Specialization:",
                BackColor = ColorTranslator.FromHtml("#131313"),
                ForeColor = ColorTranslator.FromHtml("#F92C5D"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Width = fixedColumnWidth,
                Height = reducedButtonHeight
            };

            ComboBox specializationComboBox = new ComboBox()
            {
                BackColor = ColorTranslator.FromHtml("#F92C5D"),
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

            mainTable.Controls.Add(specializationLabel, 0, 6);
            mainTable.Controls.Add(specializationComboBox, 1, 6);

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

            Button updateRecordButton = new Button()
            {
                Text = "Update",
                BackColor = ColorTranslator.FromHtml("#EEEEEE"),
                ForeColor = ColorTranslator.FromHtml("#131313"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Height = buttonHeight,
                Margin = new Padding(0, 0, 8, 0),
                Anchor = AnchorStyles.Right
            };

            Button cancelButton = new Button()
            {
                Text = "Cancel",
                BackColor = ColorTranslator.FromHtml("#F92C5D"),
                ForeColor = ColorTranslator.FromHtml("#eeeeee"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                Height = buttonHeight,
                Margin = new Padding(8, 0, 0, 0),
                Anchor = AnchorStyles.Left
            };

            buttonTable.Controls.Add(updateRecordButton, 0, 0);
            buttonTable.Controls.Add(cancelButton, 1, 0);

            mainTable.Controls.Add(buttonTable, 0, 7);
            mainTable.SetColumnSpan(buttonTable, 3);

            updateRecordButton.Click += (sender, e) =>
            {
                // Retrieve text from each entry
                string editedNameText = ((TextBox)mainTable.GetControlFromPosition(1, 0)).Text;
                string editedBirthdateText = ((TextBox)mainTable.GetControlFromPosition(1, 1)).Text;
                string editedIdText = ((TextBox)mainTable.GetControlFromPosition(1, 2)).Text;
                string editedPhoneNumberText = ((TextBox)mainTable.GetControlFromPosition(1, 3)).Text;
                string editedEmailText = ((TextBox)mainTable.GetControlFromPosition(1, 4)).Text;

                int selectedIndex = specializationComboBox.SelectedIndex;
                if (selectedIndex != -1)
                {
                    string editedSpecialization = specializationArray[selectedIndex, 0];
                    UpdateWorkerRecord(recId, editedNameText, editedBirthdateText, editedIdText, editedPhoneNumberText, editedEmailText, editedSpecialization);
                    this.Close();
                }
                else
                {
                    DialogResult result = Form1.ShowOrangeMessageBox("Specialization isn't selected. Worker will be left without it!");

                    if (result == DialogResult.Yes)
                    {
                        UpdateWorkerRecord(recId, editedNameText, editedBirthdateText, editedIdText, editedPhoneNumberText, editedEmailText, "-1");
                        this.Close();
                    }
                    else if (result == DialogResult.No)
                    {
                        // Do nothing
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
                BackColor = ColorTranslator.FromHtml("#131313"),
                ForeColor = row % 2 == 0 ? ColorTranslator.FromHtml("#F92C5D") : ColorTranslator.FromHtml("#eeeeee"),
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
                BackColor = row % 2 == 0 ? ColorTranslator.FromHtml("#F92C5D") : ColorTranslator.FromHtml("#eeeeee"),
                ForeColor = row % 2 == 0 ? ColorTranslator.FromHtml("#eeeeee") : ColorTranslator.FromHtml("#F92C5D"),
                Dock = DockStyle.Fill,
                Height = increasedHeight,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };

            return textBox;
        }

        private void UpdateWorkerRecord(string recId, string name, string birthdate, string id, string phoneNumber, string email, string specialization)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(Form1.SQLiteconnectionString))
                {
                    connection.Open();

                    string commandText = $@"UPDATE Worker 
                                       SET 
                                       Name = @Name, 
                                       Birthdate = @Birthdate, 
                                       ID = @ID, 
                                       Phone_number = @PhoneNumber, 
                                       Email = @Email, 
                                       Specialization = @Specialization 
                                       WHERE Rec_id = @RecId";

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

                        // Execute the command
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