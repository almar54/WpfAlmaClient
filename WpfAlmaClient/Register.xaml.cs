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
    /// Interaction logic for Registerwnd.xaml
    /// </summary>
    public partial class Registerwnd : Window
    {
        private UnityClient myService;
        private User user;
        private bool pass, rePass;
        public Registerwnd()
        {
            InitializeComponent();
            myService = new UnityClient();
            user = new User();
            mainGrid.DataContext = user;
            cbxCities.ItemsSource = myService.GetAllCities();
            cbxCities.DisplayMemberPath = "Name";
            pass = rePass = false;
            tbUserName.Focus();

        }

        private void GoToLogIn(object sender, RoutedEventArgs e)
        {
            Loginwnd login = new Loginwnd();
            this.Close();
            login.ShowDialog();

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckData())
            {
                dhErrors.IsOpen = true;
            }
            //username exists?
            if (!myService.IsUserNameFree(tbUserName.Text))
            {
                tbDialogText.Text = "Username is already used";
                dhErrors.IsOpen = true;
                return;
            }
            user.Password = pbPassword.Password;
            user.City = cbxCities.SelectedItem as City;
            user.IsManager = false;
            user.PhoneNum = tbPhone.Text;
            if (myService.InsertUser(user) != 1)
            {
                tbDialogText.Text = "Something Is Wrong";
                dhErrors.IsOpen = true;
                return;
            }
            tbDialogText.Text = "Thank you!";
            dhErrors.IsOpen = true;
            user=myService.Login(user);
            Homewnd homewnd = new Homewnd(user);
            this.Close();
            homewnd.ShowDialog();
        }
        private bool CheckData()
        {
            if (tbUserName.Text.Equals(string.Empty)) return false;
            if (tbFirstName.Text.Equals(string.Empty)) return false;
            if (tbLastName.Text.Equals(string.Empty)) return false;
            if (tbPhone.Text.Equals(string.Empty)) return false;
            if (tbEmail.Text.Equals(string.Empty)) return false;
            if (pbPassword.Password.Equals(string.Empty)) return false;
            if (pbRePassword.Password.Equals(string.Empty)) return false;
            if (Validation.GetHasError(tbUserName)) return false;
            if (Validation.GetHasError(tbFirstName)) return false;
            if (Validation.GetHasError(tbLastName)) return false;
            if (Validation.GetHasError(tbPhone)) return false;
            if (Validation.GetHasError(tbEmail)) return false;
            if (cbxCities.SelectedIndex == -1) return false;
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

        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            dhErrors.IsOpen = false;
            if (tbDialogText.Text.Equals("Thank you!"))
                this.Close();
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
