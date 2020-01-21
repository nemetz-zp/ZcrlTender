namespace ZcrlTender
{
    partial class ReserveMoneyOnSourceControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.balanceChangesTable = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updateMoneyRemainWorker = new System.ComponentModel.BackgroundWorker();
            this.moneyRemainsLoadingPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.balanceChangesTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.moneyRemainsLoadingPicture)).BeginInit();
            this.SuspendLayout();
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
            this.balanceChangesTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.balanceChangesTable.Enabled = false;
            this.balanceChangesTable.Location = new System.Drawing.Point(0, 0);
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
            this.balanceChangesTable.Size = new System.Drawing.Size(476, 148);
            this.balanceChangesTable.TabIndex = 18;
            this.balanceChangesTable.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.balanceChangesTable.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
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
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "Доступно коштів";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 154;
            // 
            // moneyRemainsLoadingPicture
            // 
            this.moneyRemainsLoadingPicture.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.moneyRemainsLoadingPicture.Image = global::ZcrlTender.Properties.Resources.loader;
            this.moneyRemainsLoadingPicture.InitialImage = null;
            this.moneyRemainsLoadingPicture.Location = new System.Drawing.Point(180, 41);
            this.moneyRemainsLoadingPicture.Name = "moneyRemainsLoadingPicture";
            this.moneyRemainsLoadingPicture.Size = new System.Drawing.Size(103, 80);
            this.moneyRemainsLoadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.moneyRemainsLoadingPicture.TabIndex = 20;
            this.moneyRemainsLoadingPicture.TabStop = false;
            this.moneyRemainsLoadingPicture.Visible = false;
            // 
            // ReserveMoneyOnSourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.moneyRemainsLoadingPicture);
            this.Controls.Add(this.balanceChangesTable);
            this.Name = "ReserveMoneyOnSourceControl";
            this.Size = new System.Drawing.Size(476, 148);
            ((System.ComponentModel.ISupportInitialize)(this.balanceChangesTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.moneyRemainsLoadingPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView balanceChangesTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.ComponentModel.BackgroundWorker updateMoneyRemainWorker;
        private System.Windows.Forms.PictureBox moneyRemainsLoadingPicture;
    }
}
