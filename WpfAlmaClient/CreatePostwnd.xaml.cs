using Microsoft.Win32;
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
    /// Interaction logic for CreatePostwnd.xaml
    /// </summary>
    public partial class CreatePostwnd : Window
    {
        private UnityClient myService;
        private bool nameOk;
        private Post post;
        public CreatePostwnd()
        {
            InitializeComponent();
            myService = new UnityClient();
            nameOk = false;
            post = new Post();
            cbxCities.ItemsSource = myService.GetAllCities();
            cbxCities.DisplayMemberPath = "Name";
        }

        private void tbPTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidName valid = new ValidName();
            valid.min = 2;
            valid.max = 10;
            ValidationResult result = valid.Validate(tbPTitle.Text.Trim(), null);
            if (!result.IsValid)
            {
                tbPTitle.BorderBrush = Brushes.Red;
                tbPTitle.ToolTip = result.ErrorContent.ToString();
                nameOk = false;
            }
            else
            {
                tbPTitle.BorderBrush = Brushes.Black;
                tbPTitle.ToolTip = null;
                nameOk = true;
            }
        }

        private void CreatePost_Click(object sender, RoutedEventArgs e)
        {
            if (tbPTitle.Text.Trim().Equals(string.Empty) || tbDescription.Text.Trim().Equals(string.Empty) || cbxCities.SelectedItem == null)
            {

                return;
            }
        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            string destinationFolder = @"C:\Users\User\source\repos\WpfAlmaClient\WpfAlmaClient\Images\Posts\Post1\";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif)|*.jpg; *.jpeg; *.png; *.gif";
            openFileDialog.Multiselect = true;


            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    foreach (string selectedFileName in openFileDialog.FileNames)
                    {
                        // Construct the destination file path by combining the destination folder and the selected file name
                        string destinationPath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(selectedFileName));

                        // Copy the selected image file to the destination folder
                        System.IO.File.Copy(selectedFileName, destinationPath, true);

                        // Load and display the saved image
                        BitmapImage bitmapImage = new BitmapImage(new Uri(destinationPath));

                        // Create a new Image control to display the selected image
                        Image imageControl = new Image();
                        imageControl.Source = bitmapImage;

                        // Set some margin to separate images
                        imageControl.Margin = new Thickness(5);

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving image: " + ex.Message);
                }
            }
        }

    }
}
