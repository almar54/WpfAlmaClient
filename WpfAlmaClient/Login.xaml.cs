﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfAlmaClient.CrisisUnityService;

namespace WpfAlmaClient
{
    /// <summary>
    /// Interaction logic for Loginwnd.xaml
    /// </summary>
    public partial class Loginwnd : Window
    {
        private User user;
        private bool userNameOk, passOk;
        private UnityClient myService;
        public Loginwnd()
        {
            InitializeComponent();
            string path=Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf(@"\bin"));
            ImageManager.ImageDirectory = path+@"\Images\Posts\";
            userNameOk = passOk = false;
            this.myService = new UnityClient();
        }

        private void GoToRegister(object sender, RoutedEventArgs e)
        {
            Registerwnd register = new Registerwnd();
            this.Close();
            register.ShowDialog();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (!userNameOk || !passOk)
            {
                MessageBox.Show("There are errors\n Return to fix them");
                return;
            }
            user = new User { UserName = tbUsername.Text, Password = pbPass.Password };
            user = myService.Login(user);
            if (user == null)
            {
                MessageBox.Show("No user detected");
                this.DataContext = user = new User();
                return;
            }
            if (user.IsManager)
            {
                MessageBox.Show("Loginwnd manager Succesful");
            }
            else
            {
                MessageBox.Show("Regular user login");
            }
            tbUsername.Text = pbPass.Password = string.Empty;
            Homewnd homewnd = new Homewnd(user);
            this.Close();
            homewnd.ShowDialog();
        }

        private void tbUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidUserName valid = new ValidUserName();
            valid.Min = 5;
            valid.Max = 10;
            ValidationResult result = valid.Validate(tbUsername.Text, null);
            if (!result.IsValid)
            {
                tbUsername.BorderBrush = Brushes.Red;
                tbUsername.ToolTip = result.ErrorContent.ToString();
                userNameOk = false;
            }
            else
            {
                tbUsername.BorderBrush = Brushes.Black;
                tbUsername.ToolTip = null;
                userNameOk = true;
            }
        }

        private void pbPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidPassword valid = new ValidPassword();
            valid.Min = 6;
            valid.Max = 15;
            ValidationResult result = valid.Validate(pbPass.Password, null);
            if (!result.IsValid)
            {
                pbPass.BorderBrush = Brushes.Red;
                pbPass.ToolTip = result.ErrorContent.ToString();
                passOk = false;
            }
            else
            {
                pbPass.BorderBrush = Brushes.Transparent;
                pbPass.ToolTip = null;
                passOk = true;
            }
        }
    }
}
