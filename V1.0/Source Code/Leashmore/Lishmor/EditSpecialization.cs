using System.Data.SQLite;

namespace Leashmore
{
    public class EditSpecialization : Form
    {
        private const int formWidth = 400;
        private const int formHeight = 308;
        private const int labelHeight = 30;
        private const int textBoxHeight = 30;
        private const int multilineTextBoxHeight = 100;
        private const int buttonHeight = 40;
        private string id;

        public EditSpecialization(string[] initialValues, string obtained_id)
        {
            id = obtained_id;
            InitializeForm(initialValues);
        }

        private void InitializeForm(string[] initialValues)
        {
            this.Size = new Size(formWidth, formHeight);
            this.Text = "Edit Specialization";
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
            mainTable.Controls.Add(CreateTextBox(initialValues.Length > 0 ? initialValues[0] : ""));

            mainTable.Controls.Add(CreateLabel("Description"));
            mainTable.Controls.Add(CreateMultilineTextBox(initialValues.Length > 1 ? initialValues[1] : ""));

            TableLayoutPanel buttonTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };

            Button EditButton = CreateButton("Edit", "#00BD00", "#202020");
            Button cancelButton = CreateButton("Cancel", "#EC0000", "#eeeeee");

            EditButton.Click += (sender, e) => Edit_record_in_specialization_table(mainTable);

            cancelButton.Click += (sender, e) => this.Close();

            buttonTable.Controls.Add(EditButton, 0, 0);
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

        private System.Windows.Forms.TextBox CreateTextBox(string text)
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                BackColor = ColorTranslator.FromHtml("#041020"),
                ForeColor = ColorTranslator.FromHtml("#b4E5EE"),
                Dock = DockStyle.Fill,
                Height = textBoxHeight,
                Font = new Font("Arial", 12, FontStyle.Regular),
                Text = text
            };

            return textBox;
        }

        private System.Windows.Forms.TextBox CreateMultilineTextBox(string text)
        {
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
            {
                BackColor = ColorTranslator.FromHtml("#041020"),
                ForeColor = ColorTranslator.FromHtml("#b4E5EE"),
                Dock = DockStyle.Fill,
                Multiline = true,
                Height = multilineTextBoxHeight,
                Font = new Font("Arial", 12, FontStyle.Regular),
                Text = text
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

        private void Edit_record_in_specialization_table(TableLayoutPanel mainTable)
        {
            UpdateSpecializationRecord(mainTable.GetControlFromPosition(0, 1).Text, mainTable.GetControlFromPosition(0, 3).Text);
            this.Close();
        }

        public void UpdateSpecializationRecord(string newTitle, string newDescription)
        {
            using (SQLiteConnection connection = new SQLiteConnection(Form1.SQLiteconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Construct the UPDATE statement
                    command.CommandText = "UPDATE Specialization SET Title = @title, Description = @description WHERE Rec_id = @id";
                    command.Parameters.AddWithValue("@title", Form1.Encrypt_string_with_aes_in_cbc(newTitle));
                    command.Parameters.AddWithValue("@description", Form1.Encrypt_string_with_aes_in_cbc(newDescription));
                    command.Parameters.AddWithValue("@id", id);
                    try
                    {
                        // Execute the UPDATE statement
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Form1.ShowLeashmoreMessageBox($"Record Updated Successfully.");
                        }
                        else
                        {
                            Form1.ShowErrorMessageBox("Failed to Edit Record", $"Record \"{id}\" isn't found");
                        }
                    }
                    catch (Exception ex)
                    {
                        Form1.ShowErrorMessageBox($"Failed to Edit Record \"{id}\"", ex.Message);
                    }
                }
            }
        }

    }
}