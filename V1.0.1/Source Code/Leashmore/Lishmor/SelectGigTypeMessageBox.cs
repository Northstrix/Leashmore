using System;
using System.Drawing;
using System.Windows.Forms;

namespace Leashmore
{
    public static class GigTypeMessageBox
    {
        public static DialogResult Show()
        {
            Form customMessageBox = new Form
            {
                Text = "Select The Gig Type",
                Size = new Size(320, 200),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(40, 201, 239)
            };

            // Create radio button for Result Paid
            RadioButton resultPaidRadioButton = new RadioButton
            {
                Text = "Result Paid",
                ForeColor = Color.FromArgb(238, 238, 238),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            customMessageBox.Controls.Add(resultPaidRadioButton);

            // Create radio button for Hourly Paid
            RadioButton hourlyPaidRadioButton = new RadioButton
            {
                Text = "Hourly Paid",
                ForeColor = Color.FromArgb(238, 238, 238),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            customMessageBox.Controls.Add(hourlyPaidRadioButton);

            // Create Continue button
            Button continueButton = new Button
            {
                Text = "Continue",
                Size = new Size(100, 38),
                BackColor = Color.FromArgb(32, 32, 32), // "#202020"
                ForeColor = Color.FromArgb(238, 238, 238), // "#EEEEEE"
                DialogResult = DialogResult.Yes,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            customMessageBox.Controls.Add(continueButton);

            // Create Cancel button
            Button cancelButton = new Button
            {
                Text = "Cancel",
                Size = new Size(100, 38),
                BackColor = Color.FromArgb(32, 32, 32), // "#202020"
                ForeColor = Color.FromArgb(238, 238, 238), // "#EEEEEE"
                DialogResult = DialogResult.Cancel,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            customMessageBox.Controls.Add(cancelButton);

            int leftMargin = 20;

            resultPaidRadioButton.Location = new Point(leftMargin, 20);
            hourlyPaidRadioButton.Location = new Point(leftMargin, resultPaidRadioButton.Bottom + 14);

            int buttonMargin = 14;
            int buttonWidth = (customMessageBox.ClientSize.Width - 3 * buttonMargin - leftMargin) / 2;

            continueButton.Size = new Size(buttonWidth, 38);
            cancelButton.Size = new Size(buttonWidth, 38);

            continueButton.Location = new Point(leftMargin, hourlyPaidRadioButton.Bottom + 14);
            cancelButton.Location = new Point(continueButton.Right + buttonMargin, hourlyPaidRadioButton.Bottom + 14);

            // Handle Resize event to adjust positions dynamically
            customMessageBox.Resize += (sender, e) =>
            {
                buttonWidth = (customMessageBox.ClientSize.Width - 3 * buttonMargin - leftMargin) / 2;
                continueButton.Size = new Size(buttonWidth, 38);
                cancelButton.Size = new Size(buttonWidth, 38);

                continueButton.Location = new Point(leftMargin, hourlyPaidRadioButton.Bottom + 14);
                cancelButton.Location = new Point(continueButton.Right + buttonMargin, hourlyPaidRadioButton.Bottom + 14);
            };

            // Handle Click event for radio buttons to store the selected gig type
            resultPaidRadioButton.CheckedChanged += (sender, e) =>
            {
                if (resultPaidRadioButton.Checked)
                {
                    Form1.gig_type = 1;
                }
            };

            hourlyPaidRadioButton.CheckedChanged += (sender, e) =>
            {
                if (hourlyPaidRadioButton.Checked)
                {
                    Form1.gig_type = 2;
                }
            };

            // Show the message box
            DialogResult result = customMessageBox.ShowDialog();

            return result;
        }
    }
}