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
    /// Interaction logic for CreateEventwnd.xaml
    /// </summary>
    public partial class CreateEventwnd : Window
    {
        private UnityClient myService;
        private bool nameOk;
        private Event @event;
        private User user;
        public CreateEventwnd(User user)
        {
            InitializeComponent();
            this.user = user;
            myService = new UnityClient();
            nameOk = false;
            @event = new Event();
        }

        private void tbEventName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidName valid = new ValidName();
            valid.min = 2;
            valid.max = 10;
            ValidationResult result = valid.Validate(tbEventName.Text.Trim(), null);
            if (!result.IsValid)
            {
                tbEventName.BorderBrush = Brushes.Red;
                tbEventName.ToolTip = result.ErrorContent.ToString();
                nameOk = false;
            }
            else
            {
                tbEventName.BorderBrush = Brushes.Black;
                tbEventName.ToolTip = null;
                nameOk = true;
            }
        }

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (tbEventName.Text.Trim().Equals(string.Empty) || cbxSeverity.SelectedItem.Equals(null))
            {
                MessageBox.Show("You need to enter a Name AND severity", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!myService.IsEventNameFree(tbEventName.Text.Trim()))
            {
                MessageBox.Show("This category name already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            @event.Name = tbEventName.Text.Trim();
            @event.Severity = int.Parse(cbxSeverity.SelectionBoxItem.ToString());
            if (myService.InsertEvent(@event) != 1)
            {
                MessageBox.Show("Something is wrong...", "Oops", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (!nameOk)
            {
                MessageBox.Show("You have errors, go back and change them", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("All good! lets go!", "Thank You", MessageBoxButton.OK);
            Userwnd userwnd = new Userwnd(user);
            this.Close();
            userwnd.ShowDialog();
        }
    }
}
