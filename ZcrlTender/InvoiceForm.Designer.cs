namespace ZcrlTender
{
    partial class InvoiceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.numberTextBox = new System.Windows.Forms.TextBox();
            this.invoiceDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.contractorsCBList = new System.Windows.Forms.ComboBox();
            this.contractsCBList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.contractRemainLoadingPicture = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.newStatusRButton = new System.Windows.Forms.RadioButton();
            this.onPayStatusRButton = new System.Windows.Forms.RadioButton();
            this.balanceChangesTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.invoiceFullSum = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.moneyRemainsLoadingPicture = new System.Windows.Forms.PictureBox();
            this.paidStatusRButton = new System.Windows.Forms.RadioButton();
            this.IsCreditCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.estimateNameLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.filesTable = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.updateContractsListWorker = new System.ComponentModel.BackgroundWorker();
            this.updateMoneyRemainsWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.contractRemainLoadingPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.balanceChangesTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceFullSum)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moneyRemainsLoadingPicture)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filesTable)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер:";
            // 
            // numberTextBox
            // 
            this.numberTextBox.Location = new System.Drawing.Point(96, 23);
            this.numberTextBox.Name = "numberTextBox";
            this.numberTextBox.Size = new System.Drawing.Size(138, 20);
            this.numberTextBox.TabIndex = 1;
            // 
            // invoiceDate
            // 
            this.invoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.invoiceDate.Location = new System.Drawing.Point(378, 23);
            this.invoiceDate.Name = "invoiceDate";
            this.invoiceDate.Size = new System.Drawing.Size(102, 20);
            this.invoiceDate.TabIndex = 2;
            this.invoiceDate.ValueChanged += new System.EventHandler(this.invoiceDate_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(334, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Дата:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Контрагент:";
            // 
            // contractorsCBList
            // 
            this.contractorsCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.contractorsCBList.FormattingEnabled = true;
            this.contractorsCBList.Location = new System.Drawing.Point(96, 108);
            this.contractorsCBList.Name = "contractorsCBList";
            this.contractorsCBList.Size = new System.Drawing.Size(384, 21);
            this.contractorsCBList.TabIndex = 5;
            this.contractorsCBList.SelectedIndexChanged += new System.EventHandler(this.contractorsCBList_SelectedIndexChanged);
            // 
            // contractsCBList
            // 
            this.contractsCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.contractsCBList.FormattingEnabled = true;
            this.contractsCBList.Location = new System.Drawing.Point(96, 145);
            this.contractsCBList.Name = "contractsCBList";
            this.contractsCBList.Size = new System.Drawing.Size(192, 21);
            this.contractsCBList.TabIndex = 7;
            this.contractsCBList.SelectedIndexChanged += new System.EventHandler(this.contractsCBList_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Договір:";
            // 
            // contractRemainLoadingPicture
            // 
            this.contractRemainLoadingPicture.Image = global::ZcrlTender.Properties.Resources.loader_sm;
            this.contractRemainLoadingPicture.Location = new System.Drawing.Point(294, 144);
            this.contractRemainLoadingPicture.Name = "contractRemainLoadingPicture";
            this.contractRemainLoadingPicture.Size = new System.Drawing.Size(22, 23);
            this.contractRemainLoadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.contractRemainLoadingPicture.TabIndex = 14;
            this.contractRemainLoadingPicture.TabStop = false;
            this.contractRemainLoadingPicture.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.Color.Green;
            this.label6.Location = new System.Drawing.Point(291, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(186, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "доступно: 102 102 102,65 грн.";
            // 
            // newStatusRButton
            // 
            this.newStatusRButton.AutoSize = true;
            this.newStatusRButton.Checked = true;
            this.newStatusRButton.Location = new System.Drawing.Point(14, 19);
            this.newStatusRButton.Name = "newStatusRButton";
            this.newStatusRButton.Size = new System.Drawing.Size(100, 17);
            this.newStatusRButton.TabIndex = 15;
            this.newStatusRButton.TabStop = true;
            this.newStatusRButton.Text = "Новий рахунок";
            this.newStatusRButton.UseVisualStyleBackColor = true;
            this.newStatusRButton.CheckedChanged += new System.EventHandler(this.newStatusRButton_CheckedChanged);
            // 
            // onPayStatusRButton
            // 
            this.onPayStatusRButton.AutoSize = true;
            this.onPayStatusRButton.Location = new System.Drawing.Point(14, 42);
            this.onPayStatusRButton.Name = "onPayStatusRButton";
            this.onPayStatusRButton.Size = new System.Drawing.Size(127, 17);
            this.onPayStatusRButton.TabIndex = 16;
            this.onPayStatusRButton.Text = "Передано на оплату";
            this.onPayStatusRButton.UseVisualStyleBackColor = true;
            this.onPayStatusRButton.CheckedChanged += new System.EventHandler(this.newStatusRButton_CheckedChanged);
            // 
            // balanceChangesTable
            // 
            this.balanceChangesTable.AllowUserToAddRows = false;
            this.balanceChangesTable.AllowUserToDeleteRows = false;
            this.balanceChangesTable.AllowUserToResizeColumns = false;
            this.balanceChangesTable.AllowUserToResizeRows = false;
            this.balanceChangesTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.balanceChangesTable.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.balanceChangesTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.balanceChangesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.balanceChangesTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.balanceChangesTable.Enabled = false;
            this.balanceChangesTable.Location = new System.Drawing.Point(14, 65);
            this.balanceChangesTable.MultiSelect = false;
            this.balanceChangesTable.Name = "balanceChangesTable";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.balanceChangesTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.balanceChangesTable.RowHeadersWidth = 200;
            this.balanceChangesTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.balanceChangesTable.Size = new System.Drawing.Size(466, 148);
            this.balanceChangesTable.TabIndex = 17;
            this.balanceChangesTable.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.balanceChangesTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "Взяти на оплату";
            this.Column1.Name = "Column1";
            this.Column1.Width = 120;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "Доступно коштів";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Сума:";
            // 
            // invoiceFullSum
            // 
            this.invoiceFullSum.DecimalPlaces = 2;
            this.invoiceFullSum.Location = new System.Drawing.Point(96, 209);
            this.invoiceFullSum.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.invoiceFullSum.Name = "invoiceFullSum";
            this.invoiceFullSum.Size = new System.Drawing.Size(112, 20);
            this.invoiceFullSum.TabIndex = 19;
            this.invoiceFullSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.invoiceFullSum.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.moneyRemainsLoadingPicture);
            this.groupBox1.Controls.Add(this.paidStatusRButton);
            this.groupBox1.Controls.Add(this.balanceChangesTable);
            this.groupBox1.Controls.Add(this.newStatusRButton);
            this.groupBox1.Controls.Add(this.onPayStatusRButton);
            this.groupBox1.Location = new System.Drawing.Point(8, 256);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(499, 223);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Статус рахунку";
            // 
            // moneyRemainsLoadingPicture
            // 
            this.moneyRemainsLoadingPicture.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.moneyRemainsLoadingPicture.Image = global::ZcrlTender.Properties.Resources.loader;
            this.moneyRemainsLoadingPicture.InitialImage = null;
            this.moneyRemainsLoadingPicture.Location = new System.Drawing.Point(185, 110);
            this.moneyRemainsLoadingPicture.Name = "moneyRemainsLoadingPicture";
            this.moneyRemainsLoadingPicture.Size = new System.Drawing.Size(103, 80);
            this.moneyRemainsLoadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.moneyRemainsLoadingPicture.TabIndex = 19;
            this.moneyRemainsLoadingPicture.TabStop = false;
            this.moneyRemainsLoadingPicture.Visible = false;
            // 
            // paidStatusRButton
            // 
            this.paidStatusRButton.AutoSize = true;
            this.paidStatusRButton.Location = new System.Drawing.Point(161, 42);
            this.paidStatusRButton.Name = "paidStatusRButton";
            this.paidStatusRButton.Size = new System.Drawing.Size(73, 17);
            this.paidStatusRButton.TabIndex = 18;
            this.paidStatusRButton.Text = "Сплачено";
            this.paidStatusRButton.UseVisualStyleBackColor = true;
            this.paidStatusRButton.CheckedChanged += new System.EventHandler(this.newStatusRButton_CheckedChanged);
            // 
            // IsCreditCheckBox
            // 
            this.IsCreditCheckBox.AutoSize = true;
            this.IsCreditCheckBox.Location = new System.Drawing.Point(243, 210);
            this.IsCreditCheckBox.Name = "IsCreditCheckBox";
            this.IsCreditCheckBox.Size = new System.Drawing.Size(98, 17);
            this.IsCreditCheckBox.TabIndex = 21;
            this.IsCreditCheckBox.Text = "Надано у борг";
            this.IsCreditCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(210, 517);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "Зберегти";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.estimateNameLabel);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.descriptionTextBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.IsCreditCheckBox);
            this.groupBox2.Controls.Add(this.numberTextBox);
            this.groupBox2.Controls.Add(this.invoiceDate);
            this.groupBox2.Controls.Add(this.invoiceFullSum);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.contractRemainLoadingPicture);
            this.groupBox2.Controls.Add(this.contractorsCBList);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.contractsCBList);
            this.groupBox2.Location = new System.Drawing.Point(8, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(499, 240);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Реквізити";
            // 
            // estimateNameLabel
            // 
            this.estimateNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.estimateNameLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.estimateNameLabel.Location = new System.Drawing.Point(93, 170);
            this.estimateNameLabel.Name = "estimateNameLabel";
            this.estimateNameLabel.Size = new System.Drawing.Size(387, 30);
            this.estimateNameLabel.TabIndex = 24;
            this.estimateNameLabel.Text = "Назва кошторису під який узято зобов\'язання за вищевказаним договором";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Стислий опис:";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(96, 53);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.descriptionTextBox.Size = new System.Drawing.Size(384, 42);
            this.descriptionTextBox.TabIndex = 23;
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Enabled = false;
            this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel3.Location = new System.Drawing.Point(435, 14);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(74, 13);
            this.linkLabel3.TabIndex = 3;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Завантажити";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Enabled = false;
            this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel2.Location = new System.Drawing.Point(60, 14);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(55, 13);
            this.linkLabel2.TabIndex = 2;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Видалити";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.Location = new System.Drawing.Point(9, 14);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(45, 13);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Додати";
            // 
            // filesTable
            // 
            this.filesTable.AllowUserToAddRows = false;
            this.filesTable.AllowUserToDeleteRows = false;
            this.filesTable.AllowUserToResizeColumns = false;
            this.filesTable.AllowUserToResizeRows = false;
            this.filesTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.filesTable.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filesTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.filesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.filesTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Column3});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filesTable.DefaultCellStyle = dataGridViewCellStyle9;
            this.filesTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.filesTable.Location = new System.Drawing.Point(3, 30);
            this.filesTable.MultiSelect = false;
            this.filesTable.Name = "filesTable";
            this.filesTable.ReadOnly = true;
            this.filesTable.RowHeadersVisible = false;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filesTable.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.filesTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.filesTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.filesTable.Size = new System.Drawing.Size(514, 453);
            this.filesTable.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn1.HeaderText = "№ з/п";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "PublicName";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn2.HeaderText = "Назва файлу";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "FileExt";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column3.HeaderText = "Формат";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(528, 512);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(520, 486);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Основні реквізити";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.linkLabel3);
            this.tabPage2.Controls.Add(this.linkLabel2);
            this.tabPage2.Controls.Add(this.filesTable);
            this.tabPage2.Controls.Add(this.linkLabel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(520, 486);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Долучені файли";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // updateContractsListWorker
            // 
            this.updateContractsListWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateContractsListWorker_DoWork);
            this.updateContractsListWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.updateContractsListWorker_RunWorkerCompleted);
            // 
            // updateMoneyRemainsWorker
            // 
            this.updateMoneyRemainsWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateMoneyRemainsWorker_DoWork);
            this.updateMoneyRemainsWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.updateMoneyRemainsWorker_RunWorkerCompleted);
            // 
            // InvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 545);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InvoiceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Новий рахунок";
            ((System.ComponentModel.ISupportInitialize)(this.contractRemainLoadingPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.balanceChangesTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceFullSum)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moneyRemainsLoadingPicture)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filesTable)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox numberTextBox;
        private System.Windows.Forms.DateTimePicker invoiceDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox contractorsCBList;
        private System.Windows.Forms.ComboBox contractsCBList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox contractRemainLoadingPicture;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton newStatusRButton;
        private System.Windows.Forms.RadioButton onPayStatusRButton;
        private System.Windows.Forms.DataGridView balanceChangesTable;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown invoiceFullSum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox IsCreditCheckBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.DataGridView filesTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.RadioButton paidStatusRButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.ComponentModel.BackgroundWorker updateContractsListWorker;
        private System.Windows.Forms.Label estimateNameLabel;
        private System.Windows.Forms.PictureBox moneyRemainsLoadingPicture;
        private System.ComponentModel.BackgroundWorker updateMoneyRemainsWorker;
    }
}