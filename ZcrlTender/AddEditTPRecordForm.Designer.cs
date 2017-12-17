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
            ((System.ComponentModel.ISupportInitialize)(this.dkCodeSum)).BeginInit();
            this.SuspendLayout();
            // 
            // dkCodesCBList
            // 
            this.dkCodesCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dkCodesCBList.FormattingEnabled = true;
            this.dkCodesCBList.Location = new System.Drawing.Point(116, 115);
            this.dkCodesCBList.Name = "dkCodesCBList";
            this.dkCodesCBList.Size = new System.Drawing.Size(457, 21);
            this.dkCodesCBList.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Код за ДК:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 240);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Сума:";
            // 
            // dkCodeSum
            // 
            this.dkCodeSum.DecimalPlaces = 2;
            this.dkCodeSum.Location = new System.Drawing.Point(116, 238);
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
            this.button1.Location = new System.Drawing.Point(267, 268);
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
            this.label5.Location = new System.Drawing.Point(12, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Конкретна назва:";
            // 
            // conctretePlanItemName
            // 
            this.conctretePlanItemName.Location = new System.Drawing.Point(116, 152);
            this.conctretePlanItemName.Multiline = true;
            this.conctretePlanItemName.Name = "conctretePlanItemName";
            this.conctretePlanItemName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.conctretePlanItemName.Size = new System.Drawing.Size(457, 73);
            this.conctretePlanItemName.TabIndex = 21;
            // 
            // altKekv
            // 
            this.altKekv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.altKekv.FormattingEnabled = true;
            this.altKekv.Location = new System.Drawing.Point(165, 78);
            this.altKekv.Name = "altKekv";
            this.altKekv.Size = new System.Drawing.Size(156, 21);
            this.altKekv.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(147, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "КЕКВ за старою системою:";
            // 
            // mainKekv
            // 
            this.mainKekv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mainKekv.FormattingEnabled = true;
            this.mainKekv.Location = new System.Drawing.Point(165, 47);
            this.mainKekv.Name = "mainKekv";
            this.mainKekv.Size = new System.Drawing.Size(156, 21);
            this.mainKekv.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "КЕКВ за новою системою:";
            // 
            // moneyRemainLabel
            // 
            this.moneyRemainLabel.AutoSize = true;
            this.moneyRemainLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.moneyRemainLabel.Location = new System.Drawing.Point(327, 50);
            this.moneyRemainLabel.Name = "moneyRemainLabel";
            this.moneyRemainLabel.Size = new System.Drawing.Size(253, 13);
            this.moneyRemainLabel.TabIndex = 27;
            this.moneyRemainLabel.Text = "Вільні (нерозподілені) кошти: 102 102 102,65 грн.";
            // 
            // estimatesCBList
            // 
            this.estimatesCBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.estimatesCBList.FormattingEnabled = true;
            this.estimatesCBList.Location = new System.Drawing.Point(81, 17);
            this.estimatesCBList.Name = "estimatesCBList";
            this.estimatesCBList.Size = new System.Drawing.Size(240, 21);
            this.estimatesCBList.TabIndex = 29;
            this.estimatesCBList.SelectedIndexChanged += new System.EventHandler(this.estimatesCBList_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Кошторис:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(250, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(246, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "З них зайнято договорами: 102 102 102,65 грн.";
            this.label6.Visible = false;
            // 
            // AddEditTPRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 299);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.estimatesCBList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.moneyRemainLabel);
            this.Controls.Add(this.altKekv);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.mainKekv);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.conctretePlanItemName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dkCodeSum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dkCodesCBList);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditTPRecordForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Створення запису в річному плані";
            ((System.ComponentModel.ISupportInitialize)(this.dkCodeSum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}