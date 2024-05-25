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
    /// Interaction logic for CreateCategorywnd.xaml
    /// </summary>
    public partial class CreateCategorywnd : Window
    {
        private UnityClient myService;
        private bool nameOk;
        private Category category;
        private User user;
        public CreateCategorywnd(User user)
        {
            InitializeComponent();
            myService = new UnityClient();
            this.user = user;
            nameOk = false;
            category = new Category();
        }

        private void tbCategoryName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidName valid = new ValidName();
            valid.min = 2;
            valid.max = 10;
            ValidationResult result = valid.Validate(tbCategoryName.Text, null);
            if (!result.IsValid)
            {
                tbCategoryName.BorderBrush = Brushes.Red;
                tbCategoryName.ToolTip = result.ErrorContent.ToString();
                nameOk = false;
            }
            else
            {
                tbCategoryName.BorderBrush = Brushes.Black;
                tbCategoryName.ToolTip = null;
                nameOk = true;
            }
        }

        private void AddCtg_Click(object sender, RoutedEventArgs e)
        {
            if (tbCategoryName.Text.Equals(string.Empty))
            {
                MessageBox.Show("You need to enter a Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (!myService.IsCtgNameFree(tbCategoryName.Text.Trim()))
            {
                MessageBox.Show("This category name already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            category.Name = tbCategoryName.Text.Trim();
            if (myService.InsertCategory(category) != 1)
            {
                MessageBox.Show("Something is wrong...", "Oops", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (!nameOk)
            {
                MessageBox.Show("You have errors, go back anf change them", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MessageBox.Show("All good! lets go!", "Thank You", MessageBoxButton.OK);
            this.Close();
        }
    }
}
