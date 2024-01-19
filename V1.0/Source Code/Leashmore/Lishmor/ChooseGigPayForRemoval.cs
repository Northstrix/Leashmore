using Leashmore;
using System;
using System.Drawing;
using System.Windows.Forms;

public class ChooseGigPayForRemoval : Form
{
    private TableLayoutPanel tableLayoutPanel1;
    private ComboBox comboBox;
    private Button removeButton;
    private Button cancelButton;
    private string[] idsArray;
    private string[] namesArray;

    public ChooseGigPayForRemoval(string[] ids, string[] names, string formTitle)
    {
        idsArray = ids;
        namesArray = names;
        InitializeForm(formTitle);
        InitializeControls();
    }

    private void InitializeForm(string formTitle)
    {
        Text = formTitle;
        BackColor = Color.FromArgb(0xEF, 0x7A, 0x0F); // Use the color scheme from ChooseGigPayForRemoval

        FormBorderStyle = FormBorderStyle.Sizable;
        MinimumSize = new Size(320, 120);
        Size = new Size(480, 120);
        StartPosition = FormStartPosition.CenterScreen;
    }

    private void InitializeControls()
    {
        tableLayoutPanel1 = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 3,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
        };

        comboBox = new ComboBox
        {
            BackColor = Color.FromArgb(0x18, 0x18, 0x18), // Set the background color to 181818
            ForeColor = Color.FromArgb(0xEE, 0xEE, 0xEE), // Set the foreground color to EEEEEE
            Dock = DockStyle.Fill,
            Height = 30,
            Font = new Font("Arial", 12, FontStyle.Regular),
            FlatStyle = FlatStyle.Flat,
        };

        // Populate ComboBox with the names array
        comboBox.Items.AddRange(namesArray);

        removeButton = CreateButton("Remove", "#D31B35", "#EEEEEE"); // Use the color scheme from ChooseGigPayForRemoval
        removeButton.Click += (sender, e) => RemoveButtonClick();

        cancelButton = CreateButton("Cancel", "#3FBC18", "#EEEEEE"); // Use the color scheme from ChooseGigPayForRemoval
        cancelButton.Click += (sender, e) => CancelButtonClick();

        tableLayoutPanel1.Controls.Add(comboBox, 0, 0);
        tableLayoutPanel1.SetColumnSpan(comboBox, 2);
        tableLayoutPanel1.Controls.Add(removeButton, 0, 1);
        tableLayoutPanel1.Controls.Add(cancelButton, 1, 1);

        Controls.Add(tableLayoutPanel1);

        Resize += (sender, e) =>
        {
            tableLayoutPanel1.Width = ClientRectangle.Width;
            tableLayoutPanel1.Height = ClientRectangle.Height;
        };
    }

    private Button CreateButton(string buttonText, string backColor, string foreColor)
    {
        Button button = new Button()
        {
            Text = buttonText,
            BackColor = ColorTranslator.FromHtml(backColor),
            ForeColor = ColorTranslator.FromHtml(foreColor),
            Font = new Font("Arial", 12, FontStyle.Bold),
            Height = 40,
            Width = 90,
            FlatStyle = FlatStyle.Flat,
        };

        return button;
    }

    private void RemoveButtonClick()
    {
        if (comboBox.SelectedIndex != -1 && comboBox.SelectedIndex < idsArray.Length)
        {
            Form1.selected_id = idsArray[comboBox.SelectedIndex];
        }

        DialogResult = DialogResult.Continue;
        Close();
    }

    private void CancelButtonClick()
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}