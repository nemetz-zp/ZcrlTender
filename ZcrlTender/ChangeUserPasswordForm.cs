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
    public partial class ChangeUserPasswordForm : Form
    {
        public ChangeUserPasswordForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text))
            {
                NotificationHelper.ShowError("Ви не вказали старий пароль!");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                NotificationHelper.ShowError("Ви не вказали новий пароль!");
                return;
            }

            if (!textBox2.Text.Equals(textBox3.Text))
            {
                NotificationHelper.ShowError("Новий пароль та підтверджений не співпадають!");
                return;
            }

            if(UserSession.ChangeUserPassword(UserSession.AuthenticatedUsername, textBox1.Text, textBox2.Text))
            {
                NotificationHelper.ShowInfo("Пароль успішно змінено!");
                this.Close();
            }
            else
            {
                NotificationHelper.ShowError("Помилка зміни паролю! Перевірте правильність старого паролю!");
                return;
            }
        }
    }
}
