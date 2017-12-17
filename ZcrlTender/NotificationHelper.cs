using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TenderLibrary;

namespace ZcrlTender
{
    public class NotificationHelper
    {
        public static void ShowError(string msg)
        {
            MessageBox.Show(msg, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInfo(string msg)
        {
            MessageBox.Show(msg, "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool ShowYesNoQuestion(string msg)
        {
            DialogResult res = MessageBox.Show(msg, "Підтвердження дії", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return res == DialogResult.Yes;
        }

        public static void ShowException(Exception ex)
        {
            ShowError(string.Format("Помилка: {0}\nStack Trace:\n{1}", ex.Message, ex.StackTrace));
        }
    }
}
