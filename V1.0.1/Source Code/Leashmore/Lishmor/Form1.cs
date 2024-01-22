using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Data.SQLite;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Leashmore
{
    public partial class Form1 : Form
    {
        protected static byte[] encryption_key = new byte[16];
        protected static byte[] verification_key = new byte[16];
        protected static byte[] decrypted_tag = new byte[32];
        public static string selected_id;
        private static string[] Worker_ids;
        public const string SQLiteconnectionString = "Data Source=leashmore_database.db;Version=3;";
        public static byte gig_type;
        public static string currency;
        public static long selected_row;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            create_required_tables_if_not_exist();
            string encr_bash_of_mp = get_encr_hash_of_password_from_db();
            if (encr_bash_of_mp == "-1")
                set_password();
            else
                ask_user_for_password(encr_bash_of_mp);

            // Check if the "currency.txt" file exists
            if (File.Exists("currency.txt"))
            {
                // Read the symbol from the file and assign it to the global variable
                currency = File.ReadAllText("currency.txt");
            }
            else
            {
                // If the file doesn't exist, create it and put '$' in it
                File.WriteAllText("currency.txt", "$");
                currency = "$"; // Assign '$' to the global variable
            }

            DisplayWorkerInformation();
        }

        private void DisplayWorkerInformation()
        {
            selected_row = -1;
            // Assume dataGridView1 is the name of your DataGridView control
            dataGridView1.Rows.Clear(); // Clear the DataGridView
            dataGridView1.Columns.Clear(); // Clear existing columns

            // Define DataGridView columns
            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name",
                ReadOnly = true // Set column as read-only
            };

            DataGridViewTextBoxColumn phoneColumn = new DataGridViewTextBoxColumn
            {
                Name = "Phone_number",
                HeaderText = "Phone Number",
                DataPropertyName = "Phone_number",
                ReadOnly = true // Set column as read-only
            };

            DataGridViewTextBoxColumn emailColumn = new DataGridViewTextBoxColumn
            {
                Name = "Email",
                HeaderText = "Email",
                DataPropertyName = "Email",
                ReadOnly = true // Set column as read-only
            };

            // Add columns to the DataGridView
            dataGridView1.Columns.AddRange(nameColumn, phoneColumn, emailColumn);

            // Set DataGridViewCellStyle properties for appearance
            DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(16, 16, 16), // Background color
                ForeColor = Color.FromArgb(238, 238, 238), // Foreground color
                SelectionBackColor = ColorTranslator.FromHtml("#0A6879"), // Selected cell color
                SelectionForeColor = Color.FromArgb(238, 238, 238), // Selected text color
            };

            dataGridView1.DefaultCellStyle = dataGridViewCellStyle;

            // Set AutoSizeColumnsMode to Fill
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set Dock property to Fill
            dataGridView1.Dock = DockStyle.Fill;

            // Disable row and column headers
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;

            // Set SelectionMode to FullRowSelect
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Set MultiSelect property to false (allow only single row selection)
            dataGridView1.MultiSelect = false;

            // Set ReadOnly property for the entire DataGridView
            dataGridView1.ReadOnly = true;

            // Allow users to copy cell contents
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            // Handle CellFormatting event to set the background color and bold text of the first row
            dataGridView1.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex == 0)
                {
                    e.CellStyle.BackColor = ColorTranslator.FromHtml("#363636");
                    e.CellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
            };

            // Fill your DataGridView with data (using the same approach as before)
            FillDataGridView();
        }

        private void FillDataGridView()
        {
            // Create a List to store Rec_id values
            List<string> recIdsList = new List<string>();

            dataGridView1.Rows.Add("Name", "Phone Number", "Email");

            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();

                // Assuming Worker is your table name
                string query = "SELECT Rec_id, Name, Phone_number, Email FROM Worker";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Extracting information
                            string recId = reader["Rec_id"].ToString();
                            string name = reader["Name"].ToString();
                            string phoneNumber = reader["Phone_number"].ToString();
                            string email = reader["Email"].ToString();

                            // Add the information to dataGridView1
                            dataGridView1.Rows.Add(Decrypt_string_with_aes_in_cbc(name), Decrypt_string_with_aes_in_cbc(phoneNumber), Decrypt_string_with_aes_in_cbc(email));

                            // Add the Rec_id to the List
                            recIdsList.Add(recId);
                        }
                    }
                }

                connection.Close();
            }

            // Convert the List to an array at the end
            Worker_ids = recIdsList.ToArray();
        }

        private void set_password()
        {
            string user_password = get_data_from_entry("Set Your Password", false);
            string hashed_password = HashStringWithSHA512(user_password, 4 * CalculateAsciiSum(HashStringWithSHA512(user_password, 500)));
            byte[] source = StringToByteArray(hashed_password);
            for (int i = 0; i < 16; i++)
            {
                encryption_key[i] = source[i];
                verification_key[i] = source[i + 16];
            }
            byte[] to_be_hmaced = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                to_be_hmaced[i] = source[i + 32];
            }


            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();

                try
                {
                    string query = $"INSERT INTO Unlock (Rec_id, Encrypted_hash_of_the_password) VALUES (1, '" + Encrypt_hash_with_aes_in_cbc(CalculateHMACSHA256(to_be_hmaced)) + "')";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        ShowLeashmoreMessageBox("Password Set Successfully");
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessageBox("Something went wrong with the database", $"Error: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            set_unlocked_status();
        }

        private void ask_user_for_password(string encr_bash_of_mp)
        {
            string user_password = get_data_from_entry("Enter Your Password", true);
            string hashed_password = HashStringWithSHA512(user_password, 4 * CalculateAsciiSum(HashStringWithSHA512(user_password, 500)));
            byte[] source = StringToByteArray(hashed_password);
            for (int i = 0; i < 16; i++)
            {
                encryption_key[i] = source[i];
                verification_key[i] = source[i + 16];
            }
            byte[] to_be_hmaced = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                to_be_hmaced[i] = source[i + 32];
            }

            if (!Decrypt_hash_with_aes_in_cbc(encr_bash_of_mp).SequenceEqual(CalculateHMACSHA256(to_be_hmaced)))
            {
                ShowErrorMessageBox("Wrong Password", "Please, Try Again");
                ask_user_for_password(encr_bash_of_mp);
            }
            else
                set_unlocked_status();
        }

        private void set_unlocked_status()
        {
            sodtware_status_label.Text = "Unlocked";
            button3.Text = "Lock";
            button3.BackColor = Color.FromArgb(255, 61, 36);
            button3.ForeColor = Color.FromArgb(238, 238, 238);
        }

        private void set_locked_status()
        {
            sodtware_status_label.Text = "Locked";
            button3.Text = "Unlock";
            button3.BackColor = Color.FromArgb(40, 201, 239);
            button3.ForeColor = Color.FromArgb(22, 22, 22);
        }

        public static void ShowErrorMessageBox(string line1, string line2)
        {
            Form customMessageBox = new Form
            {
                Text = "Leashmore Error",
                Size = new Size(640, 162),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(171, 49, 18)
            };

            // Create label for the first line
            Label label1 = new Label
            {
                Text = line1,
                ForeColor = Color.FromArgb(238, 238, 238),
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customMessageBox.Controls.Add(label1);

            // Create label for the second line
            Label label2 = new Label
            {
                Text = line2,
                ForeColor = Color.FromArgb(238, 238, 238),
                Font = new Font("Arial", 14),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customMessageBox.Controls.Add(label2);

            // Create OK button
            Button okButton = new Button
            {
                Text = "OK",
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(32, 32, 32), // "#202020"
                ForeColor = Color.FromArgb(238, 238, 238), // "#EEEEEE"
                DialogResult = DialogResult.OK,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            customMessageBox.Controls.Add(okButton);

            label1.Location = new Point((customMessageBox.ClientSize.Width) / 2, +10);
            label2.Location = new Point((customMessageBox.ClientSize.Width) / 2, label1.Bottom + 10);
            okButton.Location = new Point((customMessageBox.ClientSize.Width - okButton.Width) / 2, label2.Bottom + 12);

            CenterLabelText(label1, customMessageBox);
            CenterLabelText(label2, customMessageBox);

            // Handle Resize event to adjust positions dynamically
            customMessageBox.Resize += (sender, e) =>
            {
                CenterLabelText(label1, customMessageBox);
                CenterLabelText(label2, customMessageBox);
                okButton.Location = new Point((customMessageBox.ClientSize.Width - okButton.Width) / 2, label2.Bottom + 20);
            };

            // Show the message box
            customMessageBox.ShowDialog();
        }

        public static DialogResult ShowOrangeMessageBox(string line1)
        {
            Form customMessageBox = new Form
            {
                Text = "Leashmore Warning",
                Size = new Size(540, 162),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(237, 137, 40)
            };

            // Create label for the first line
            Label label1 = new Label
            {
                Text = line1,
                ForeColor = Color.FromArgb(238, 238, 238),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customMessageBox.Controls.Add(label1);

            // Create label for the second line
            Label label2 = new Label
            {
                Text = "Would you like to continue?",
                ForeColor = Color.FromArgb(238, 238, 238),
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customMessageBox.Controls.Add(label2);

            // Create Yes button
            Button yesButton = new Button
            {
                Text = "Yes",
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(32, 32, 32), // "#202020"
                ForeColor = Color.FromArgb(238, 238, 238), // "#EEEEEE"
                DialogResult = DialogResult.Yes,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            customMessageBox.Controls.Add(yesButton);

            // Create No button
            Button noButton = new Button
            {
                Text = "No",
                Size = new Size(60, 30),
                BackColor = Color.FromArgb(32, 32, 32), // "#202020"
                ForeColor = Color.FromArgb(238, 238, 238), // "#EEEEEE"
                DialogResult = DialogResult.No,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            customMessageBox.Controls.Add(noButton);

            label1.Location = new Point((customMessageBox.ClientSize.Width) / 2, +10);
            label2.Location = new Point((customMessageBox.ClientSize.Width) / 2, label1.Bottom + 10);

            int buttonMargin = 30;
            int buttonWidth = (customMessageBox.ClientSize.Width - 3 * buttonMargin) / 2;

            yesButton.Size = new Size(buttonWidth, 30);
            noButton.Size = new Size(buttonWidth, 30);

            yesButton.Location = new Point(buttonMargin, label2.Bottom + 20);
            noButton.Location = new Point(yesButton.Right + buttonMargin, label2.Bottom + 20);

            CenterLabelText(label1, customMessageBox);
            CenterLabelText(label2, customMessageBox);

            // Handle Resize event to adjust positions dynamically
            customMessageBox.Resize += (sender, e) =>
            {
                CenterLabelText(label1, customMessageBox);
                CenterLabelText(label2, customMessageBox);

                buttonWidth = (customMessageBox.ClientSize.Width - 3 * buttonMargin) / 2;
                yesButton.Size = new Size(buttonWidth, 30);
                noButton.Size = new Size(buttonWidth, 30);

                yesButton.Location = new Point(buttonMargin, label2.Bottom + 20);
                noButton.Location = new Point(yesButton.Right + buttonMargin, label2.Bottom + 20);
            };

            // Show the message box
            DialogResult result = customMessageBox.ShowDialog();

            return result;
        }

        public static void ShowLeashmoreMessageBox(string line)
        {
            Form customMessageBox = new Form
            {
                Text = "Leashmore Message",
                Size = new Size(700, 162),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = ColorTranslator.FromHtml("#08162F")
            };

            // Create label for the first line
            Label label1 = new Label
            {
                Text = line,
                ForeColor = Color.FromArgb(238, 238, 238),
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customMessageBox.Controls.Add(label1);

            // Create OK button
            Button okButton = new Button
            {
                Text = "OK",
                Size = new Size(96, 32),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(32, 32, 32),
                BackColor = Color.FromArgb(198, 198, 198),
                DialogResult = DialogResult.OK,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            customMessageBox.Controls.Add(okButton);

            label1.Location = new Point((customMessageBox.ClientSize.Width) / 2, 12);
            okButton.Location = new Point((customMessageBox.ClientSize.Width - okButton.Width) / 2, label1.Bottom + 20);

            CenterLabelText(label1, customMessageBox);

            // Handle Resize event to adjust positions dynamically
            customMessageBox.Resize += (sender, e) =>
            {
                CenterLabelText(label1, customMessageBox);
                okButton.Location = new Point((customMessageBox.ClientSize.Width - okButton.Width) / 2, label1.Bottom + 20);
            };

            // Show the message box
            customMessageBox.ShowDialog();
        }


        static void CenterLabelText(Label label, Control parent)
        {
            // Center the text horizontally in the label
            label.Location = new Point((parent.ClientSize.Width - label.Width) / 2, label.Location.Y);
        }

        private static string get_encr_hash_of_password_from_db()
        {
            StringBuilder enc_hash_to_ret = new StringBuilder();
            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();
                int recId = 1;
                string query = $"SELECT * FROM Unlock WHERE Rec_id = {recId}";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Record found, you can access the values using reader["ColumnName"]
                            int foundRecId = reader.GetInt32(reader.GetOrdinal("Rec_id"));
                            enc_hash_to_ret.Append(reader.GetString(reader.GetOrdinal("Encrypted_hash_of_the_password")));
                        }
                        else
                        {
                            enc_hash_to_ret.Append("-1");
                        }
                    }
                }

                connection.Close();
            }
            return enc_hash_to_ret.ToString();
        }

        public static bool CheckIfRecordExists(string recId, string table)
        {
            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();

                try
                {
                    string query = $"SELECT 1 FROM {table} WHERE Rec_id = '{recId}' LIMIT 1";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();
                        return result != null && result != DBNull.Value;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static void create_required_tables_if_not_exist()
        {
            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();

                CreateTableIfNotExist(connection, "Unlock", "Rec_id INTEGER, Encrypted_hash_of_the_password TEXT");
                CreateTableIfNotExist(connection, "Worker", "Rec_id TEXT, Name Text, Birthdate Text, ID Text, Phone_number Text, Email Text, Specialization Text, ResultPaidGigs Text, HourlyPaidGigs Text, Payments Text");
                CreateTableIfNotExist(connection, "Specialization", "Rec_id TEXT, Title Text, Description Text");
                CreateTableIfNotExist(connection, "ResultPaidGig", "Rec_id TEXT, Title Text, Start_date Text, End_date Text, Payment_for_gig Text, Description Text");
                CreateTableIfNotExist(connection, "HourlyPaidGig", "Rec_id TEXT, Title Text, Start_date Text, End_date Text, Hours Text, Hourly_pay Text, Description Text");
                CreateTableIfNotExist(connection, "Payment", "Rec_id TEXT, Amount Text");

                connection.Close();
            }
        }

        private bool DeleteRecord(string table, string id)
        {
            bool rec_deltd = false;
            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Construct the DELETE statement
                    command.CommandText = $"DELETE FROM {table} WHERE Rec_id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        // Execute the DELETE statement
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            rec_deltd = true;
                        }
                        else
                        {
                            ShowErrorMessageBox("Failed to Delete Record", $"Record \"{id}\" isn't found");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessageBox($"Failed to Delete Record \"{id}\"", ex.Message);
                    }
                }
                connection.Close();
            }
            return rec_deltd;
        }

        private static void CreateTableIfNotExist(SQLiteConnection connection, string tableName, string columns)
        {
            using (SQLiteCommand command = new SQLiteCommand($"CREATE TABLE IF NOT EXISTS {tableName} ({columns});", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static string get_data_from_entry(string prompt, bool displayStars)
        {
            Form customForm = new Form
            {
                Text = "Leashmore",
                Size = new Size(320, 170),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = ColorTranslator.FromHtml("#7B08A5")
            };

            // Create label
            Label label = new Label
            {
                Text = prompt,
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customForm.Controls.Add(label);

            // Create text field
            TextBox textField = new TextBox
            {
                Size = new Size(200, 30),
                Location = new Point((customForm.ClientSize.Width - 200) / 2, label.Bottom + 10),
                Font = new Font("Segoe UI", 12),
                BackColor = ColorTranslator.FromHtml("#2C2C2C"),
                ForeColor = ColorTranslator.FromHtml("#E4E3DF")
            };
            customForm.Controls.Add(textField);

            // If displayStars is true, set the UseSystemPasswordChar property to true
            if (displayStars)
            {
                textField.UseSystemPasswordChar = true;
            }

            // Create Continue button
            Button continueButton = new Button
            {
                Text = "Continue",
                Size = new Size(120, 38),
                BackColor = ColorTranslator.FromHtml("#7B08A5"),
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"),
                DialogResult = DialogResult.Yes,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 3, BorderColor = ColorTranslator.FromHtml("#E4E3DF") },
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            customForm.Controls.Add(continueButton);

            label.Location = new Point((customForm.ClientSize.Width) / 2, +10);
            textField.Location = new Point((customForm.ClientSize.Width - textField.Width) / 2, label.Bottom + 10);
            CenterLabelText(label, customForm);
            continueButton.Location = new Point((customForm.ClientSize.Width - continueButton.Width) / 2, textField.Bottom + 10);

            // Handle Resize event to adjust positions dynamically
            customForm.Resize += (sender, e) =>
            {
                textField.Location = new Point((customForm.ClientSize.Width - textField.Width) / 2, label.Bottom + 10);
                CenterLabelText(label, customForm);
                continueButton.Location = new Point((customForm.ClientSize.Width - continueButton.Width) / 2, textField.Bottom + 20);
            };

            // Show the form
            DialogResult result = customForm.ShowDialog();

            return textField.Text;
        }

        private static byte[] GenerateRandomByteArray(int length)
        {
            byte[] randomBytes = new byte[length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return randomBytes;
        }

        public static string Encrypt_string_with_aes_in_cbc(string plaintext)
        {
            byte[] input = Encoding.Unicode.GetBytes(plaintext);
            byte[] iv = GenerateRandomByteArray(16);
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");
            cipher.Init(true, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", encryption_key), iv));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CipherStream cipherStream = new CipherStream(memoryStream, null, cipher))
                {
                    cipherStream.Write(input, 0, input.Length);
                }

                return Encrypt_hash_with_aes_in_cbc(CalculateHMACSHA256(input)) + BitConverter.ToString(EncryptAES(iv)).Replace("-", "") + BitConverter.ToString(memoryStream.ToArray()).Replace("-", "");
            }
        }

        private static string Decrypt_string_with_aes_in_cbc(string ciphertext)
        {
            try
            {
                decrypted_tag = Decrypt_hash_with_aes_in_cbc(ciphertext.Substring(0, 96));
                byte[] encrypted_iv = StringToByteArray(ciphertext.Substring(96, 32));
                byte[] iv = DecryptAES(encrypted_iv);
                byte[] input = StringToByteArray(ciphertext.Substring(128));
                IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");
                cipher.Init(false, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", encryption_key), iv));

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CipherStream cipherStream = new CipherStream(memoryStream, null, cipher))
                    {
                        cipherStream.Write(input, 0, input.Length);
                    }

                    if (!CalculateHMACSHA256(memoryStream.ToArray()).AsSpan().SequenceEqual(decrypted_tag))
                        ShowErrorMessageBox("Failed to Verify Integrity/Authenticity of a Ciphertext", "Decrypted and Computed Tags Don't Match");


                    return Encoding.Unicode.GetString(memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessageBox("Failed to Decrypt Ciphertext", "Error: " + ex.Message);
                return "\"Decryption Failed\"";

            }
        }

        private static string Encrypt_hash_with_aes_in_cbc(byte[] input)
        {
            byte[] iv = GenerateRandomByteArray(16);
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/NoPadding");
            cipher.Init(true, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", encryption_key), iv));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CipherStream cipherStream = new CipherStream(memoryStream, null, cipher))
                {
                    cipherStream.Write(input, 0, input.Length);
                }

                return BitConverter.ToString(EncryptAES(iv)).Replace("-", "") + BitConverter.ToString(memoryStream.ToArray()).Replace("-", "");
            }
        }

        private static byte[] Decrypt_hash_with_aes_in_cbc(string ciphertext)
        {
            byte[] encrypted_iv = StringToByteArray(ciphertext.Substring(0, 32));
            byte[] iv = DecryptAES(encrypted_iv);
            byte[] input = StringToByteArray(ciphertext.Substring(32));
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/NoPadding");
            cipher.Init(false, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", encryption_key), iv));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CipherStream cipherStream = new CipherStream(memoryStream, null, cipher))
                {
                    cipherStream.Write(input, 0, input.Length);
                }

                return memoryStream.ToArray();
            }
        }

        private static byte[] EncryptAES(byte[] data)
        {
            // Create the AES cipher with ECB mode and no padding
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/ECB/NoPadding");
            cipher.Init(true, new KeyParameter(encryption_key));

            // Encrypt the data
            return cipher.DoFinal(data);
        }

        private static byte[] DecryptAES(byte[] encryptedData)
        {
            // Create the AES cipher with ECB mode and no padding
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/ECB/NoPadding");
            cipher.Init(false, new KeyParameter(encryption_key));

            // Decrypt the data
            return cipher.DoFinal(encryptedData);
        }

        public static int CalculateAsciiSum(string input)
        {
            int sum = 0;

            foreach (char character in input)
            {
                sum += (int)character;
            }

            return sum;
        }

        public static string HashStringWithSHA512(string input, int iterations)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(input);

                for (int i = 0; i < iterations; i++)
                {
                    data = sha512.ComputeHash(data);
                }

                // Convert the final hash to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in data)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static byte[] StringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] byteArray = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                byteArray[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return byteArray;
        }

        private static byte[] CalculateHMACSHA256(byte[] data)
        {
            using (HMACSHA256 hmac = new HMACSHA256(verification_key))
            {
                return hmac.ComputeHash(data);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Unlock")
            {
                ask_user_for_password(get_encr_hash_of_password_from_db());
                set_unlocked_status();
                DisplayWorkerInformation();
            }

            else if (button3.Text == "Lock")
            {
                for (int i = 0; i < 16; i++)
                {
                    encryption_key[i] = 0;
                    verification_key[i] = 0;
                }
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                set_locked_status();
            }
        }

        private bool software_unlocked()
        {
            if (button3.Text == "Lock")
                return true;
            else
            {
                ShowErrorMessageBox("The Software is Locked", "Unlock the Software to Continue");
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
            {
                using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                {
                    connection.Open();
                    AddWorkerForm addWorkerForm = new AddWorkerForm(ExtractRecordTitlesAndIDs(connection, "Specialization", "Title"));
                    addWorkerForm.ShowDialog();
                    connection.Close();
                    selected_id = "";
                    DisplayWorkerInformation();
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
            {
                AddSpecialization addSpecialization = new AddSpecialization();
                addSpecialization.ShowDialog();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
            {
                using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                {
                    connection.Open();

                    SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "Specialization", "Title"), "Edit Specialization", "View");
                    DialogResult result = selectRecordForm.ShowDialog();

                    if (result == DialogResult.Continue)
                    {
                        EditSpecialization espec = new EditSpecialization(ExtractSpecialization(selected_id), selected_id);
                        espec.ShowDialog();
                    }

                    if (result == DialogResult.Cancel)
                    {
                        ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                    }

                    if (result == DialogResult.Abort)
                    {
                        ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                    }

                    connection.Close();
                    selected_id = "";
                }
            }
        }

        private void Update_worker_table_after_deleting_spec(string spec_to_rem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();

                // Assuming Worker is your table name
                string query = $"UPDATE Worker SET Specialization = '-1' WHERE Specialization = '{spec_to_rem}'";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
            {
                using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                {
                    connection.Open();

                    SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "Specialization", "Title"), "Delete Specialization", "Delete");
                    DialogResult result = selectRecordForm.ShowDialog();

                    if (result == DialogResult.Continue)
                    {
                        if (DeleteRecord("Specialization", selected_id))
                        {
                            Update_worker_table_after_deleting_spec(selected_id);
                            ShowLeashmoreMessageBox("Record Deleted Successfully"); //ShowLeashmoreMessageBox($"Record \"{selected_id}\" has been deleted!");
                        }
                    }

                    if (result == DialogResult.Cancel)
                    {
                        ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                    }

                    if (result == DialogResult.Abort)
                    {
                        ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                    }

                    connection.Close();
                    selected_id = "";
                }
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
            {
                using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                {
                    connection.Open();

                    SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "Specialization", "Title"), "View Specialization", "View");
                    DialogResult result = selectRecordForm.ShowDialog();

                    if (result == DialogResult.Continue)
                    {
                        ViewSpecialization vspec = new ViewSpecialization(ExtractSpecialization(selected_id));
                        vspec.ShowDialog();
                    }

                    if (result == DialogResult.Cancel)
                    {
                        ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                    }

                    if (result == DialogResult.Abort)
                    {
                        ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                    }

                    connection.Close();
                    selected_id = "";
                }
            }
        }

        static string[,] ExtractRecordTitlesAndIDs(SQLiteConnection connection, string table, string title_field)
        {
            // Define the SQL command to select records from the "Specialization" table
            string selectSql = "SELECT Rec_id, " + title_field + " FROM " + table + ";";

            // Create a list to store records
            List<string[]> recordsList = new List<string[]>();

            using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Read values from the reader
                        string recId = GetSafeString(reader, "Rec_id");
                        string title = GetSafeString(reader, title_field);

                        // Add values to the list only if they are not null and not DBNull
                        if (recId != null && title != null)
                        {
                            recordsList.Add(new string[] { recId, Decrypt_string_with_aes_in_cbc(title) });
                        }
                    }
                }
            }

            // Convert the list to a 2D array
            string[,] recordsArray = new string[recordsList.Count, 2];
            for (int i = 0; i < recordsList.Count; i++)
            {
                recordsArray[i, 0] = recordsList[i][0]; // Rec_id
                recordsArray[i, 1] = recordsList[i][1]; // Title
            }

            return recordsArray;
        }

        static string[] ExtractSpecialization(string recId)
        {
            string selectSql = $"SELECT Title, Description FROM Specialization WHERE Rec_id = '{recId}'";

            string[] recordsArray = new string[2];
            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recordsArray[0] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Title"));
                            recordsArray[1] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Description"));
                        }
                    }
                }
                connection.Close();
            }
            return recordsArray;
        }

        static string GetSafeString(SQLiteDataReader reader, string columnName)
        {
            int columnIndex = reader.GetOrdinal(columnName);
            return reader.IsDBNull(columnIndex) ? null : reader.GetString(columnIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
                DisplayWorkerInformation();
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
            {
                gig_type = 0;
                DialogResult gigTypeResult = GigTypeMessageBox.Show();

                if (gigTypeResult == DialogResult.Yes)
                {
                    if (gig_type == 0)
                        ShowErrorMessageBox("Gig Type isn't Selected", "Please, Try Again");
                    if (gig_type == 1)
                    {
                        AddResultPaidGigForm resultPaidGigForm = new AddResultPaidGigForm();
                        resultPaidGigForm.ShowDialog();

                    }
                    if (gig_type == 2)
                    {
                        AddHourlyPaidGigForm hourlyPaidGigForm = new AddHourlyPaidGigForm();
                        hourlyPaidGigForm.ShowDialog();
                    }
                }
                else if (gigTypeResult == DialogResult.Cancel)
                {
                    ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                }
            }
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
            {
                gig_type = 0;
                DialogResult gigTypeResult = GigTypeMessageBox.Show();

                if (gigTypeResult == DialogResult.Yes)
                {
                    if (gig_type == 0)
                        ShowErrorMessageBox("Gig Type isn't Selected", "Please, Try Again");
                    if (gig_type == 1)
                    {

                        using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                        {
                            connection.Open();

                            SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "ResultPaidGig", "Title"), "Edit Result Paid Gig", "Edit");
                            DialogResult result = selectRecordForm.ShowDialog();

                            if (result == DialogResult.Continue)
                            {
                                // Assume you have the extracted data
                                string[] resultPaidGigData = ExtractResultPaidGig(selected_id);

                                // Create an instance of EditResultPaidGigForm
                                EditResultPaidGigForm editForm = new EditResultPaidGigForm(selected_id, resultPaidGigData[0], resultPaidGigData[1], resultPaidGigData[2], resultPaidGigData[3], resultPaidGigData[4]);

                                // Show the form
                                editForm.ShowDialog();
                            }

                            if (result == DialogResult.Cancel)
                            {
                                ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                            }

                            if (result == DialogResult.Abort)
                            {
                                ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                            }

                            connection.Close();
                            selected_id = "";
                        }
                    }
                    if (gig_type == 2)
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                        {
                            connection.Open();

                            SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "HourlyPaidGig", "Title"), "Edit Hourly Paid Gig", "Edit");
                            DialogResult result = selectRecordForm.ShowDialog();

                            if (result == DialogResult.Continue)
                            {
                                // Assume you have the extracted data
                                string[] hourlyPaidGigData = ExtractHourlyPaidGig(selected_id);

                                // Create an instance of EditResultPaidGigForm
                                EditHourlyPaidGigForm editForm = new EditHourlyPaidGigForm(selected_id, hourlyPaidGigData[0], hourlyPaidGigData[1], hourlyPaidGigData[2], hourlyPaidGigData[3], hourlyPaidGigData[4], hourlyPaidGigData[5]);

                                // Show the form
                                editForm.ShowDialog();
                            }

                            if (result == DialogResult.Cancel)
                            {
                                ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                            }

                            if (result == DialogResult.Abort)
                            {
                                ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                            }

                            connection.Close();
                            selected_id = "";
                        }
                    }
                }
                else if (gigTypeResult == DialogResult.Cancel)
                {
                    ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                }
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
            {

                gig_type = 0;
                DialogResult gigTypeResult = GigTypeMessageBox.Show();

                if (gigTypeResult == DialogResult.Yes)
                {
                    if (gig_type == 0)
                        ShowErrorMessageBox("Gig Type isn't Selected", "Please, Try Again");
                    if (gig_type == 1)
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                        {
                            connection.Open();

                            SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "ResultPaidGig", "Title"), "Delete Result Paid Gig", "Delete");
                            DialogResult result = selectRecordForm.ShowDialog();

                            if (result == DialogResult.Continue)
                            {
                                if (DeleteRecord("ResultPaidGig", selected_id))
                                {
                                    Update_worker_table_after_deleting_spec(selected_id);
                                    ShowLeashmoreMessageBox("Record Deleted Successfully");
                                }
                            }

                            if (result == DialogResult.Cancel)
                            {
                                ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                            }

                            if (result == DialogResult.Abort)
                            {
                                ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                            }

                            connection.Close();
                            selected_id = "";
                        }

                    }
                    if (gig_type == 2)
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                        {
                            connection.Open();

                            SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "HourlyPaidGig", "Title"), "Delete Hourly Paid Gig", "Delete");
                            DialogResult result = selectRecordForm.ShowDialog();

                            if (result == DialogResult.Continue)
                            {
                                if (DeleteRecord("HourlyPaidGig", selected_id))
                                {
                                    Update_worker_table_after_deleting_spec(selected_id);
                                    ShowLeashmoreMessageBox("Record Deleted Successfully");
                                }
                            }

                            if (result == DialogResult.Cancel)
                            {
                                ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                            }

                            if (result == DialogResult.Abort)
                            {
                                ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                            }

                            connection.Close();
                            selected_id = "";
                        }
                    }
                }
                else if (gigTypeResult == DialogResult.Cancel)
                {
                    ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                }
            }
        }

        static string[] ExtractResultPaidGig(string recId)
        {
            string selectSql = $"SELECT Title, Start_date, End_date, Payment_for_gig, Description FROM ResultPaidGig WHERE Rec_id = '{recId}'";

            string[] recordsArray = new string[5];
            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recordsArray[0] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Title"));
                            recordsArray[1] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Start_date"));
                            recordsArray[2] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "End_date"));
                            recordsArray[3] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Payment_for_gig"));
                            recordsArray[4] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Description"));
                        }
                    }
                }
                connection.Close();
            }
            return recordsArray;
        }

        static string[] ExtractHourlyPaidGig(string recId)
        {
            string selectSql = $"SELECT Title, Start_date, End_date, Hours, Hourly_pay, Description FROM HourlyPaidGig WHERE Rec_id = '{recId}'";

            string[] recordsArray = new string[6];
            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recordsArray[0] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Title"));
                            recordsArray[1] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Start_date"));
                            recordsArray[2] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "End_date"));
                            recordsArray[3] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Hours"));
                            recordsArray[4] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Hourly_pay"));
                            recordsArray[5] = Decrypt_string_with_aes_in_cbc(GetSafeString(reader, "Description"));
                        }
                    }
                }
                connection.Close();
            }
            return recordsArray;
        }

        private void viewToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
            {
                gig_type = 0;
                DialogResult gigTypeResult = GigTypeMessageBox.Show();

                if (gigTypeResult == DialogResult.Yes)
                {
                    if (gig_type == 0)
                        ShowErrorMessageBox("Gig Type isn't Selected", "Please, Try Again");
                    if (gig_type == 1)
                    {

                        using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                        {
                            connection.Open();

                            SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "ResultPaidGig", "Title"), "View Result Paid Gig", "View");
                            DialogResult result = selectRecordForm.ShowDialog();

                            if (result == DialogResult.Continue)
                            {
                                // Assume you have the extracted data
                                string[] resultPaidGigData = ExtractResultPaidGig(selected_id);

                                // Create an instance of ViewResultPaidGigForm
                                ViewResultPaidGigForm viewForm = new ViewResultPaidGigForm(resultPaidGigData[0], resultPaidGigData[1], resultPaidGigData[2], resultPaidGigData[3], resultPaidGigData[4]);

                                // Show the form
                                viewForm.ShowDialog();
                            }

                            if (result == DialogResult.Cancel)
                            {
                                ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                            }

                            if (result == DialogResult.Abort)
                            {
                                ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                            }

                            connection.Close();
                            selected_id = "";
                        }
                    }
                    if (gig_type == 2)
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                        {
                            connection.Open();

                            SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "HourlyPaidGig", "Title"), "View Hourly Paid Gig", "View");
                            DialogResult result = selectRecordForm.ShowDialog();

                            if (result == DialogResult.Continue)
                            {
                                // Assume you have the extracted data
                                string[] hourlyPaidGigData = ExtractHourlyPaidGig(selected_id);

                                // Create an instance of ViewResultPaidGigForm
                                ViewHourlyPaidGigForm viewForm = new ViewHourlyPaidGigForm(hourlyPaidGigData[0], hourlyPaidGigData[1], hourlyPaidGigData[2], hourlyPaidGigData[3], hourlyPaidGigData[4], hourlyPaidGigData[5]);

                                // Show the form
                                viewForm.ShowDialog();
                            }

                            if (result == DialogResult.Cancel)
                            {
                                ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                            }

                            if (result == DialogResult.Abort)
                            {
                                ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                            }

                            connection.Close();
                            selected_id = "";
                        }
                    }
                }
                else if (gigTypeResult == DialogResult.Cancel)
                {
                    ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                }
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows[0].Index > 0 && dataGridView1.SelectedRows[0].Index < (Worker_ids.Length + 1))
                    selected_row = dataGridView1.SelectedRows[0].Index - 1;
                else
                    selected_row = -1;

            }
            fill_labels_with_basic_worker_info();
        }

        private void button4_Click(object sender, EventArgs e) // Add Gig to Worker
        {
            if (software_unlocked())
            {
                if (selected_row != -1)
                {
                    gig_type = 0;
                    DialogResult gigTypeResult = GigTypeMessageBox.Show();

                    if (gigTypeResult == DialogResult.Yes)
                    {
                        if (gig_type == 0)
                            ShowErrorMessageBox("Gig Type isn't Selected", "Please, Try Again");
                        if (gig_type == 1)
                        {

                            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                            {
                                connection.Open();

                                SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "ResultPaidGig", "Title"), "Select Result Paid Gig", "Add");
                                DialogResult result = selectRecordForm.ShowDialog();

                                if (result == DialogResult.Continue)
                                {
                                    string all_extracted_rp_gigs = Extract_value_from_record("Worker", Worker_ids[selected_row], "ResultPaidGigs");
                                    if (all_extracted_rp_gigs.Contains(selected_id))
                                    {
                                        ShowErrorMessageBox("Can't add the same gig twice", "Worker already has that gig.");
                                    }
                                    else
                                    {
                                        UpdateResultPaidGigsForWorker(Worker_ids[selected_row], all_extracted_rp_gigs + selected_id + ",");
                                    }
                                }

                                if (result == DialogResult.Cancel)
                                {
                                    ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                                }

                                if (result == DialogResult.Abort)
                                {
                                    ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                                }

                                connection.Close();
                                selected_id = "";
                            }
                        }
                        if (gig_type == 2)
                        {
                            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                            {
                                connection.Open();

                                SelectRecordForm selectRecordForm = new SelectRecordForm(ExtractRecordTitlesAndIDs(connection, "HourlyPaidGig", "Title"), "Select Hourly Paid Gig", "Add");
                                DialogResult result = selectRecordForm.ShowDialog();

                                if (result == DialogResult.Continue)
                                {
                                    string all_extracted_hp_gigs = Extract_value_from_record("Worker", Worker_ids[selected_row], "HourlyPaidGigs");
                                    if (all_extracted_hp_gigs.Contains(selected_id))
                                    {
                                        ShowErrorMessageBox("Can't add the same gig twice", "Worker already has that gig.");
                                    }
                                    else
                                    {
                                        UpdateHourlyPaidGigsForWorker(Worker_ids[selected_row], all_extracted_hp_gigs + selected_id + ",");
                                    }
                                }

                                if (result == DialogResult.Cancel)
                                {
                                    ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                                }

                                if (result == DialogResult.Abort)
                                {
                                    ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                                }

                                connection.Close();
                                selected_id = "";
                            }
                        }
                    }
                    else if (gigTypeResult == DialogResult.Cancel)
                    {
                        ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                    }
                }
                else
                    ShowErrorMessageBox("Record Selection Required", "Please select a worker record first.");
            }
        }

        private static string Extract_value_from_record(string table_name, string Record_id, string column_name)
        {
            string resultPaidGigs = string.Empty;

            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();
                string query = $"SELECT {column_name} FROM {table_name} WHERE Rec_id = @Record_id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Record_id", Record_id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            resultPaidGigs = reader[column_name].ToString();
                        }
                    }
                }
                connection.Close();
            }
            return resultPaidGigs;
        }

        private static void UpdateResultPaidGigsForWorker(string workerRecId, string updatedResultPaidGigs)
        {
            string query = $"UPDATE Worker SET ResultPaidGigs = @UpdatedResultPaidGigs WHERE Rec_id = @WorkerRecId";
            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UpdatedResultPaidGigs", updatedResultPaidGigs);
                    command.Parameters.AddWithValue("@WorkerRecId", workerRecId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ShowLeashmoreMessageBox("Result Paid Gig Added to Worker Successfully.");
                    }
                    else
                    {
                        ShowErrorMessageBox("Error", "Failed to Add Result Paid Gig to Worker.");
                    }
                }
                connection.Close();
            }
        }

        private static void UpdateHourlyPaidGigsForWorker(string workerRecId, string updatedHourlyPaidGigs)
        {
            string query = $"UPDATE Worker SET HourlyPaidGigs = @UpdatedHourlyPaidGigs WHERE Rec_id = @WorkerRecId";
            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UpdatedHourlyPaidGigs", updatedHourlyPaidGigs);
                    command.Parameters.AddWithValue("@WorkerRecId", workerRecId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ShowLeashmoreMessageBox("Hourly Paid Gig Added to Worker Successfully.");
                    }
                    else
                    {
                        ShowErrorMessageBox("Error", "Failed to Add Hourly Paid Gig to Worker.");
                    }
                }
                connection.Close();
            }
        }

        public static string RemoveGigOrPayFromList(string extractedGigsStr, string selectedId)
        {
            if (extractedGigsStr == null || selectedId == null)
            {
                // Handle null strings if needed
                return extractedGigsStr;
            }

            // Split the string into an array using the ',' delimiter
            string[] gigArray = extractedGigsStr.Split(',');

            // Remove the selectedId from the array
            gigArray = Array.FindAll(gigArray, gig => gig != selectedId);

            // Join the array back into a string using the ',' delimiter
            string updatedGigsStr = string.Join(",", gigArray);

            return updatedGigsStr;
        }
        public static void UpdateColumnValue(string tableName, string recordId, string columnName, string newValue)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                {
                    connection.Open();

                    // Construct the SQL command
                    string query = $"UPDATE {tableName} SET {columnName} = @NewValue WHERE Rec_id = @RecordId";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@NewValue", newValue);
                        command.Parameters.AddWithValue("@RecordId", recordId);

                        // Execute the update query
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ShowLeashmoreMessageBox("Record Updated Successfully");
                        }
                        else
                        {
                            ShowErrorMessageBox("Failed to Update Record", "Please, Try Again");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessageBox("Something went wrong with the database", $"Error: {ex.Message}");
            }
        }

        private void button6_Click(object sender, EventArgs e) // Remove Gig from Worker
        {
            if (software_unlocked())
            {
                if (selected_row != -1)
                {
                    gig_type = 0;
                    DialogResult gigTypeResult = GigTypeMessageBox.Show();

                    if (gigTypeResult == DialogResult.Yes)
                    {
                        if (gig_type == 0)
                            ShowErrorMessageBox("Gig Type isn't Selected", "Please, Try Again");
                        if (gig_type == 1)
                        {
                            string extracted_gigs_str = Extract_value_from_record("Worker", Worker_ids[selected_row], "ResultPaidGigs");
                            if (!string.IsNullOrEmpty(extracted_gigs_str) && extracted_gigs_str.Length > 15) // ResultPaidGigs isn't Null and has more than 15 chars
                            {
                                string[] GigsArray = extracted_gigs_str.Split(',');
                                int arrayLength = GigsArray.Length;
                                string[] NamesArray = new string[arrayLength - 1];
                                for (int i = 0; i < arrayLength - 1; i++)
                                {
                                    NamesArray[i] = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("ResultPaidGig", GigsArray[i], "Title"));
                                }
                                ChooseGigPayForRemoval selectRecordForm = new ChooseGigPayForRemoval(GigsArray, NamesArray, "Choose a Gig to Remove from Worker");
                                DialogResult result = selectRecordForm.ShowDialog();

                                if (result == DialogResult.Continue)
                                {
                                    UpdateColumnValue("Worker", Worker_ids[selected_row], "ResultPaidGigs", RemoveGigOrPayFromList(extracted_gigs_str, selected_id));
                                }

                                if (result == DialogResult.Cancel)
                                {
                                    ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                                }

                                if (result == DialogResult.Abort)
                                {
                                    ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                                }

                            }
                            else
                            {
                                ShowErrorMessageBox("Can't Do That", "Worker Doesn't Have Any Gigs Yet.");
                            }
                        }
                        if (gig_type == 2)
                        {
                            string extracted_gigs_str = Extract_value_from_record("Worker", Worker_ids[selected_row], "HourlyPaidGigs");
                            if (!string.IsNullOrEmpty(extracted_gigs_str) && extracted_gigs_str.Length > 15) // ResultPaidGigs isn't Null and has more than 15 chars
                            {
                                string[] GigsArray = extracted_gigs_str.Split(',');
                                int arrayLength = GigsArray.Length;
                                string[] NamesArray = new string[arrayLength - 1];
                                for (int i = 0; i < arrayLength - 1; i++)
                                {
                                    NamesArray[i] = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("HourlyPaidGig", GigsArray[i], "Title"));
                                }
                                ChooseGigPayForRemoval selectRecordForm = new ChooseGigPayForRemoval(GigsArray, NamesArray, "Choose a Gig to Remove from Worker");
                                DialogResult result = selectRecordForm.ShowDialog();

                                if (result == DialogResult.Continue)
                                {
                                    UpdateColumnValue("Worker", Worker_ids[selected_row], "HourlyPaidGigs", RemoveGigOrPayFromList(extracted_gigs_str, selected_id));
                                }

                                if (result == DialogResult.Cancel)
                                {
                                    ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                                }

                                if (result == DialogResult.Abort)
                                {
                                    ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                                }

                            }
                            else
                            {
                                ShowErrorMessageBox("Can't Do That", "Worker Doesn't Have Any Gigs Yet.");
                            }
                        }
                    }
                    else if (gigTypeResult == DialogResult.Cancel)
                    {
                        ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                    }
                }
                else
                    ShowErrorMessageBox("Record Selection Required", "Please select a worker record first.");
            }
        }

        private void fill_labels_with_basic_worker_info()
        {
            if (selected_row == -1)
            {
                loading_label.Text = "No Record is Selected";
                worker_name_label.Text = "";
                total_earned_label.Text = "";
                total_received_label.Text = "";
                entitled_to_label.Text = "";
            }
            else
            {
                loading_label.Text = "Loading Worker Info";
                worker_name_label.Text = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Name"));
                double total_earned = calculate_total_earned_by_worker();
                total_earned_label.Text = currency + String.Format("{0:0.00}", total_earned);

                double total_received = calculate_total_received_by_worker();
                total_received_label.Text = currency + String.Format("{0:0.00}", total_received);
                entitled_to_label.Text = currency + String.Format("{0:0.00}", (total_earned - total_received));
                loading_label.Text = "";
            }
        }

        private double calculate_total_received_by_worker()
        {
            double total_received = 0;
            string extracted_pays_str = Extract_value_from_record("Worker", Worker_ids[selected_row], "Payments");
            if (!string.IsNullOrEmpty(extracted_pays_str) && extracted_pays_str.Length > 15)
            {
                string[] PaysArray = extracted_pays_str.Split(',');
                int arrayLength = PaysArray.Length;
                for (int i = 0; i < arrayLength - 1; i++)
                {
                    if (double.TryParse(Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Payment", PaysArray[i], "Amount")), out double payed_to_worker))
                    {
                        if (payed_to_worker > 0)
                            total_received += payed_to_worker;
                    }
                }
            }
            return total_received;
        }

        private double calculate_total_earned_by_worker()
        {
            double total_earned = 0;
            string extracted_gigs_str = Extract_value_from_record("Worker", Worker_ids[selected_row], "ResultPaidGigs");
            if (!string.IsNullOrEmpty(extracted_gigs_str) && extracted_gigs_str.Length > 15) // ResultPaidGigs isn't Null and has more than 15 chars
            {
                string[] GigsArray = extracted_gigs_str.Split(',');
                int arrayLength = GigsArray.Length;
                for (int i = 0; i < arrayLength - 1; i++)
                {
                    if (double.TryParse(Decrypt_string_with_aes_in_cbc(Extract_value_from_record("ResultPaidGig", GigsArray[i], "Payment_for_gig")), out double earned_for_gig))
                    {
                        if (earned_for_gig > 0)
                            total_earned += earned_for_gig;
                    }
                }
            }
            extracted_gigs_str = Extract_value_from_record("Worker", Worker_ids[selected_row], "HourlyPaidGigs");
            if (!string.IsNullOrEmpty(extracted_gigs_str) && extracted_gigs_str.Length > 15) // HourlyPaidGigs isn't Null and has more than 15 chars
            {
                string[] GigsArray = extracted_gigs_str.Split(',');
                int arrayLength = GigsArray.Length;
                for (int i = 0; i < arrayLength - 1; i++)
                {
                    if (double.TryParse(Decrypt_string_with_aes_in_cbc(Extract_value_from_record("HourlyPaidGig", GigsArray[i], "Hours")), out double hours) && double.TryParse(Decrypt_string_with_aes_in_cbc(Extract_value_from_record("HourlyPaidGig", GigsArray[i], "Hourly_pay")), out double hourly_pay))
                    {
                        if (hours > 0 && hourly_pay > 0)
                            total_earned += (hours * hourly_pay);
                    }
                }
            }
            return total_earned;
        }

        private void button7_Click(object sender, EventArgs e) // Pay to worker
        {
            if (software_unlocked())
            {
                if (selected_row != -1)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                    {
                        selected_id = "";
                        connection.Open();
                        AddPaymentForm addPaymentForm = new AddPaymentForm(Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Name")), (calculate_total_earned_by_worker() - calculate_total_received_by_worker()));
                        addPaymentForm.ShowDialog();
                        connection.Close();
                        if (selected_id.Length > 10)
                        {
                            string all_extracted_ps = Extract_value_from_record("Worker", Worker_ids[selected_row], "Payments");
                            UpdateColumnValue("Worker", Worker_ids[selected_row], "Payments", all_extracted_ps + selected_id + ",");
                        }
                        else
                        {
                            ShowErrorMessageBox("Attention!", "Worker Didn't Get Paid!");
                        }
                        selected_id = "";
                        DisplayWorkerInformation();
                    }
                }
                else
                    ShowErrorMessageBox("Record Selection Required", "Please select a worker record first.");
            }

        }

        private void button8_Click(object sender, EventArgs e) // View Worker
        {
            if (software_unlocked())
            {
                if (selected_row != -1)
                {
                    string[] labelStrings = new string[15];
                    labelStrings[0] = "Name: " + Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Name"));
                    labelStrings[1] = "Birthdate: " + Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Birthdate"));
                    labelStrings[2] = "ID: " + Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "ID"));
                    labelStrings[3] = "Phone Number: " + Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Phone_number"));
                    labelStrings[4] = "Email: " + Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Email"));
                    if (Extract_value_from_record("Worker", Worker_ids[selected_row], "Specialization") == "-1")
                        labelStrings[5] = "No Specialization";
                    else
                        labelStrings[5] = "Specialization: " + Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Specialization", Extract_value_from_record("Worker", Worker_ids[selected_row], "Specialization"), "Title"));

                    long num_of_res_paid_gigs = 0;
                    double earned_for_res_paid_gigs = 0;
                    string extracted_gigs_str = Extract_value_from_record("Worker", Worker_ids[selected_row], "ResultPaidGigs");
                    if (!string.IsNullOrEmpty(extracted_gigs_str) && extracted_gigs_str.Length > 15) // ResultPaidGigs isn't Null and has more than 15 chars
                    {
                        string[] GigsArray = extracted_gigs_str.Split(',');
                        int arrayLength = GigsArray.Length;
                        num_of_res_paid_gigs = arrayLength - 1;
                        for (int i = 0; i < arrayLength - 1; i++)
                        {
                            if (double.TryParse(Decrypt_string_with_aes_in_cbc(Extract_value_from_record("ResultPaidGig", GigsArray[i], "Payment_for_gig")), out double earned_for_gig))
                            {
                                if (earned_for_gig > 0)
                                    earned_for_res_paid_gigs += earned_for_gig;
                            }
                        }
                    }
                    long num_of_hour_paid_gigs = 0;
                    double earned_for_hour_paid_gigs = 0;
                    extracted_gigs_str = Extract_value_from_record("Worker", Worker_ids[selected_row], "HourlyPaidGigs");
                    if (!string.IsNullOrEmpty(extracted_gigs_str) && extracted_gigs_str.Length > 15) // HourlyPaidGigs isn't Null and has more than 15 chars
                    {
                        string[] GigsArray = extracted_gigs_str.Split(',');
                        int arrayLength = GigsArray.Length;
                        num_of_hour_paid_gigs = arrayLength - 1;
                        for (int i = 0; i < arrayLength - 1; i++)
                        {
                            if (double.TryParse(Decrypt_string_with_aes_in_cbc(Extract_value_from_record("HourlyPaidGig", GigsArray[i], "Hours")), out double hours) && double.TryParse(Decrypt_string_with_aes_in_cbc(Extract_value_from_record("HourlyPaidGig", GigsArray[i], "Hourly_pay")), out double hourly_pay))
                            {
                                if (hours > 0 && hourly_pay > 0)
                                    earned_for_hour_paid_gigs += (hours * hourly_pay);
                            }
                        }
                    }
                    labelStrings[6] = "Number of Result Paid Gigs: " + num_of_res_paid_gigs.ToString();
                    labelStrings[7] = "Number of Hourly Paid Gigs: " + num_of_hour_paid_gigs.ToString();
                    labelStrings[8] = "Earned from Result Paid Gigs: " + currency + String.Format("{0:0.00}", earned_for_res_paid_gigs);
                    labelStrings[9] = "Earned from Hourly Paid Gigs: " + currency + String.Format("{0:0.00}", earned_for_hour_paid_gigs);
                    labelStrings[10] = ""; // Empty Space
                    labelStrings[11] = "Total Earned: " + currency + String.Format("{0:0.00}", earned_for_res_paid_gigs + earned_for_hour_paid_gigs);
                    double total_received = calculate_total_received_by_worker();
                    labelStrings[12] = "Total Received: " + currency + String.Format("{0:0.00}", total_received);
                    labelStrings[13] = "Entitled to: " + currency + String.Format("{0:0.00}", (earned_for_res_paid_gigs + earned_for_hour_paid_gigs) - total_received);
                    labelStrings[14] = ""; // Empty Space


                    using (var form = new ViewWorker(labelStrings))
                    {
                        DialogResult result = form.ShowDialog();
                    }
                }
                else
                    ShowErrorMessageBox("Record Selection Required", "Please select a worker record first.");
            }
        }

        static string[] GetWorkerDetailsByPaymentId(string pId)
        {
            string[] resultArray = new string[2];

            using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
            {
                connection.Open();

                string query = $"SELECT Rec_id, Payments FROM Worker WHERE Payments LIKE @pId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@pId", "%" + pId + "%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            resultArray[0] = reader.GetString(0); // Assuming Rec_id is the first column
                            resultArray[1] = reader.GetString(1); // Assuming Payments is the second column
                        }
                    }
                }
            }

            return resultArray;
        }

        private void deleteToolStripMenuItem1_Click_1(object sender, EventArgs e) // Delete payment
        {
            if (software_unlocked())
            {

                using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                {
                    connection.Open();
                    string[,] extracted_pays = ExtractRecordTitlesAndIDs(connection, "Payment", "Amount");
                    int rowCount = extracted_pays.GetLength(0);
                    for (int i = 0; i < rowCount; i++)
                    {
                        string[] wrkr_data = GetWorkerDetailsByPaymentId(extracted_pays[i, 0]);
                        extracted_pays[i, 1] = currency + extracted_pays[i, 1] + " paid to " + Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", wrkr_data[0], "Name"));
                    }

                    SelectRecordForm selectRecordForm = new SelectRecordForm(extracted_pays, "Delete Payment Record", "Delete");
                    DialogResult result = selectRecordForm.ShowDialog();

                    if (result == DialogResult.Continue)
                    {
                        if (DeleteRecord("Payment", selected_id))
                        {
                            ShowLeashmoreMessageBox("Payment Deleted Successfully\nYou Should See the \"Record Updated Successfully\" Message Next");
                        }
                        string[] wrkr_data = GetWorkerDetailsByPaymentId(selected_id); // Rec_id, Payments
                        UpdateColumnValue("Worker", wrkr_data[0], "Payments", RemoveGigOrPayFromList(wrkr_data[1], selected_id));

                    }

                    if (result == DialogResult.Cancel)
                    {
                        ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                    }

                    if (result == DialogResult.Abort)
                    {
                        ShowErrorMessageBox("No Record is Selected", "Please, Try Again");
                    }

                    connection.Close();
                    selected_id = "";

                }
                DisplayWorkerInformation();
            }
        }

        private void viewAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (software_unlocked())
            {

                using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                {
                    connection.Open();
                    string[,] extracted_pays = ExtractRecordTitlesAndIDs(connection, "Payment", "Amount");
                    int rowCount = extracted_pays.GetLength(0);
                    string[,] pay_to = new string[rowCount, 2];
                    for (int i = 0; i < rowCount; i++)
                    {
                        string[] wrkr_data = GetWorkerDetailsByPaymentId(extracted_pays[i, 0]);
                        pay_to[i, 0] = currency + extracted_pays[i, 1];
                        pay_to[i, 1] = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", wrkr_data[0], "Name"));
                    }
                    ViewAllPayments selectRecordForm = new ViewAllPayments(pay_to);
                    DialogResult result = selectRecordForm.ShowDialog();
                    connection.Close();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) // Edit Worker
        {
            if (software_unlocked())
            {
                if (selected_row != -1)
                {

                    string recId = Worker_ids[selected_row];
                    string name = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Name"));
                    string birthdate = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Birthdate"));
                    string id = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "ID"));
                    string phoneNumber = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Phone_number"));
                    string email = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Email"));
                    string specialization;
                    if (Extract_value_from_record("Worker", Worker_ids[selected_row], "Specialization") == "-1")
                        specialization = "None";
                    else
                        specialization = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Specialization", Extract_value_from_record("Worker", Worker_ids[selected_row], "Specialization"), "Title"));

                    using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                    {
                        connection.Open();
                        EditWorkerForm editForm = new EditWorkerForm(recId, ExtractRecordTitlesAndIDs(connection, "Specialization", "Title"), name, birthdate, id, phoneNumber, email, specialization);
                        DialogResult result = editForm.ShowDialog();


                        connection.Close();
                        selected_id = "";
                        DisplayWorkerInformation();
                    }


                }
                else
                    ShowErrorMessageBox("Record Selection Required", "Please select a worker record first.");
            }
        }

        public static DialogResult confirm_worker_deletion(string line1, string line2)
        {
            Form customMessageBox = new Form
            {
                Text = "Delete Record of \"" + line1 + "\"",
                Size = new Size(640, 242),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = ColorTranslator.FromHtml("#AE0031") // Set background color to "#AE0031"
            };

            Label attl = new Label
            {
                Text = "You are about to delete the following record with all of its payments",
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"), // Set foreground color to "#E4E3DF"
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customMessageBox.Controls.Add(attl);

            // Create label for the first line
            Label label1 = new Label
            {
                Text = "Name: " + line1,
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"), // Set foreground color to "#E4E3DF"
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customMessageBox.Controls.Add(label1);

            // Create label for the second line
            Label label2 = new Label
            {
                Text = line2,
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"), // Set foreground color to "#E4E3DF"
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customMessageBox.Controls.Add(label2);

            Label label3 = new Label
            {
                Text = "This Can't be Undone! Would You Like to Delete that Record?",
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"), // Set foreground color to "#E4E3DF"
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customMessageBox.Controls.Add(label3);

            // Create Yes button
            Button yesButton = new Button
            {
                Text = "Yes, Delete it",
                Size = new Size(90, 48),
                BackColor = ColorTranslator.FromHtml("#AE0031"), // Set background color to "#AE0031"
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"), // Set foreground color to "#E4E3DF"
                DialogResult = DialogResult.Yes,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 3, BorderColor = ColorTranslator.FromHtml("#E4E3DF") }, // Set border properties
                Font = new Font("Segoe UI", 12, FontStyle.Bold) // Set bold font
            };
            customMessageBox.Controls.Add(yesButton);

            // Create No button
            Button noButton = new Button
            {
                Text = "No, Cancel",
                Size = new Size(90, 48),
                BackColor = ColorTranslator.FromHtml("#00844F"), // Set background color to "#00844F"
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"), // Set foreground color to "#E4E3DF"
                DialogResult = DialogResult.No,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 3, BorderColor = ColorTranslator.FromHtml("#E4E3DF") }, // Set border properties
                Font = new Font("Segoe UI", 12, FontStyle.Bold) // Set bold font
            };
            customMessageBox.Controls.Add(noButton);

            attl.Location = new Point((customMessageBox.ClientSize.Width) / 2, +11);
            label1.Location = new Point((customMessageBox.ClientSize.Width) / 2, attl.Bottom + 12);
            label2.Location = new Point((customMessageBox.ClientSize.Width) / 2, label1.Bottom + 8);
            label3.Location = new Point((customMessageBox.ClientSize.Width) / 2, label2.Bottom + 12);

            int buttonMargin = 30;
            int buttonWidth = (customMessageBox.ClientSize.Width - 3 * buttonMargin) / 2;

            yesButton.Size = new Size(buttonWidth, 41);
            noButton.Size = new Size(buttonWidth, 41);

            yesButton.Location = new Point(buttonMargin, label3.Bottom + 20);
            noButton.Location = new Point(yesButton.Right + buttonMargin, label3.Bottom + 20);
            CenterLabelText(attl, customMessageBox);
            CenterLabelText(label1, customMessageBox);
            CenterLabelText(label2, customMessageBox);
            CenterLabelText(label3, customMessageBox);

            // Handle Resize event to adjust positions dynamically
            customMessageBox.Resize += (sender, e) =>
            {
                CenterLabelText(attl, customMessageBox);
                CenterLabelText(label1, customMessageBox);
                CenterLabelText(label2, customMessageBox);
                CenterLabelText(label3, customMessageBox);

                buttonWidth = (customMessageBox.ClientSize.Width - 3 * buttonMargin) / 2;
                yesButton.Size = new Size(buttonWidth, 48);
                noButton.Size = new Size(buttonWidth, 48);

                yesButton.Location = new Point(buttonMargin, label3.Bottom + 20);
                noButton.Location = new Point(yesButton.Right + buttonMargin, label3.Bottom + 20);
            };

            // Show the message box
            DialogResult result = customMessageBox.ShowDialog();

            return result;
        }

        private void button9_Click(object sender, EventArgs e) // Delete Worker
        {
            if (software_unlocked())
            {
                if (selected_row != -1)
                {
                    string recId = Worker_ids[selected_row];
                    string name = Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Name"));
                    double total_earned = calculate_total_earned_by_worker();
                    double total_received = calculate_total_received_by_worker();
                    string entitled_to = "Entitled to: " + currency + String.Format("{0:0.00}", (total_earned - total_received));

                    DialogResult result = confirm_worker_deletion(name, entitled_to);

                    if (result == DialogResult.Yes)
                    {
                        string extracted_pays_str = Extract_value_from_record("Worker", Worker_ids[selected_row], "Payments");
                        if (!string.IsNullOrEmpty(extracted_pays_str) && extracted_pays_str.Length > 15)
                        {
                            string[] PaysArray = extracted_pays_str.Split(',');
                            int arrayLength = PaysArray.Length;
                            for (int i = 0; i < arrayLength - 1; i++)
                            {
                                using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                                {
                                    connection.Open();

                                    using (SQLiteCommand command = new SQLiteCommand(connection))
                                    {
                                        // Construct the DELETE statement
                                        command.CommandText = $"DELETE FROM Payment WHERE Rec_id = @id";
                                        command.Parameters.AddWithValue("@id", PaysArray[i]);

                                        try
                                        {
                                            // Execute the DELETE statement
                                            int rowsAffected = command.ExecuteNonQuery();

                                        }
                                        catch (Exception ex)
                                        {
                                            ShowErrorMessageBox($"Failed to Delete Payment Record with ID: \"{PaysArray[i]}\"", ex.Message);
                                        }
                                    }
                                    connection.Close();
                                }

                            }
                        }

                        using (SQLiteConnection connection = new SQLiteConnection(SQLiteconnectionString))
                        {
                            connection.Open();

                            using (SQLiteCommand command = new SQLiteCommand(connection))
                            {
                                // Construct the DELETE statement
                                command.CommandText = $"DELETE FROM Worker WHERE Rec_id = @id";
                                command.Parameters.AddWithValue("@id", recId);

                                try
                                {
                                    // Execute the DELETE statement
                                    int rowsAffected = command.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        ShowLeashmoreMessageBox("Record Deleted Successfully");
                                    }
                                    else
                                    {
                                        ShowErrorMessageBox("Failed to Delete Record", $"Record \"{recId}\" isn't found");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ShowErrorMessageBox($"Failed to Delete Record \"{recId}\"", ex.Message);
                                }
                            }
                            connection.Close();
                            selected_id = "";
                            DisplayWorkerInformation();
                        }

                    }
                    else if (result == DialogResult.No)
                    {
                        ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                    }
                    else
                    {
                        ShowLeashmoreMessageBox("Operation Was Cancelled By User");
                    }

                }
                else
                    ShowErrorMessageBox("Record Selection Required", "Please select a worker record first.");
            }
        }

        private void exportBasicInfoTocsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long selected_row_back = selected_row;
            int i = 0;
            List<string> stringList = new List<string>();
            foreach (string workerId in Worker_ids)
            {
                selected_row = i;
                double total_earned = calculate_total_earned_by_worker();
                double total_received = calculate_total_received_by_worker();
                stringList.Add(Decrypt_string_with_aes_in_cbc(Extract_value_from_record("Worker", Worker_ids[selected_row], "Name")).Replace(",", ".") + "," + String.Format("{0:0.00}", total_earned) + "," + String.Format("{0:0.00}", total_received) + "," + String.Format("{0:0.00}", total_earned - total_received));
                i++;
            }

            // Ask the user for the file path to save the CSV file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.Title = "Export Basic Info to...";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToCsv(stringList, saveFileDialog.FileName);
                ShowLeashmoreMessageBox($"Data has been successfully exported to {saveFileDialog.FileName}");
            }
            selected_row = selected_row_back;
        }

        private void ExportToCsv(List<string> list, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                string[] column_names = new string[4] { "Worker Name", "Total Earned", "Total Received", "Entitled To" };
                // Write header
                for (int i = 0; i < 4; i++)
                {
                    writer.Write(column_names[i]);
                    if (i < 3)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();
                foreach (string item in list)
                {
                    writer.WriteLine(item);
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about_leashmore();
        }

        private void about_leashmore()
        {
            Form customForm = new Form
            {

                Text = "About Leashmore",
                Size = new Size(640, 430),
                MinimumSize = new Size(640, 430),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = ColorTranslator.FromHtml("#7B08A5")
            };

            Label label = new Label
            {
                Text = "Leashmore is an open-source software distributed under the MIT License.\n" +
                       "You are free to modify and distribute copies of the Leashmore.\n" +
                       "You can use Leashmore in commercial applications.\n\n" +
                       "Leashmore app and its source code can be found on:\n\n" +
                       "SourceForge",
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customForm.Controls.Add(label);

            TextBox textField = new TextBox
            {
                Size = new Size(350, 30),
                Text = "sourceforge.net/p/leashmore",
                Location = new Point((customForm.ClientSize.Width - 200) / 2, label.Bottom + 12),
                Font = new Font("Segoe UI", 14),
                ReadOnly = true,
                BackColor = ColorTranslator.FromHtml("#2C2C2C"),
                ForeColor = ColorTranslator.FromHtml("#E4E3DF")
            };
            customForm.Controls.Add(textField);

            Label label1 = new Label
            {
                Location = new Point((customForm.ClientSize.Width - 200) / 2, textField.Bottom + 15),
                Text = "Github",
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customForm.Controls.Add(label1);

            TextBox textField1 = new TextBox
            {
                Size = new Size(350, 30),
                Text = "github.com/Northstrix/Leashmore",
                Location = new Point((customForm.ClientSize.Width - 200) / 2, label1.Bottom + 6),
                Font = new Font("Segoe UI", 14),
                ReadOnly = true,
                BackColor = ColorTranslator.FromHtml("#2C2C2C"),
                ForeColor = ColorTranslator.FromHtml("#E4E3DF")
            };
            customForm.Controls.Add(textField1);

            Label label2 = new Label
            {
                Location = new Point((customForm.ClientSize.Width - 200) / 2, textField1.Bottom + 20),
                Text = "Copyright " + "\u00a9" + " 2024 Maxim Bortnikov",
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            customForm.Controls.Add(label2);

            Button continueButton = new Button
            {
                Text = "Got It",
                Size = new Size(120, 38),
                Location = new Point((customForm.ClientSize.Width - 200) / 2, label2.Bottom + 30),
                BackColor = ColorTranslator.FromHtml("#4113AA"),
                ForeColor = ColorTranslator.FromHtml("#E4E3DF"),
                DialogResult = DialogResult.Yes,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            customForm.Controls.Add(continueButton);

            CenterLabelText(label, customForm);
            CenterLabelText(label1, customForm);
            CenterLabelText(label2, customForm);
            label.Location = new Point((customForm.ClientSize.Width - label.Width) / 2, + 12);
            label1.Location = new Point((customForm.ClientSize.Width - label1.Width) / 2, textField.Bottom + 15);
            textField.Location = new Point((customForm.ClientSize.Width - textField.Width) / 2, label.Bottom + 10);
            textField1.Location = new Point((customForm.ClientSize.Width - textField.Width) / 2, label1.Bottom + 6);
            label2.Location = new Point((customForm.ClientSize.Width - label2.Width) / 2, textField1.Bottom + 20);
            continueButton.Location = new Point((customForm.ClientSize.Width - continueButton.Width) / 2, label2.Bottom + 20);

            // Handle Resize event to adjust positions dynamically
            customForm.Resize += (sender, e) =>
            {
                CenterLabelText(label, customForm);
                CenterLabelText(label1, customForm);
                CenterLabelText(label2, customForm);
                label.Location = new Point((customForm.ClientSize.Width - label.Width) / 2, + 12);
                label1.Location = new Point((customForm.ClientSize.Width - label1.Width) / 2, textField.Bottom + 15);
                textField.Location = new Point((customForm.ClientSize.Width - textField.Width) / 2, label.Bottom + 10);
                textField1.Location = new Point((customForm.ClientSize.Width - textField.Width) / 2, label1.Bottom + 6);
                label2.Location = new Point((customForm.ClientSize.Width - label2.Width) / 2, textField1.Bottom + 20);
                continueButton.Location = new Point((customForm.ClientSize.Width - continueButton.Width) / 2, label2.Bottom + 20);
            };
            customForm.ShowDialog();
        }
    }
}