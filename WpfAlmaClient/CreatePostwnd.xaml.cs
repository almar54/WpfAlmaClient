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
        private Post post;
        private User user;
        public CreatePostwnd(User user)
        {
            InitializeComponent();
            myService = new UnityClient();
            this.user = user;
            post = new Post();
            this.DataContext = post;
            cbxCities.ItemsSource = myService.GetAllCities();
            cbxCities.DisplayMemberPath = "Name";
            cbxEvent.ItemsSource = myService.GetAllEvents();
            cbxEvent.DisplayMemberPath = "Name";
            cbxCategories.ItemsSource = myService.GetAllCategories();
            cbxCategories.DisplayMemberPath = "Name";
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
                
            }
            else
            {
                tbPTitle.BorderBrush = Brushes.Black;
                tbPTitle.ToolTip = null;
                
            }
        }

        private void CreatePost_Click(object sender, RoutedEventArgs e)
        {
            if (tbPTitle.Text.Equals(string.Empty) || tbDescription.Text.Equals(string.Empty) || cbxCities.SelectedItem == null)
            {
                MessageBox.Show("You must fill all of the fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            post.User = user;
            post.City = cbxCities.SelectedItem as City;
            post.Category = cbxCategories.SelectedItem as Category;
            post.Event = cbxEvent.SelectedItem as Event;
            post.PostDate = (DateTime)DPpostDate.SelectedDate;
            post.PostTime = (DateTime)TPpostTime.SelectedTime;
            
            if (myService.InsertPost(post) != 1)
            {
                MessageBox.Show("Something is wrong...", "Oops", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            MessageBox.Show("All good! lets go!", "Thank You!", MessageBoxButton.OK);
            Userwnd userwnd = new Userwnd();
            this.Close();
            userwnd.ShowDialog();

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
                        imageControl.Width = 80; 
                        imageControl.Height = 80;

                        // Set some margin to separate images
                        imageControl.Margin = new Thickness(5);
                        imagesContainer.Children.Add(imageControl);

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
