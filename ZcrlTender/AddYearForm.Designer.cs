namespace ZcrlTender
{
    partial class AddYearForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.yearValue = new System.Windows.Forms.NumericUpDown();
            this.yearDescription = new System.Windows.Forms.TextBox();
            this.addYearButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.yearValue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(15, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Рік:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(15, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Коментар:";
            // 
            // yearValue
            // 
            this.yearValue.Location = new System.Drawing.Point(111, 27);
            this.yearValue.Maximum = new decimal(new int[] {
            2050,
            0,
            0,
            0});
            this.yearValue.Minimum = new decimal(new int[] {
            1990,
            0,
            0,
            0});
            this.yearValue.Name = "yearValue";
            this.yearValue.Size = new System.Drawing.Size(74, 20);
            this.yearValue.TabIndex = 2;
            this.yearValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.yearValue.Value = new decimal(new int[] {
            1990,
            0,
            0,
            0});
            // 
            // yearDescription
            // 
            this.yearDescription.Location = new System.Drawing.Point(111, 54);
            this.yearDescription.Name = "yearDescription";
            this.yearDescription.Size = new System.Drawing.Size(210, 20);
            this.yearDescription.TabIndex = 3;
            // 
            // addYearButton
            // 
            this.addYearButton.Location = new System.Drawing.Point(111, 81);
            this.addYearButton.Name = "addYearButton";
            this.addYearButton.Size = new System.Drawing.Size(75, 23);
            this.addYearButton.TabIndex = 4;
            this.addYearButton.Text = "Додати рік";
            this.addYearButton.UseVisualStyleBackColor = true;
            this.addYearButton.Click += new System.EventHandler(this.addYearButton_Click);
            // 
            // AddYearForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 115);
            this.Controls.Add(this.addYearButton);
            this.Controls.Add(this.yearDescription);
            this.Controls.Add(this.yearValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddYearForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Додати новий рік закупівель";
            ((System.ComponentModel.ISupportInitialize)(this.yearValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown yearValue;
        private System.Windows.Forms.TextBox yearDescription;
        private System.Windows.Forms.Button addYearButton;
    }
}