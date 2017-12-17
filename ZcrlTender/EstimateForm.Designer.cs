namespace ZcrlTender
{
    partial class EstimateForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.estimateTotalsTable = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.moneySourceCBList = new System.Windows.Forms.ComboBox();
            this.addMoneySourceButton = new System.Windows.Forms.Button();
            this.deleteMoneySourceButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.estimateNameTextBox = new System.Windows.Forms.TextBox();
            this.saveEstimateDataButton = new System.Windows.Forms.Button();
            this.oldSystemRButton = new System.Windows.Forms.RadioButton();
            this.newSystemRButton = new System.Windows.Forms.RadioButton();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.filesTable = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.estimateTotalsTable)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filesTable)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(3, 88);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(658, 258);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.estimateTotalsTable);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(650, 232);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "ВСЬОГО";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // estimateTotalsTable
            // 
            this.estimateTotalsTable.AllowUserToAddRows = false;
            this.estimateTotalsTable.AllowUserToDeleteRows = false;
            this.estimateTotalsTable.AllowUserToResizeColumns = false;
            this.estimateTotalsTable.AllowUserToResizeRows = false;
            this.estimateTotalsTable.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.estimateTotalsTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.estimateTotalsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.estimateTotalsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.estimateTotalsTable.Location = new System.Drawing.Point(3, 3);
            this.estimateTotalsTable.MultiSelect = false;
            this.estimateTotalsTable.Name = "estimateTotalsTable";
            this.estimateTotalsTable.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.estimateTotalsTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.estimateTotalsTable.RowHeadersWidth = 100;
            this.estimateTotalsTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.estimateTotalsTable.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.estimateTotalsTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.estimateTotalsTable.Size = new System.Drawing.Size(644, 226);
            this.estimateTotalsTable.TabIndex = 0;
            this.estimateTotalsTable.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.estimateTotalsTable_CellValidating);
            this.estimateTotalsTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.estimateTotalsTable_CellValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(6, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Джерело фінансування:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // moneySourceCBList
            // 
            this.moneySourceCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.moneySourceCBList.FormattingEnabled = true;
            this.moneySourceCBList.Location = new System.Drawing.Point(193, 39);
            this.moneySourceCBList.Name = "moneySourceCBList";
            this.moneySourceCBList.Size = new System.Drawing.Size(239, 21);
            this.moneySourceCBList.TabIndex = 2;
            // 
            // addMoneySourceButton
            // 
            this.addMoneySourceButton.Location = new System.Drawing.Point(438, 38);
            this.addMoneySourceButton.Name = "addMoneySourceButton";
            this.addMoneySourceButton.Size = new System.Drawing.Size(105, 22);
            this.addMoneySourceButton.TabIndex = 3;
            this.addMoneySourceButton.Text = "Додати";
            this.addMoneySourceButton.UseVisualStyleBackColor = true;
            this.addMoneySourceButton.Click += new System.EventHandler(this.addMoneySourceButton_Click);
            // 
            // deleteMoneySourceButton
            // 
            this.deleteMoneySourceButton.Enabled = false;
            this.deleteMoneySourceButton.Location = new System.Drawing.Point(549, 38);
            this.deleteMoneySourceButton.Name = "deleteMoneySourceButton";
            this.deleteMoneySourceButton.Size = new System.Drawing.Size(112, 22);
            this.deleteMoneySourceButton.TabIndex = 4;
            this.deleteMoneySourceButton.Text = "Видалити";
            this.deleteMoneySourceButton.UseVisualStyleBackColor = true;
            this.deleteMoneySourceButton.Click += new System.EventHandler(this.deleteMoneySourceButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(6, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Найменування кошторису:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // estimateNameTextBox
            // 
            this.estimateNameTextBox.Location = new System.Drawing.Point(193, 12);
            this.estimateNameTextBox.Name = "estimateNameTextBox";
            this.estimateNameTextBox.Size = new System.Drawing.Size(468, 20);
            this.estimateNameTextBox.TabIndex = 9;
            // 
            // saveEstimateDataButton
            // 
            this.saveEstimateDataButton.Enabled = false;
            this.saveEstimateDataButton.Location = new System.Drawing.Point(266, 378);
            this.saveEstimateDataButton.Name = "saveEstimateDataButton";
            this.saveEstimateDataButton.Size = new System.Drawing.Size(108, 23);
            this.saveEstimateDataButton.TabIndex = 1;
            this.saveEstimateDataButton.Text = "Зберегти дані";
            this.saveEstimateDataButton.UseVisualStyleBackColor = true;
            this.saveEstimateDataButton.Click += new System.EventHandler(this.saveEstimateDataButton_Click);
            // 
            // oldSystemRButton
            // 
            this.oldSystemRButton.AutoSize = true;
            this.oldSystemRButton.Location = new System.Drawing.Point(362, 66);
            this.oldSystemRButton.Name = "oldSystemRButton";
            this.oldSystemRButton.Size = new System.Drawing.Size(115, 17);
            this.oldSystemRButton.TabIndex = 1;
            this.oldSystemRButton.Text = "По старій системі";
            this.oldSystemRButton.UseVisualStyleBackColor = true;
            this.oldSystemRButton.CheckedChanged += new System.EventHandler(this.newSystemRButton_CheckedChanged);
            // 
            // newSystemRButton
            // 
            this.newSystemRButton.AutoSize = true;
            this.newSystemRButton.Checked = true;
            this.newSystemRButton.Location = new System.Drawing.Point(246, 66);
            this.newSystemRButton.Name = "newSystemRButton";
            this.newSystemRButton.Size = new System.Drawing.Size(110, 17);
            this.newSystemRButton.TabIndex = 0;
            this.newSystemRButton.TabStop = true;
            this.newSystemRButton.Text = "По новій системі";
            this.newSystemRButton.UseVisualStyleBackColor = true;
            this.newSystemRButton.CheckedChanged += new System.EventHandler(this.newSystemRButton_CheckedChanged);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(672, 373);
            this.tabControl2.TabIndex = 10;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.oldSystemRButton);
            this.tabPage2.Controls.Add(this.tabControl1);
            this.tabPage2.Controls.Add(this.estimateNameTextBox);
            this.tabPage2.Controls.Add(this.deleteMoneySourceButton);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.newSystemRButton);
            this.tabPage2.Controls.Add(this.moneySourceCBList);
            this.tabPage2.Controls.Add(this.addMoneySourceButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(664, 347);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Реквізити кошторису";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.linkLabel3);
            this.tabPage3.Controls.Add(this.linkLabel2);
            this.tabPage3.Controls.Add(this.linkLabel1);
            this.tabPage3.Controls.Add(this.filesTable);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(664, 347);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Долучені файли";
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Enabled = false;
            this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel3.Location = new System.Drawing.Point(582, 15);
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
            this.linkLabel2.Location = new System.Drawing.Point(59, 15);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(55, 13);
            this.linkLabel2.TabIndex = 6;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Видалити";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.Location = new System.Drawing.Point(8, 15);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(45, 13);
            this.linkLabel1.TabIndex = 5;
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
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filesTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.filesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.filesTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Column3});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filesTable.DefaultCellStyle = dataGridViewCellStyle8;
            this.filesTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.filesTable.Location = new System.Drawing.Point(3, 31);
            this.filesTable.MultiSelect = false;
            this.filesTable.Name = "filesTable";
            this.filesTable.ReadOnly = true;
            this.filesTable.RowHeadersVisible = false;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.filesTable.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.filesTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.filesTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.filesTable.Size = new System.Drawing.Size(658, 313);
            this.filesTable.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn1.HeaderText = "№ з/п";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "PublicName";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn2.HeaderText = "Назва файлу";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "FileExt";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column3.HeaderText = "Формат";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // EstimateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 408);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.saveEstimateDataButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EstimateForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Кошторис";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.estimateTotalsTable)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filesTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox moneySourceCBList;
        private System.Windows.Forms.Button addMoneySourceButton;
        private System.Windows.Forms.Button deleteMoneySourceButton;
        private System.Windows.Forms.Button saveEstimateDataButton;
        private System.Windows.Forms.RadioButton oldSystemRButton;
        private System.Windows.Forms.RadioButton newSystemRButton;
        private System.Windows.Forms.DataGridView estimateTotalsTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox estimateNameTextBox;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.DataGridView filesTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}