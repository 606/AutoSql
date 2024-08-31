using System;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using AutoSql.Consts;
using AutoSql.Helpers;
using AutoSql.Services;

namespace AutoSql.Forms
{
    public partial class LoginForm : Form
    {
        private readonly AuthorizationService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthorizationService();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (_authService.Login(username, password))
            {
                //MessageHelper.ShowSuccess(AppMessages.LoginSuccess);
                this.Hide();
                var mainForm = new MainForm(); // Перемикаємося на основну форму
                mainForm.Show();
            }
            else
            {
                MessageHelper.ShowError(AppMessages.InvalidUsernameOrPassword);
            }
        }
    }
}