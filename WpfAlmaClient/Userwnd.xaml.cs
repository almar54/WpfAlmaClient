using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Userwnd.xaml
    /// </summary>
    public partial class Userwnd : Window
    {
        private User user;
        private UnityClient myService;
        private bool pass, rePass;
        public Userwnd(User user)
        {
            InitializeComponent();
            this.user = user;
            this.myService = new UnityClient();
            tbName.Text += user.UserName;
            pass = rePass = false;
        }

        private void homeDr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Homewnd homewnd = new Homewnd(user);
            this.Close();
            homewnd.ShowDialog();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData())
            {
                user.UserName = tbUsername.Text;
                user.Email = tbEmail.Text;
                user.PhoneNum = tbPhone.Text;
                user.Password = pbPassword.Password.ToString();
                myService.UpdateUser(user);
            }
            else
            {
                MessageBox.Show("You have errors, go back and change them", "Error", MessageBoxButton.OK);
            }
        }
        private bool CheckData()
        {
            if (tbUsername.Text.Equals(string.Empty)) return false;
            if (tbEmail.Text.Equals(string.Empty)) return false;
            if (tbPhone.Text.Equals(string.Empty)) return false;
            if (pbPassword.Password.Equals(string.Empty)) return false;
            if (pbRePassword.Password.Equals(string.Empty)) return false;
            if (Validation.GetHasError(tbUsername)) return false;
            if (Validation.GetHasError(tbEmail)) return false;
            if (Validation.GetHasError(tbPhone)) return false;
            if (!pass) return false;
            if (!rePass) return false;
            return true;
        }
        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidPassword valid = new ValidPassword();
            valid.Max = 15;
            valid.Min = 6;
            ValidationResult result = valid.Validate(pbPassword.Password, null);
            if (result.IsValid)
            {
                pbPassword.BorderBrush = Brushes.Gray;
                HintAssist.SetHelperText(pbPassword, "Password");
            }
            else
            {
                pbPassword.BorderBrush = Brushes.Red;
                HintAssist.SetHelperText(pbPassword, result.ErrorContent.ToString());
            }
        }
        private void pbRePassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pbRePassword.Password.Equals(pbPassword.Password))
            {
                pbRePassword.BorderBrush = Brushes.Gray;
                HintAssist.SetHelperText(pbRePassword, "Passwords match!");
            }
            else
            {
                pbRePassword.BorderBrush = Brushes.Red;
                HintAssist.SetHelperText(pbRePassword, "Passwords do not match");
            }

        }
    }
}
