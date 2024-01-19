using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Leashmore
{
    public class AddSpecialization : Form
    {
        private const int formWidth = 400;
        private const int formHeight = 308;
        private const int labelHeight = 30;
        private const int textBoxHeight = 30;
        private const int multilineTextBoxHeight = 100;
        private const int buttonHeight = 40;

        public AddSpecialization()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Size = new Size(formWidth, formHeight);
            this.Text = "Add Specialization";
            this.BackColor = ColorTranslator.FromHtml("#041020");

            TableLayoutPanel mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };

            this.Controls.Add(mainTable);

            mainTable.Controls.Add(CreateLabel("Title"));
            mainTable.Controls.Add(CreateTextBox());

            mainTable.Controls.Add(CreateLabel("Description"));
            mainTable.Controls.Add(CreateMultilineTextBox());

            TableLayoutPanel buttonTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };

            Button addButton = CreateButton("Add", "#00BD00", "#202020");
            Button cancelButton = CreateButton("Cancel", "#EC0000", "#eeeeee");

            addButton.Click += (sender, e) => add_record_to_specialization_table(mainTable);

            cancelButton.Click += (sender, e) => this.Close();

            buttonTable.Controls.Add(addButton, 0, 0);
            buttonTable.Controls.Add(cancelButton, 1, 0);

            mainTable.Controls.Add(buttonTable);
            mainTable.SetRowSpan(buttonTable, 2);
            mainTable.SetRow(buttonTable, 6);

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

        private System.Windows.Forms.TextBox CreateTextBox()
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                BackColor = ColorTranslator.FromHtml("#041020"),
                ForeColor = ColorTranslator.FromHtml("#b4E5EE"),
                Dock = DockStyle.Fill,
                Height = textBoxHeight,
                Font = new Font("Arial", 12, FontStyle.Regular)
            };

            return textBox;
        }

        private System.Windows.Forms.TextBox CreateMultilineTextBox()
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                BackColor = ColorTranslator.FromHtml("#041020"),
                ForeColor = ColorTranslator.FromHtml("#b4E5EE"),
                Dock = DockStyle.Fill,
                Multiline = true,
                Height = multilineTextBoxHeight,
                Font = new Font("Arial", 12, FontStyle.Regular)
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

        private void add_record_to_specialization_table(TableLayoutPanel mainTable)
        {
            string title = mainTable.GetControlFromPosition(0, 1).Text;
            string description = mainTable.GetControlFromPosition(0, 3).Text;

            string id = GenerateRandomString(10); // Generate random ID
            while (Form1.CheckIfRecordExists(id,"Specialization") == true) // Check if the record with that ID is already in the database. If true, then keep generating new IDs until DB tells that record with such ID isn't present
                id = GenerateRandomString(10); // If record with the generated ID already exists, then generate new ID and check again

            using (SQLiteConnection connection = new SQLiteConnection(Form1.SQLiteconnectionString))
            {
                connection.Open();

                // Call the function to insert data into the table
                InsertDataIntoTable(connection, id, Form1.Encrypt_string_with_aes_in_cbc(title), Form1.Encrypt_string_with_aes_in_cbc(description));

                connection.Close();
            }
            Form1.ShowLeashmoreMessageBox("Record Added Successfully!");
            this.Close();
        }

    static void InsertDataIntoTable(SQLiteConnection connection, string recId, string title, string description)
    {
        // Define the SQL command for insertion
        string insertSql = "INSERT INTO Specialization (Rec_id, Title, Description) VALUES (@RecId, @Title, @Description);";

        using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
        {
            // Add parameters to the command to prevent SQL injection
            command.Parameters.AddWithValue("@RecId", recId);
            command.Parameters.AddWithValue("@Title", title);
            command.Parameters.AddWithValue("@Description", description);

            // Execute the insertion command
            command.ExecuteNonQuery();
        }
    }

    private string GenerateRandomString(int length)
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
