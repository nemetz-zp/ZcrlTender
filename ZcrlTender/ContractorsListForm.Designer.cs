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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
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
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.contractorsTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.contractorsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.contractorsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RowNumberColumn,
            this.NameColumn,
            this.SumColumn});
            this.contractorsTable.Location = new System.Drawing.Point(12, 42);
            this.contractorsTable.MultiSelect = false;
            this.contractorsTable.Name = "contractorsTable";
            this.contractorsTable.ReadOnly = true;
            this.contractorsTable.RowHeadersVisible = false;
            this.contractorsTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.contractorsTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.contractorsTable.Size = new System.Drawing.Size(753, 289);
            this.contractorsTable.TabIndex = 1;
            this.contractorsTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.contractorsTable_CellDoubleClick);
            this.contractorsTable.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.estimateTable_RowsAdded);
            this.contractorsTable.SelectionChanged += new System.EventHandler(this.contractorsTable_SelectionChanged);
            // 
            // RowNumberColumn
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.RowNumberColumn.DefaultCellStyle = dataGridViewCellStyle18;
            this.RowNumberColumn.HeaderText = "№ з/п";
            this.RowNumberColumn.Name = "RowNumberColumn";
            this.RowNumberColumn.ReadOnly = true;
            this.RowNumberColumn.Width = 45;
            // 
            // NameColumn
            // 
            this.NameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.NameColumn.DataPropertyName = "ShortName";
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NameColumn.DefaultCellStyle = dataGridViewCellStyle19;
            this.NameColumn.HeaderText = "Контрагент";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            this.NameColumn.Width = 300;
            // 
            // SumColumn
            // 
            this.SumColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SumColumn.DataPropertyName = "Description";
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.Format = "N2";
            dataGridViewCellStyle20.NullValue = null;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SumColumn.DefaultCellStyle = dataGridViewCellStyle20;
            this.SumColumn.HeaderText = "Примітка";
            this.SumColumn.Name = "SumColumn";
            this.SumColumn.ReadOnly = true;
            // 
            // addContractorButton
            // 
            this.addContractorButton.Location = new System.Drawing.Point(771, 42);
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
            this.editContractorButton.Location = new System.Drawing.Point(771, 71);
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
            this.deleteContractorButton.Location = new System.Drawing.Point(771, 100);
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
            this.loadingPicture.Location = new System.Drawing.Point(266, 168);
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
            this.loadingLabel.Location = new System.Drawing.Point(375, 196);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Пошук за назвою:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(133, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(493, 20);
            this.textBox1.TabIndex = 22;
            // 
            // ContractorsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 343);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
    }
}