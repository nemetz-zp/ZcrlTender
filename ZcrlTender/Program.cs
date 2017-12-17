using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZcrlTender
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm lf = null;
            Application.Run(lf = new LoginForm());

            if(!lf.AuthorizationComplete)
            {
                return;
            }

            TenderYearForm tf = null;
            Application.Run(tf = new TenderYearForm());

            if (tf.SelectedYear == null)
            {
                return;
            }
            else
            {
                MainProgramForm.CurrentTenderYear = tf.SelectedYear;
            }

            try
            {
                Application.Run(new MainProgramForm());
            }
            catch(Exception ex)
            {
                NotificationHelper.ShowException(ex);
            }
        }
    }
}
