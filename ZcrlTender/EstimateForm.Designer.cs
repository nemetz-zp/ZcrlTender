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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.estimateTotalsTable = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.moneySourceCBList = new System.Windows.Forms.ComboBox();
            this.addMoneySourceButton = new System.Windows.Forms.Button();
            this.deleteMoneySourceButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.estimateNameTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.saveEstimateDataButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.oldSystemRButton = new System.Windows.Forms.RadioButton();
            this.newSystemRButton = new System.Windows.Forms.RadioButton();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.estimateTotalsTable)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(645, 294);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.estimateTotalsTable);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(637, 268);
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
            this.estimateTotalsTable.Size = new System.Drawing.Size(631, 262);
            this.estimateTotalsTable.TabIndex = 0;
            this.estimateTotalsTable.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.estimateTotalsTable_CellValidating);
            this.estimateTotalsTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.estimateTotalsTable_CellValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(3, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 28);
            this.label1.TabIndex = 1;
            this.label1.Text = "Джерело фінансування:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // moneySourceCBList
            // 
            this.moneySourceCBList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.moneySourceCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.moneySourceCBList.FormattingEnabled = true;
            this.moneySourceCBList.Location = new System.Drawing.Point(198, 29);
            this.moneySourceCBList.Name = "moneySourceCBList";
            this.moneySourceCBList.Size = new System.Drawing.Size(222, 21);
            this.moneySourceCBList.TabIndex = 2;
            // 
            // addMoneySourceButton
            // 
            this.addMoneySourceButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addMoneySourceButton.Location = new System.Drawing.Point(426, 29);
            this.addMoneySourceButton.Name = "addMoneySourceButton";
            this.addMoneySourceButton.Size = new System.Drawing.Size(105, 22);
            this.addMoneySourceButton.TabIndex = 3;
            this.addMoneySourceButton.Text = "Додати";
            this.addMoneySourceButton.UseVisualStyleBackColor = true;
            this.addMoneySourceButton.Click += new System.EventHandler(this.addMoneySourceButton_Click);
            // 
            // deleteMoneySourceButton
            // 
            this.deleteMoneySourceButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deleteMoneySourceButton.Enabled = false;
            this.deleteMoneySourceButton.Location = new System.Drawing.Point(537, 29);
            this.deleteMoneySourceButton.Name = "deleteMoneySourceButton";
            this.deleteMoneySourceButton.Size = new System.Drawing.Size(111, 22);
            this.deleteMoneySourceButton.TabIndex = 4;
            this.deleteMoneySourceButton.Text = "Видалити";
            this.deleteMoneySourceButton.UseVisualStyleBackColor = true;
            this.deleteMoneySourceButton.Click += new System.EventHandler(this.deleteMoneySourceButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.17225F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.82775F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.moneySourceCBList, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.addMoneySourceButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.deleteMoneySourceButton, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.estimateNameTextBox, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(651, 54);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 26);
            this.label2.TabIndex = 8;
            this.label2.Text = "Найменування кошторису:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // estimateNameTextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.estimateNameTextBox, 3);
            this.estimateNameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.estimateNameTextBox.Location = new System.Drawing.Point(198, 3);
            this.estimateNameTextBox.Name = "estimateNameTextBox";
            this.estimateNameTextBox.Size = new System.Drawing.Size(450, 20);
            this.estimateNameTextBox.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.saveEstimateDataButton, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 94);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.93655F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.063444F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(651, 331);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // saveEstimateDataButton
            // 
            this.saveEstimateDataButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveEstimateDataButton.Enabled = false;
            this.saveEstimateDataButton.Location = new System.Drawing.Point(3, 303);
            this.saveEstimateDataButton.Name = "saveEstimateDataButton";
            this.saveEstimateDataButton.Size = new System.Drawing.Size(645, 25);
            this.saveEstimateDataButton.TabIndex = 1;
            this.saveEstimateDataButton.Text = "Зберегти дані";
            this.saveEstimateDataButton.UseVisualStyleBackColor = true;
            this.saveEstimateDataButton.Click += new System.EventHandler(this.saveEstimateDataButton_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.oldSystemRButton, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.newSystemRButton, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 54);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(651, 27);
            this.tableLayoutPanel3.TabIndex = 7;
            // 
            // oldSystemRButton
            // 
            this.oldSystemRButton.AutoSize = true;
            this.oldSystemRButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.oldSystemRButton.Location = new System.Drawing.Point(328, 3);
            this.oldSystemRButton.Name = "oldSystemRButton";
            this.oldSystemRButton.Size = new System.Drawing.Size(115, 21);
            this.oldSystemRButton.TabIndex = 1;
            this.oldSystemRButton.Text = "По старій системі";
            this.oldSystemRButton.UseVisualStyleBackColor = true;
            this.oldSystemRButton.CheckedChanged += new System.EventHandler(this.newSystemRButton_CheckedChanged);
            // 
            // newSystemRButton
            // 
            this.newSystemRButton.AutoSize = true;
            this.newSystemRButton.Checked = true;
            this.newSystemRButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.newSystemRButton.Location = new System.Drawing.Point(212, 3);
            this.newSystemRButton.Name = "newSystemRButton";
            this.newSystemRButton.Size = new System.Drawing.Size(110, 21);
            this.newSystemRButton.TabIndex = 0;
            this.newSystemRButton.TabStop = true;
            this.newSystemRButton.Text = "По новій системі";
            this.newSystemRButton.UseVisualStyleBackColor = true;
            this.newSystemRButton.CheckedChanged += new System.EventHandler(this.newSystemRButton_CheckedChanged);
            // 
            // EstimateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 425);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
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
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox moneySourceCBList;
        private System.Windows.Forms.Button addMoneySourceButton;
        private System.Windows.Forms.Button deleteMoneySourceButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button saveEstimateDataButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton oldSystemRButton;
        private System.Windows.Forms.RadioButton newSystemRButton;
        private System.Windows.Forms.DataGridView estimateTotalsTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox estimateNameTextBox;
    }
}