using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZcrlTender
{
    public partial class ActionCommentForm : Form
    {
        private string reasonDescription;

        public string ReasonDescription
        {
            get
            {
                return reasonDescription;
            }
        }

        public ActionCommentForm()
        {
            InitializeComponent();
        }

        public ActionCommentForm(string msg) : this()
        {
            label1.Text = msg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text))
            {
                NotificationHelper.ShowError("Ви не вказали опис дії для операції, що хочете виконати");
                return;
            }
            else
            {
                reasonDescription = textBox1.Text;
                Close();
            }
        }


    }
}
