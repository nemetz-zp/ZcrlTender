namespace ZcrlTender
{
    partial class TenderYearForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TenderYearForm));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.addYearButton = new System.Windows.Forms.Button();
            this.selectYearButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(300, 160);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // addYearButton
            // 
            this.addYearButton.Location = new System.Drawing.Point(318, 41);
            this.addYearButton.Name = "addYearButton";
            this.addYearButton.Size = new System.Drawing.Size(94, 23);
            this.addYearButton.TabIndex = 1;
            this.addYearButton.Text = "Додати новий";
            this.addYearButton.UseVisualStyleBackColor = true;
            this.addYearButton.Click += new System.EventHandler(this.addYearButton_Click);
            // 
            // selectYearButton
            // 
            this.selectYearButton.Enabled = false;
            this.selectYearButton.Location = new System.Drawing.Point(318, 12);
            this.selectYearButton.Name = "selectYearButton";
            this.selectYearButton.Size = new System.Drawing.Size(94, 23);
            this.selectYearButton.TabIndex = 2;
            this.selectYearButton.Text = "Вибрати";
            this.selectYearButton.UseVisualStyleBackColor = true;
            this.selectYearButton.Click += new System.EventHandler(this.selectYearButton_Click);
            // 
            // TenderYearForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 193);
            this.Controls.Add(this.selectYearButton);
            this.Controls.Add(this.addYearButton);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TenderYearForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Виберіть рік закупівлі";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button addYearButton;
        private System.Windows.Forms.Button selectYearButton;
    }
}