namespace ZcrlTender
{
    partial class TenderPlanRecordSelectForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tenderPlanTable = new System.Windows.Forms.DataGridView();
            this.KekvColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DkCodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlannedSumColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContractsSumColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsedByContractsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordsLoader = new System.ComponentModel.BackgroundWorker();
            this.yearPlanLoadingPicture = new System.Windows.Forms.PictureBox();
            this.yearPlanLoadingLabel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tpAltSystemRButton = new System.Windows.Forms.RadioButton();
            this.tpEstimateCBList = new System.Windows.Forms.ComboBox();
            this.tpNewSystemRButton = new System.Windows.Forms.RadioButton();
            this.tpKekvsCBList = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tenderPlanTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yearPlanLoadingPicture)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tenderPlanTable
            // 
            this.tenderPlanTable.AllowUserToAddRows = false;
            this.tenderPlanTable.AllowUserToDeleteRows = false;
            this.tenderPlanTable.AllowUserToResizeColumns = false;
            this.tenderPlanTable.AllowUserToResizeRows = false;
            this.tenderPlanTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tenderPlanTable.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tenderPlanTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tenderPlanTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tenderPlanTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KekvColumn,
            this.DkCodeColumn,
            this.PlannedSumColumn,
            this.ContractsSumColumn,
            this.UsedByContractsColumn});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tenderPlanTable.DefaultCellStyle = dataGridViewCellStyle7;
            this.tenderPlanTable.Location = new System.Drawing.Point(7, 92);
            this.tenderPlanTable.MultiSelect = false;
            this.tenderPlanTable.Name = "tenderPlanTable";
            this.tenderPlanTable.ReadOnly = true;
            this.tenderPlanTable.RowHeadersVisible = false;
            this.tenderPlanTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tenderPlanTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tenderPlanTable.Size = new System.Drawing.Size(765, 312);
            this.tenderPlanTable.TabIndex = 2;
            this.tenderPlanTable.SelectionChanged += new System.EventHandler(this.tenderPlanTable_SelectionChanged);
            // 
            // KekvColumn
            // 
            this.KekvColumn.DataPropertyName = "Kekv";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.KekvColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.KekvColumn.HeaderText = "КЕКВ";
            this.KekvColumn.Name = "KekvColumn";
            this.KekvColumn.ReadOnly = true;
            this.KekvColumn.Width = 60;
            // 
            // DkCodeColumn
            // 
            this.DkCodeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DkCodeColumn.DataPropertyName = "Dk";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DkCodeColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.DkCodeColumn.HeaderText = "Код за ДК";
            this.DkCodeColumn.Name = "DkCodeColumn";
            this.DkCodeColumn.ReadOnly = true;
            // 
            // PlannedSumColumn
            // 
            this.PlannedSumColumn.DataPropertyName = "PlannedMoney";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PlannedSumColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.PlannedSumColumn.HeaderText = "Запланована сума";
            this.PlannedSumColumn.Name = "PlannedSumColumn";
            this.PlannedSumColumn.ReadOnly = true;
            // 
            // ContractsSumColumn
            // 
            this.ContractsSumColumn.DataPropertyName = "MoneyRemain";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ContractsSumColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.ContractsSumColumn.HeaderText = "Залишок для реєстрації";
            this.ContractsSumColumn.Name = "ContractsSumColumn";
            this.ContractsSumColumn.ReadOnly = true;
            // 
            // UsedByContractsColumn
            // 
            this.UsedByContractsColumn.DataPropertyName = "Commentary";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.NullValue = null;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.UsedByContractsColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.UsedByContractsColumn.HeaderText = "Примітка";
            this.UsedByContractsColumn.Name = "UsedByContractsColumn";
            this.UsedByContractsColumn.ReadOnly = true;
            this.UsedByContractsColumn.Width = 200;
            // 
            // recordsLoader
            // 
            this.recordsLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.recordsLoader_DoWork);
            this.recordsLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.recordsLoader_RunWorkerCompleted);
            // 
            // yearPlanLoadingPicture
            // 
            this.yearPlanLoadingPicture.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.yearPlanLoadingPicture.Image = global::ZcrlTender.Properties.Resources.loader;
            this.yearPlanLoadingPicture.InitialImage = null;
            this.yearPlanLoadingPicture.Location = new System.Drawing.Point(265, 221);
            this.yearPlanLoadingPicture.Name = "yearPlanLoadingPicture";
            this.yearPlanLoadingPicture.Size = new System.Drawing.Size(103, 80);
            this.yearPlanLoadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.yearPlanLoadingPicture.TabIndex = 20;
            this.yearPlanLoadingPicture.TabStop = false;
            this.yearPlanLoadingPicture.Visible = false;
            // 
            // yearPlanLoadingLabel
            // 
            this.yearPlanLoadingLabel.AutoSize = true;
            this.yearPlanLoadingLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.yearPlanLoadingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yearPlanLoadingLabel.Location = new System.Drawing.Point(374, 249);
            this.yearPlanLoadingLabel.Name = "yearPlanLoadingLabel";
            this.yearPlanLoadingLabel.Size = new System.Drawing.Size(178, 20);
            this.yearPlanLoadingLabel.TabIndex = 19;
            this.yearPlanLoadingLabel.Text = "Оновлення даних ...";
            this.yearPlanLoadingLabel.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.tpAltSystemRButton);
            this.groupBox3.Controls.Add(this.tpEstimateCBList);
            this.groupBox3.Controls.Add(this.tpNewSystemRButton);
            this.groupBox3.Controls.Add(this.tpKekvsCBList);
            this.groupBox3.Location = new System.Drawing.Point(7, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(868, 78);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Фільтр за критеріями";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 51);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(38, 13);
            this.label17.TabIndex = 14;
            this.label17.Text = "КЕКВ:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(5, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Кошторис:";
            // 
            // tpAltSystemRButton
            // 
            this.tpAltSystemRButton.AutoSize = true;
            this.tpAltSystemRButton.Location = new System.Drawing.Point(329, 49);
            this.tpAltSystemRButton.Name = "tpAltSystemRButton";
            this.tpAltSystemRButton.Size = new System.Drawing.Size(115, 17);
            this.tpAltSystemRButton.TabIndex = 17;
            this.tpAltSystemRButton.Text = "По старій системі";
            this.tpAltSystemRButton.UseVisualStyleBackColor = true;
            // 
            // tpEstimateCBList
            // 
            this.tpEstimateCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tpEstimateCBList.FormattingEnabled = true;
            this.tpEstimateCBList.Location = new System.Drawing.Point(78, 21);
            this.tpEstimateCBList.Name = "tpEstimateCBList";
            this.tpEstimateCBList.Size = new System.Drawing.Size(609, 21);
            this.tpEstimateCBList.TabIndex = 5;
            // 
            // tpNewSystemRButton
            // 
            this.tpNewSystemRButton.AutoSize = true;
            this.tpNewSystemRButton.Checked = true;
            this.tpNewSystemRButton.Location = new System.Drawing.Point(213, 49);
            this.tpNewSystemRButton.Name = "tpNewSystemRButton";
            this.tpNewSystemRButton.Size = new System.Drawing.Size(110, 17);
            this.tpNewSystemRButton.TabIndex = 16;
            this.tpNewSystemRButton.TabStop = true;
            this.tpNewSystemRButton.Text = "По новій системі";
            this.tpNewSystemRButton.UseVisualStyleBackColor = true;
            // 
            // tpKekvsCBList
            // 
            this.tpKekvsCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tpKekvsCBList.FormattingEnabled = true;
            this.tpKekvsCBList.Location = new System.Drawing.Point(78, 48);
            this.tpKekvsCBList.Name = "tpKekvsCBList";
            this.tpKekvsCBList.Size = new System.Drawing.Size(109, 21);
            this.tpKekvsCBList.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(778, 92);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "Вибрати";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TenderPlanRecordSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 404);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.yearPlanLoadingPicture);
            this.Controls.Add(this.yearPlanLoadingLabel);
            this.Controls.Add(this.tenderPlanTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TenderPlanRecordSelectForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Вибір запису у річному плані";
            ((System.ComponentModel.ISupportInitialize)(this.tenderPlanTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yearPlanLoadingPicture)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView tenderPlanTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn KekvColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DkCodeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlannedSumColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContractsSumColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsedByContractsColumn;
        private System.ComponentModel.BackgroundWorker recordsLoader;
        private System.Windows.Forms.PictureBox yearPlanLoadingPicture;
        private System.Windows.Forms.Label yearPlanLoadingLabel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton tpAltSystemRButton;
        private System.Windows.Forms.ComboBox tpEstimateCBList;
        private System.Windows.Forms.RadioButton tpNewSystemRButton;
        private System.Windows.Forms.ComboBox tpKekvsCBList;
        private System.Windows.Forms.Button button1;
    }
}