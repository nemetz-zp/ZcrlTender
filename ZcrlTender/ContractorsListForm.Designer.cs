namespace ZcrlTender
{
    partial class ContractorsListForm
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
            this.contractorsTable = new System.Windows.Forms.DataGridView();
            this.RowNumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addContractorButton = new System.Windows.Forms.Button();
            this.editContractorButton = new System.Windows.Forms.Button();
            this.deleteContractorButton = new System.Windows.Forms.Button();
            this.loadingPicture = new System.Windows.Forms.PictureBox();
            this.loadingLabel = new System.Windows.Forms.Label();
            this.reloadContractorsListWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.contractorsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // contractorsTable
            // 
            this.contractorsTable.AllowUserToAddRows = false;
            this.contractorsTable.AllowUserToDeleteRows = false;
            this.contractorsTable.AllowUserToResizeColumns = false;
            this.contractorsTable.AllowUserToResizeRows = false;
            this.contractorsTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.contractorsTable.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.contractorsTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.contractorsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.contractorsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RowNumberColumn,
            this.NameColumn,
            this.SumColumn});
            this.contractorsTable.Location = new System.Drawing.Point(12, 12);
            this.contractorsTable.MultiSelect = false;
            this.contractorsTable.Name = "contractorsTable";
            this.contractorsTable.ReadOnly = true;
            this.contractorsTable.RowHeadersVisible = false;
            this.contractorsTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.contractorsTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.contractorsTable.Size = new System.Drawing.Size(753, 269);
            this.contractorsTable.TabIndex = 1;
            this.contractorsTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.contractorsTable_CellDoubleClick);
            this.contractorsTable.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.estimateTable_RowsAdded);
            this.contractorsTable.SelectionChanged += new System.EventHandler(this.contractorsTable_SelectionChanged);
            // 
            // RowNumberColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.RowNumberColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.RowNumberColumn.HeaderText = "№ з/п";
            this.RowNumberColumn.Name = "RowNumberColumn";
            this.RowNumberColumn.ReadOnly = true;
            this.RowNumberColumn.Width = 45;
            // 
            // NameColumn
            // 
            this.NameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.NameColumn.DataPropertyName = "ShortName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NameColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.NameColumn.HeaderText = "Контрагент";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            this.NameColumn.Width = 300;
            // 
            // SumColumn
            // 
            this.SumColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SumColumn.DataPropertyName = "Description";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SumColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.SumColumn.HeaderText = "Примітка";
            this.SumColumn.Name = "SumColumn";
            this.SumColumn.ReadOnly = true;
            // 
            // addContractorButton
            // 
            this.addContractorButton.Location = new System.Drawing.Point(771, 12);
            this.addContractorButton.Name = "addContractorButton";
            this.addContractorButton.Size = new System.Drawing.Size(98, 23);
            this.addContractorButton.TabIndex = 2;
            this.addContractorButton.Text = "Додати";
            this.addContractorButton.UseVisualStyleBackColor = true;
            this.addContractorButton.Click += new System.EventHandler(this.addContractorButton_Click);
            // 
            // editContractorButton
            // 
            this.editContractorButton.Enabled = false;
            this.editContractorButton.Location = new System.Drawing.Point(771, 41);
            this.editContractorButton.Name = "editContractorButton";
            this.editContractorButton.Size = new System.Drawing.Size(98, 23);
            this.editContractorButton.TabIndex = 3;
            this.editContractorButton.Text = "Змінити";
            this.editContractorButton.UseVisualStyleBackColor = true;
            this.editContractorButton.Click += new System.EventHandler(this.editContractorButton_Click);
            // 
            // deleteContractorButton
            // 
            this.deleteContractorButton.Enabled = false;
            this.deleteContractorButton.Location = new System.Drawing.Point(771, 70);
            this.deleteContractorButton.Name = "deleteContractorButton";
            this.deleteContractorButton.Size = new System.Drawing.Size(98, 23);
            this.deleteContractorButton.TabIndex = 4;
            this.deleteContractorButton.Text = "Видалити";
            this.deleteContractorButton.UseVisualStyleBackColor = true;
            this.deleteContractorButton.Click += new System.EventHandler(this.deleteContractorButton_Click);
            // 
            // loadingPicture
            // 
            this.loadingPicture.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.loadingPicture.Image = global::ZcrlTender.Properties.Resources.loader;
            this.loadingPicture.InitialImage = null;
            this.loadingPicture.Location = new System.Drawing.Point(241, 102);
            this.loadingPicture.Name = "loadingPicture";
            this.loadingPicture.Size = new System.Drawing.Size(103, 80);
            this.loadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.loadingPicture.TabIndex = 20;
            this.loadingPicture.TabStop = false;
            this.loadingPicture.Visible = false;
            // 
            // loadingLabel
            // 
            this.loadingLabel.AutoSize = true;
            this.loadingLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.loadingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.loadingLabel.Location = new System.Drawing.Point(350, 130);
            this.loadingLabel.Name = "loadingLabel";
            this.loadingLabel.Size = new System.Drawing.Size(178, 20);
            this.loadingLabel.TabIndex = 19;
            this.loadingLabel.Text = "Оновлення даних ...";
            this.loadingLabel.Visible = false;
            // 
            // reloadContractorsListWorker
            // 
            this.reloadContractorsListWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.reloadContractorsListWorker_DoWork);
            this.reloadContractorsListWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.reloadContractorsListWorker_RunWorkerCompleted);
            // 
            // ContractorsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 293);
            this.Controls.Add(this.loadingPicture);
            this.Controls.Add(this.loadingLabel);
            this.Controls.Add(this.deleteContractorButton);
            this.Controls.Add(this.editContractorButton);
            this.Controls.Add(this.addContractorButton);
            this.Controls.Add(this.contractorsTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ContractorsListForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Список контрагентів";
            ((System.ComponentModel.ISupportInitialize)(this.contractorsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView contractorsTable;
        private System.Windows.Forms.Button addContractorButton;
        private System.Windows.Forms.Button editContractorButton;
        private System.Windows.Forms.Button deleteContractorButton;
        private System.Windows.Forms.PictureBox loadingPicture;
        private System.Windows.Forms.Label loadingLabel;
        private System.ComponentModel.BackgroundWorker reloadContractorsListWorker;
        private System.Windows.Forms.DataGridViewTextBoxColumn RowNumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumColumn;
    }
}