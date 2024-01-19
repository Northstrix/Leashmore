namespace Leashmore
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new Label();
            button1 = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            label3 = new Label();
            label2 = new Label();
            label4 = new Label();
            label5 = new Label();
            label8 = new Label();
            loading_label = new Label();
            worker_name_label = new Label();
            total_earned_label = new Label();
            total_received_label = new Label();
            entitled_to_label = new Label();
            tableLayoutPanel5 = new TableLayoutPanel();
            label9 = new Label();
            sodtware_status_label = new Label();
            button3 = new Button();
            tableLayoutPanel6 = new TableLayoutPanel();
            button2 = new Button();
            button9 = new Button();
            button8 = new Button();
            button7 = new Button();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            dataGridView1 = new DataGridView();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exportBasicInfoTocsvToolStripMenuItem = new ToolStripMenuItem();
            quitToolStripMenuItem = new ToolStripMenuItem();
            gigToolStripMenuItem = new ToolStripMenuItem();
            addToolStripMenuItem1 = new ToolStripMenuItem();
            editToolStripMenuItem1 = new ToolStripMenuItem();
            viewToolStripMenuItem1 = new ToolStripMenuItem();
            viewToolStripMenuItem2 = new ToolStripMenuItem();
            specializationToolStripMenuItem = new ToolStripMenuItem();
            addToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            paymentToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem1 = new ToolStripMenuItem();
            viewAllToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel4 = new TableLayoutPanel();
            label6 = new Label();
            label7 = new Label();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            menuStrip1.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 2F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 3.5F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 1, 5);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel5, 3, 5);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel6, 3, 3);
            tableLayoutPanel1.Controls.Add(dataGridView1, 1, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 24);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 3.5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 93F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 14F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 174F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 3.5F));
            tableLayoutPanel1.Size = new Size(1073, 705);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(button1, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(40, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(745, 39);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(238, 238, 238);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(629, 39);
            label1.TabIndex = 0;
            label1.Text = "Worker Records:";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(203, 128, 78);
            button1.Dock = DockStyle.Fill;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = Color.FromArgb(243, 248, 219);
            button1.Location = new Point(638, 3);
            button1.Name = "button1";
            button1.Size = new Size(104, 33);
            button1.TabIndex = 2;
            button1.Text = "Refresh List";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = SystemColors.AppWorkspace;
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(label3, 0, 3);
            tableLayoutPanel3.Controls.Add(label2, 0, 1);
            tableLayoutPanel3.Controls.Add(label4, 0, 5);
            tableLayoutPanel3.Controls.Add(label5, 0, 7);
            tableLayoutPanel3.Controls.Add(label8, 0, 9);
            tableLayoutPanel3.Controls.Add(loading_label, 1, 1);
            tableLayoutPanel3.Controls.Add(worker_name_label, 1, 3);
            tableLayoutPanel3.Controls.Add(total_earned_label, 1, 5);
            tableLayoutPanel3.Controls.Add(total_received_label, 1, 7);
            tableLayoutPanel3.Controls.Add(entitled_to_label, 1, 9);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(40, 517);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 11;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 3F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 3F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 3F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 3F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 3F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 3F));
            tableLayoutPanel3.Size = new Size(745, 168);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(22, 22, 22);
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.FromArgb(238, 238, 238);
            label3.Location = new Point(3, 36);
            label3.Name = "label3";
            label3.Size = new Size(194, 30);
            label3.TabIndex = 1;
            label3.Text = "Worker Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(22, 22, 22);
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(238, 238, 238);
            label2.Location = new Point(3, 3);
            label2.Name = "label2";
            label2.Size = new Size(194, 30);
            label2.TabIndex = 0;
            label2.Text = "Basic Info";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(22, 22, 22);
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label4.ForeColor = Color.FromArgb(238, 238, 238);
            label4.Location = new Point(3, 69);
            label4.Name = "label4";
            label4.Size = new Size(194, 30);
            label4.TabIndex = 2;
            label4.Text = "Total Earned";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.FromArgb(22, 22, 22);
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label5.ForeColor = Color.FromArgb(238, 238, 238);
            label5.Location = new Point(3, 102);
            label5.Name = "label5";
            label5.Size = new Size(194, 30);
            label5.TabIndex = 3;
            label5.Text = "Total Received";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.FromArgb(22, 22, 22);
            label8.Dock = DockStyle.Fill;
            label8.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label8.ForeColor = Color.FromArgb(238, 238, 238);
            label8.Location = new Point(3, 135);
            label8.Name = "label8";
            label8.Size = new Size(194, 30);
            label8.TabIndex = 4;
            label8.Text = "Entitled To";
            // 
            // loading_label
            // 
            loading_label.AutoSize = true;
            loading_label.BackColor = Color.FromArgb(22, 22, 22);
            loading_label.Dock = DockStyle.Fill;
            loading_label.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            loading_label.ForeColor = Color.FromArgb(238, 238, 238);
            loading_label.Location = new Point(203, 3);
            loading_label.Name = "loading_label";
            loading_label.Size = new Size(539, 30);
            loading_label.TabIndex = 5;
            // 
            // worker_name_label
            // 
            worker_name_label.AutoSize = true;
            worker_name_label.BackColor = Color.FromArgb(22, 22, 22);
            worker_name_label.Dock = DockStyle.Fill;
            worker_name_label.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            worker_name_label.ForeColor = Color.FromArgb(238, 238, 238);
            worker_name_label.Location = new Point(203, 36);
            worker_name_label.Name = "worker_name_label";
            worker_name_label.Size = new Size(539, 30);
            worker_name_label.TabIndex = 6;
            // 
            // total_earned_label
            // 
            total_earned_label.AutoSize = true;
            total_earned_label.BackColor = Color.FromArgb(22, 22, 22);
            total_earned_label.Dock = DockStyle.Fill;
            total_earned_label.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            total_earned_label.ForeColor = Color.FromArgb(238, 238, 238);
            total_earned_label.Location = new Point(203, 69);
            total_earned_label.Name = "total_earned_label";
            total_earned_label.Size = new Size(539, 30);
            total_earned_label.TabIndex = 7;
            // 
            // total_received_label
            // 
            total_received_label.AutoSize = true;
            total_received_label.BackColor = Color.FromArgb(22, 22, 22);
            total_received_label.Dock = DockStyle.Fill;
            total_received_label.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            total_received_label.ForeColor = Color.FromArgb(238, 238, 238);
            total_received_label.Location = new Point(203, 102);
            total_received_label.Name = "total_received_label";
            total_received_label.Size = new Size(539, 30);
            total_received_label.TabIndex = 8;
            // 
            // entitled_to_label
            // 
            entitled_to_label.AutoSize = true;
            entitled_to_label.BackColor = Color.FromArgb(22, 22, 22);
            entitled_to_label.Dock = DockStyle.Fill;
            entitled_to_label.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            entitled_to_label.ForeColor = Color.FromArgb(238, 238, 238);
            entitled_to_label.Location = new Point(203, 135);
            entitled_to_label.Name = "entitled_to_label";
            entitled_to_label.Size = new Size(539, 30);
            entitled_to_label.TabIndex = 9;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            tableLayoutPanel5.Controls.Add(label9, 0, 0);
            tableLayoutPanel5.Controls.Add(sodtware_status_label, 0, 1);
            tableLayoutPanel5.Controls.Add(button3, 1, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(812, 517);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 3;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Size = new Size(219, 168);
            tableLayoutPanel5.TabIndex = 3;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.FromArgb(22, 22, 22);
            label9.Dock = DockStyle.Fill;
            label9.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label9.ForeColor = Color.FromArgb(238, 238, 238);
            label9.Location = new Point(3, 0);
            label9.Name = "label9";
            label9.Size = new Size(123, 112);
            label9.TabIndex = 10;
            label9.Text = "Software Satus:";
            // 
            // sodtware_status_label
            // 
            sodtware_status_label.AutoSize = true;
            sodtware_status_label.BackColor = Color.FromArgb(22, 22, 22);
            sodtware_status_label.Dock = DockStyle.Fill;
            sodtware_status_label.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            sodtware_status_label.ForeColor = Color.FromArgb(238, 238, 238);
            sodtware_status_label.Location = new Point(3, 112);
            sodtware_status_label.Name = "sodtware_status_label";
            sodtware_status_label.Size = new Size(123, 36);
            sodtware_status_label.TabIndex = 11;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(238, 238, 238);
            button3.Dock = DockStyle.Fill;
            button3.FlatStyle = FlatStyle.Popup;
            button3.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button3.ForeColor = Color.FromArgb(238, 238, 238);
            button3.Location = new Point(132, 115);
            button3.Name = "button3";
            button3.Size = new Size(84, 30);
            button3.TabIndex = 12;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 3;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 11F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 11F));
            tableLayoutPanel6.Controls.Add(button2, 1, 1);
            tableLayoutPanel6.Controls.Add(button9, 1, 13);
            tableLayoutPanel6.Controls.Add(button8, 1, 11);
            tableLayoutPanel6.Controls.Add(button7, 1, 9);
            tableLayoutPanel6.Controls.Add(button6, 1, 7);
            tableLayoutPanel6.Controls.Add(button5, 1, 5);
            tableLayoutPanel6.Controls.Add(button4, 1, 3);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(812, 69);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 15;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutPanel6.Size = new Size(219, 428);
            tableLayoutPanel6.TabIndex = 4;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(69, 153, 127);
            button2.Dock = DockStyle.Fill;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button2.ForeColor = Color.FromArgb(238, 238, 238);
            button2.Location = new Point(14, 8);
            button2.Name = "button2";
            button2.Size = new Size(191, 30);
            button2.TabIndex = 3;
            button2.Text = "Add Worker";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button9
            // 
            button9.BackColor = Color.FromArgb(171, 49, 18);
            button9.Dock = DockStyle.Fill;
            button9.FlatStyle = FlatStyle.Popup;
            button9.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button9.ForeColor = Color.FromArgb(238, 238, 238);
            button9.Location = new Point(14, 299);
            button9.Name = "button9";
            button9.Size = new Size(191, 30);
            button9.TabIndex = 5;
            button9.Text = "Delete Worker";
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // button8
            // 
            button8.BackColor = Color.FromArgb(75, 111, 151);
            button8.Dock = DockStyle.Fill;
            button8.FlatStyle = FlatStyle.Popup;
            button8.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button8.ForeColor = Color.FromArgb(238, 238, 238);
            button8.Location = new Point(14, 213);
            button8.Name = "button8";
            button8.Size = new Size(191, 30);
            button8.TabIndex = 5;
            button8.Text = "View Full Worker Record";
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // button7
            // 
            button7.BackColor = Color.FromArgb(75, 111, 151);
            button7.Dock = DockStyle.Fill;
            button7.FlatStyle = FlatStyle.Popup;
            button7.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button7.ForeColor = Color.FromArgb(238, 238, 238);
            button7.Location = new Point(14, 172);
            button7.Name = "button7";
            button7.Size = new Size(191, 30);
            button7.TabIndex = 5;
            button7.Text = "Pay to Worker";
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // button6
            // 
            button6.BackColor = Color.FromArgb(232, 149, 104);
            button6.Dock = DockStyle.Fill;
            button6.FlatStyle = FlatStyle.Popup;
            button6.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button6.ForeColor = Color.FromArgb(32, 32, 32);
            button6.Location = new Point(14, 131);
            button6.Name = "button6";
            button6.Size = new Size(191, 30);
            button6.TabIndex = 5;
            button6.Text = "Remove Worker's Gig";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.FromArgb(232, 181, 104);
            button5.Dock = DockStyle.Fill;
            button5.FlatStyle = FlatStyle.Popup;
            button5.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button5.ForeColor = Color.FromArgb(32, 32, 32);
            button5.Location = new Point(14, 90);
            button5.Name = "button5";
            button5.Size = new Size(191, 30);
            button5.TabIndex = 5;
            button5.Text = "Edit Worker Record";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(69, 153, 127);
            button4.Dock = DockStyle.Fill;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button4.ForeColor = Color.FromArgb(238, 238, 238);
            button4.Location = new Point(14, 49);
            button4.Name = "button4";
            button4.Size = new Size(191, 30);
            button4.TabIndex = 5;
            button4.Text = "Add Gig to Worker";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.FromArgb(16, 16, 16);
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.GridColor = Color.FromArgb(237, 137, 40);
            dataGridView1.Location = new Point(40, 69);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(745, 428);
            dataGridView1.TabIndex = 5;
            dataGridView1.CurrentCellChanged += dataGridView1_CurrentCellChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, gigToolStripMenuItem, specializationToolStripMenuItem, paymentToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1073, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exportBasicInfoTocsvToolStripMenuItem, quitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // exportBasicInfoTocsvToolStripMenuItem
            // 
            exportBasicInfoTocsvToolStripMenuItem.Name = "exportBasicInfoTocsvToolStripMenuItem";
            exportBasicInfoTocsvToolStripMenuItem.Size = new Size(199, 22);
            exportBasicInfoTocsvToolStripMenuItem.Text = "Export Basic Info to .csv";
            exportBasicInfoTocsvToolStripMenuItem.Click += exportBasicInfoTocsvToolStripMenuItem_Click;
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new Size(199, 22);
            quitToolStripMenuItem.Text = "Quit";
            quitToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // gigToolStripMenuItem
            // 
            gigToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addToolStripMenuItem1, editToolStripMenuItem1, viewToolStripMenuItem1, viewToolStripMenuItem2 });
            gigToolStripMenuItem.Name = "gigToolStripMenuItem";
            gigToolStripMenuItem.Size = new Size(37, 20);
            gigToolStripMenuItem.Text = "Gig";
            // 
            // addToolStripMenuItem1
            // 
            addToolStripMenuItem1.Name = "addToolStripMenuItem1";
            addToolStripMenuItem1.Size = new Size(107, 22);
            addToolStripMenuItem1.Text = "Add";
            addToolStripMenuItem1.Click += addToolStripMenuItem1_Click;
            // 
            // editToolStripMenuItem1
            // 
            editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            editToolStripMenuItem1.Size = new Size(107, 22);
            editToolStripMenuItem1.Text = "Edit";
            editToolStripMenuItem1.Click += editToolStripMenuItem1_Click;
            // 
            // viewToolStripMenuItem1
            // 
            viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            viewToolStripMenuItem1.Size = new Size(107, 22);
            viewToolStripMenuItem1.Text = "Delete";
            viewToolStripMenuItem1.Click += deleteToolStripMenuItem1_Click;
            // 
            // viewToolStripMenuItem2
            // 
            viewToolStripMenuItem2.Name = "viewToolStripMenuItem2";
            viewToolStripMenuItem2.Size = new Size(107, 22);
            viewToolStripMenuItem2.Text = "View";
            viewToolStripMenuItem2.Click += viewToolStripMenuItem2_Click;
            // 
            // specializationToolStripMenuItem
            // 
            specializationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addToolStripMenuItem, editToolStripMenuItem, deleteToolStripMenuItem, viewToolStripMenuItem });
            specializationToolStripMenuItem.Name = "specializationToolStripMenuItem";
            specializationToolStripMenuItem.Size = new Size(91, 20);
            specializationToolStripMenuItem.Text = "Specialization";
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new Size(107, 22);
            addToolStripMenuItem.Text = "Add";
            addToolStripMenuItem.Click += addToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(107, 22);
            editToolStripMenuItem.Text = "Edit";
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(107, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(107, 22);
            viewToolStripMenuItem.Text = "View";
            viewToolStripMenuItem.Click += viewToolStripMenuItem_Click;
            // 
            // paymentToolStripMenuItem
            // 
            paymentToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { deleteToolStripMenuItem1, viewAllToolStripMenuItem });
            paymentToolStripMenuItem.Name = "paymentToolStripMenuItem";
            paymentToolStripMenuItem.Size = new Size(66, 20);
            paymentToolStripMenuItem.Text = "Payment";
            // 
            // deleteToolStripMenuItem1
            // 
            deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            deleteToolStripMenuItem1.Size = new Size(116, 22);
            deleteToolStripMenuItem1.Text = "Delete";
            deleteToolStripMenuItem1.Click += deleteToolStripMenuItem1_Click_1;
            // 
            // viewAllToolStripMenuItem
            // 
            viewAllToolStripMenuItem.Name = "viewAllToolStripMenuItem";
            viewAllToolStripMenuItem.Size = new Size(116, 22);
            viewAllToolStripMenuItem.Text = "View All";
            viewAllToolStripMenuItem.Click += viewAllToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(52, 20);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = Color.FromArgb(238, 238, 238);
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(label6, 0, 3);
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 4;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(200, 100);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(22, 22, 22);
            label6.Dock = DockStyle.Fill;
            label6.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label6.ForeColor = Color.FromArgb(238, 238, 238);
            label6.Location = new Point(3, 60);
            label6.Name = "label6";
            label6.Size = new Size(194, 40);
            label6.TabIndex = 1;
            label6.Text = "Worker Name";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(22, 22, 22);
            label7.Dock = DockStyle.Fill;
            label7.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label7.ForeColor = Color.FromArgb(238, 238, 238);
            label7.Location = new Point(3, 0);
            label7.Name = "label7";
            label7.Size = new Size(194, 30);
            label7.TabIndex = 0;
            label7.Text = "Basic Info";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(22, 22, 22);
            ClientSize = new Size(1073, 729);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Leashmore";
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private MenuStrip menuStrip1;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button button1;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TableLayoutPanel tableLayoutPanel4;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label loading_label;
        private Label worker_name_label;
        private Label total_earned_label;
        private Label total_received_label;
        private Label entitled_to_label;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem gigToolStripMenuItem;
        private ToolStripMenuItem specializationToolStripMenuItem;
        private ToolStripMenuItem paymentToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel5;
        private Label label9;
        private Button button2;
        private Label sodtware_status_label;
        private TableLayoutPanel tableLayoutPanel6;
        private Button button3;
        private Button button9;
        private Button button8;
        private Button button7;
        private Button button6;
        private Button button5;
        private Button button4;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private ToolStripMenuItem viewAllToolStripMenuItem;
        private DataGridView dataGridView1;
        private ToolStripMenuItem addToolStripMenuItem1;
        private ToolStripMenuItem editToolStripMenuItem1;
        private ToolStripMenuItem viewToolStripMenuItem1;
        private ToolStripMenuItem viewToolStripMenuItem2;
        private ToolStripMenuItem exportBasicInfoTocsvToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
    }
}