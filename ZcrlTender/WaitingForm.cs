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
    // Окно ожидания завершения длительной операции
    public partial class WaitingForm : Form
    {
        private string error;
        private Action operation;

        public string Error
        {
            get
            {
                return error;
            }
        }

        public WaitingForm(string waitingText, Action operation)
        {
            InitializeComponent();
            label1.Text = waitingText;
            this.operation = operation;
        } 

        private void slowOperation_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                operation.Invoke();
            }
            catch(Exception ex)
            {
                error = ex.Message;
            }
        }

        private void slowOperation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void WaitingForm_Shown(object sender, EventArgs e)
        {
            slowOperation.RunWorkerAsync();
        }


    }
}
