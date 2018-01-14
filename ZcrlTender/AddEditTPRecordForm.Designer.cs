namespace ZcrlTender
{
    partial class AddEditTPRecordForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dkCodesCBList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dkCodeSum = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.conctretePlanItemName = new System.Windows.Forms.TextBox();
            this.altKekv = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.mainKekv = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.moneyRemainLabel = new System.Windows.Forms.Label();
            this.estimatesCBList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.codeRepeatReasonTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.isCodeRepeatCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.protocolDate = new System.Windows.Forms.DateTimePicker();
            this.protocolNum = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tenderBeginDate = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.procedureTypeCBList = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.isProcedureComplete = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.filesTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dkCodeSum)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filesTable)).BeginInit();
            this.SuspendLayout();
            // 
            // dkCodesCBList
            // 
            this.dkCodesCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dkCodesCBList.FormattingEnabled = true;
            this.dkCodesCBList.Location = new System.Drawing.Point(79, 119);
            this.dkCodesCBList.Name = "dkCodesCBList";
            this.dkCodesCBList.Size = new System.Drawing.Size(502, 21);
            this.dkCodesCBList.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Код за ДК:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Сума:";
            // 
            // dkCodeSum
            // 
            this.dkCodeSum.DecimalPlaces = 2;
            this.dkCodeSum.Location = new System.Drawing.Point(54, 23);
            this.dkCodeSum.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.dkCodeSum.Name = "dkCodeSum";
            this.dkCodeSum.Size = new System.Drawing.Size(116, 20);
            this.dkCodeSum.TabIndex = 5;
            this.dkCodeSum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(246, 387);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Зберегти";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Конкретна назва:";
            // 
            // conctretePlanItemName
            // 
            this.conctretePlanItemName.Location = new System.Drawing.Point(113, 153);
            this.conctretePlanItemName.Multiline = true;
            this.conctretePlanItemName.Name = "conctretePlanItemName";
            this.conctretePlanItemName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.conctretePlanItemName.Size = new System.Drawing.Size(468, 73);
            this.conctretePlanItemName.TabIndex = 21;
            // 
            // altKekv
            // 
            this.altKekv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.altKekv.FormattingEnabled = true;
            this.altKekv.Location = new System.Drawing.Point(168, 89);
            this.altKekv.Name = "altKekv";
            this.altKekv.Size = new System.Drawing.Size(156, 21);
            this.altKekv.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(147, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "КЕКВ за старою системою:";
            // 
            // mainKekv
            // 
            this.mainKekv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mainKekv.FormattingEnabled = true;
            this.mainKekv.Location = new System.Drawing.Point(168, 58);
            this.mainKekv.Name = "mainKekv";
            this.mainKekv.Size = new System.Drawing.Size(156, 21);
            this.mainKekv.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "КЕКВ за новою системою:";
            // 
            // moneyRemainLabel
            // 
            this.moneyRemainLabel.AutoSize = true;
            this.moneyRemainLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.moneyRemainLabel.Location = new System.Drawing.Point(330, 61);
            this.moneyRemainLabel.Name = "moneyRemainLabel";
            this.moneyRemainLabel.Size = new System.Drawing.Size(253, 13);
            this.moneyRemainLabel.TabIndex = 27;
            this.moneyRemainLabel.Text = "Вільні (нерозподілені) кошти: 102 102 102,65 грн.";
            // 
            // estimatesCBList
            // 
            this.estimatesCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.estimatesCBList.FormattingEnabled = true;
            this.estimatesCBList.Location = new System.Drawing.Point(81, 53);
            this.estimatesCBList.Name = "estimatesCBList";
            this.estimatesCBList.Size = new System.Drawing.Size(502, 21);
            this.estimatesCBList.TabIndex = 29;
            this.estimatesCBList.SelectedIndexChanged += new System.EventHandler(this.estimatesCBList_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Кошторис:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(176, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(246, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "З них зайнято договорами: 102 102 102,65 грн.";
            this.label6.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.estimatesCBList);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(8, 120);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(595, 94);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Фінансування закупівлі";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(405, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Оберіть кошторис, під кошти якому створюється данний запис у річному плані";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.mainKekv);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.moneyRemainLabel);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.altKekv);
            this.groupBox2.Location = new System.Drawing.Point(8, 218);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(595, 126);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Коди за КЕКВ";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(15, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(541, 30);
            this.label9.TabIndex = 1;
            this.label9.Text = "Вкажіть коди за КЕКВ за новою (основною) системою та за старою системою. Якщо вон" +
    "и співпадать - можете вказати один і той же код.";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.codeRepeatReasonTextBox);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.isCodeRepeatCheckBox);
            this.groupBox3.Controls.Add(this.dkCodesCBList);
            this.groupBox3.Controls.Add(this.conctretePlanItemName);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(8, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(593, 242);
            this.groupBox3.TabIndex = 33;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Код предмету закупівлі за класифікатором";
            // 
            // codeRepeatReasonTextBox
            // 
            this.codeRepeatReasonTextBox.Enabled = false;
            this.codeRepeatReasonTextBox.Location = new System.Drawing.Point(112, 39);
            this.codeRepeatReasonTextBox.Multiline = true;
            this.codeRepeatReasonTextBox.Name = "codeRepeatReasonTextBox";
            this.codeRepeatReasonTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.codeRepeatReasonTextBox.Size = new System.Drawing.Size(469, 67);
            this.codeRepeatReasonTextBox.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(13, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(123, 32);
            this.label10.TabIndex = 22;
            this.label10.Text = "Обгрунтування повторення коду:";
            // 
            // isCodeRepeatCheckBox
            // 
            this.isCodeRepeatCheckBox.AutoSize = true;
            this.isCodeRepeatCheckBox.Location = new System.Drawing.Point(13, 19);
            this.isCodeRepeatCheckBox.Name = "isCodeRepeatCheckBox";
            this.isCodeRepeatCheckBox.Size = new System.Drawing.Size(215, 17);
            this.isCodeRepeatCheckBox.TabIndex = 4;
            this.isCodeRepeatCheckBox.Text = "Створення коду, що вже існує у плані";
            this.isCodeRepeatCheckBox.UseVisualStyleBackColor = true;
            this.isCodeRepeatCheckBox.CheckedChanged += new System.EventHandler(this.isCodeRepeatCheckBox_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.protocolDate);
            this.groupBox4.Controls.Add(this.protocolNum);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.tenderBeginDate);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.procedureTypeCBList);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Location = new System.Drawing.Point(8, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(592, 106);
            this.groupBox4.TabIndex = 34;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Опис процедури закупівлі";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(288, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(21, 13);
            this.label14.TabIndex = 7;
            this.label14.Text = "від";
            // 
            // protocolDate
            // 
            this.protocolDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.protocolDate.Location = new System.Drawing.Point(315, 71);
            this.protocolDate.Name = "protocolDate";
            this.protocolDate.Size = new System.Drawing.Size(106, 20);
            this.protocolDate.TabIndex = 6;
            // 
            // protocolNum
            // 
            this.protocolNum.Location = new System.Drawing.Point(180, 71);
            this.protocolNum.Name = "protocolNum";
            this.protocolNum.Size = new System.Drawing.Size(100, 20);
            this.protocolNum.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(162, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Затвердждено протоколом № ";
            // 
            // tenderBeginDate
            // 
            this.tenderBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tenderBeginDate.Location = new System.Drawing.Point(477, 29);
            this.tenderBeginDate.Name = "tenderBeginDate";
            this.tenderBeginDate.Size = new System.Drawing.Size(106, 20);
            this.tenderBeginDate.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(374, 29);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 29);
            this.label12.TabIndex = 2;
            this.label12.Text = "Орієнтований початок закупівлі:";
            // 
            // procedureTypeCBList
            // 
            this.procedureTypeCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.procedureTypeCBList.FormattingEnabled = true;
            this.procedureTypeCBList.Location = new System.Drawing.Point(100, 29);
            this.procedureTypeCBList.Name = "procedureTypeCBList";
            this.procedureTypeCBList.Size = new System.Drawing.Size(255, 21);
            this.procedureTypeCBList.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 32);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Тип процедури:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.isProcedureComplete);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.dkCodeSum);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Location = new System.Drawing.Point(9, 254);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(592, 89);
            this.groupBox5.TabIndex = 35;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Запланована сумма";
            // 
            // isProcedureComplete
            // 
            this.isProcedureComplete.AutoSize = true;
            this.isProcedureComplete.Location = new System.Drawing.Point(15, 53);
            this.isProcedureComplete.Name = "isProcedureComplete";
            this.isProcedureComplete.Size = new System.Drawing.Size(185, 17);
            this.isProcedureComplete.TabIndex = 31;
            this.isProcedureComplete.Text = "Завершити процедуру закупівлі";
            this.isProcedureComplete.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(622, 381);
            this.tabControl1.TabIndex = 36;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(614, 355);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Дані процедури";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(614, 355);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Предмет закупівлі";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.linkLabel3);
            this.tabPage3.Controls.Add(this.linkLabel2);
            this.tabPage3.Controls.Add(this.filesTable);
            this.tabPage3.Controls.Add(this.linkLabel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(614, 355);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Долучені файли";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Enabled = false;
            this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel3.Location = new System.Drawing.Point(532, 9);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(74, 13);
            this.linkLabel3.TabIndex = 7;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Завантажити";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Enabled = false;
            this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel2.Location = new System.Drawing.Point(57, 9);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(55, 13);
            this.linkLabel2.TabIndex = 6;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Видалити";
            // 
            // filesTable
            // 
            this.filesTable.AllowUserToAddRows = false;
            this.filesTable.AllowUserToDeleteRows = false;
            this.filesTable.AllowUserToResizeColumns = false;
            this.filesTable.AllowUserToResizeRows = false;
            this.filesTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.filesTable.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filesTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.filesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.filesTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filesTable.DefaultCellStyle = dataGridViewCellStyle5;
            this.filesTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.filesTable.Location = new System.Drawing.Point(0, 29);
            this.filesTable.MultiSelect = false;
            this.filesTable.Name = "filesTable";
            this.filesTable.ReadOnly = true;
            this.filesTable.RowHeadersVisible = false;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filesTable.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.filesTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.filesTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.filesTable.Size = new System.Drawing.Size(614, 326);
            this.filesTable.TabIndex = 4;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "№ з/п";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 50;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.DataPropertyName = "PublicName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "Назва файлу";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "FileExt";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column3.HeaderText = "Формат";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.Location = new System.Drawing.Point(6, 9);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(45, 13);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Додати";
            // 
            // AddEditTPRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 417);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditTPRecordForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Створення запису в річному плані";
            ((System.ComponentModel.ISupportInitialize)(this.dkCodeSum)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filesTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox dkCodesCBList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown dkCodeSum;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox conctretePlanItemName;
        private System.Windows.Forms.ComboBox altKekv;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox mainKekv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label moneyRemainLabel;
        private System.Windows.Forms.ComboBox estimatesCBList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox codeRepeatReasonTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox isCodeRepeatCheckBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DateTimePicker tenderBeginDate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox procedureTypeCBList;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox isProcedureComplete;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker protocolDate;
        private System.Windows.Forms.TextBox protocolNum;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.DataGridView filesTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}