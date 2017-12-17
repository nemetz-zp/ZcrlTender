using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TenderLibrary;

namespace ZcrlTender
{
    public partial class LoginForm : Form
    {
        private bool authorizationComplete;

        public bool AuthorizationComplete
        {
            get
            {
                return authorizationComplete;
            }
        }

        public LoginForm()
        {
            InitializeComponent();
            authorizationComplete = false;

            using(TenderContext tc = new TenderContext())
            {
                usersCBList.DataSource = tc.Users.Select(p => p.Name).ToList();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Enabled = label2.Enabled = usersCBList.Enabled = passwordTextBox.Enabled = !checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            if (checkBox1.Checked)
            {
                authorizationComplete = true;
                this.Close();
            }
            else
            {
                string selectedUser = usersCBList.SelectedItem as string;

                if (selectedUser == null)
                {
                    NotificationHelper.ShowError("Ви не вибрали користувача для входу в систему!");
                    return;
                }

                if (!checkBox1.Checked && string.IsNullOrWhiteSpace(passwordTextBox.Text))
                {
                    NotificationHelper.ShowError("Ви не ввели пароль!");
                    return;
                }

                if (UserSession.AuthorizeUser(selectedUser, passwordTextBox.Text))
                {
                    authorizationComplete = true;
                    this.Close();
                }
                else
                {
                    NotificationHelper.ShowError("Неправильний пароль! (перевірте мову на якій ви вводите)");
                    return;
                }
            }
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
                e.Handled = true;
            }
        }
    }
}
