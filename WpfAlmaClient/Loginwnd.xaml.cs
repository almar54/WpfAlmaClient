using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private UnityClient client;
        public Loginwnd()
        {
            InitializeComponent();
            client = new UnityClient();
            userNameOk = passOk = false;
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
            user = client.Login(user);
            if (user == null)
            {
                MessageBox.Show("No user detected");
                this.DataContext = user = new User();
                return;
            }
            if (user.IsManager)
            {
                MessageBox.Show("Loginwnd Succesful");
                MainWindow mw = new MainWindow(user);
                mw.ShowDialog();
            }
            else
            {
                MessageBox.Show("Regular user login");
            }
            tbUsername.Text = pbPass.Password = string.Empty;
            Userwnd userwnd = new Userwnd();
            this.Close();
            userwnd.ShowDialog();
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
