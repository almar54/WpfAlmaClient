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
using System.IO;

namespace WpfAlmaClient
{
    /// <summary>
    /// Interaction logic for CreatePostwnd.xaml
    /// </summary>
    public partial class CreatePostwnd : Window
    {
        private UnityClient myService;
        private List<string> files;
        private Post post;
        private User user;
        public CreatePostwnd(User user)
        {
            InitializeComponent();
            myService = new UnityClient();
            this.user = user;
            post = new Post();
            this.DataContext = post;
            files = new List<string>();
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
            if (!(bool)contactTgl.IsChecked)
            {
                post.User.PhoneNum = null;
            }
            int id = myService.InsertPost(post);
            if (id==-1)
            {
                MessageBox.Show("Something is wrong...", "Oops", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            //יצירת תיקייה
            post.ID = id;
            string folderName = id.ToString();
            //string postsDirectoryPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Posts");
            try
            {
                //if (!Directory.Exists(postsDirectoryPath))
                //        Directory.CreateDirectory(postsDirectoryPath);

                //    postsDirectoryPath = System.IO.Path.Combine(postsDirectoryPath, folderName);

                //    if (!Directory.Exists(postsDirectoryPath))
                //        Directory.CreateDirectory(postsDirectoryPath);

                //move files to new folder
                foreach (string file in files)
                {
                    //string name = file.Substring(file.LastIndexOf(@"\"));
                    //System.IO.File.Copy(file, postsDirectoryPath + name, true);
                    string name = ImageManager.SaveImageToClient(file, folderName);
                    ImageManager.SendImageToService(folderName+"\\"+name);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //שמירת כל התמונות בתיקייה נוכחית

            //שמירת כל התמונות בשירות
            MessageBox.Show("All good! lets go!", "Thank You!", MessageBoxButton.OK);
            Userwnd userwnd = new Userwnd(user);
            this.Close();
            userwnd.ShowDialog();

        }

        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            string[] images = ImageManager.Image_Dialog();

            if (images.Length>0)
            {
                try
                {
                    foreach (string selectedFileName in images)
                    {
                        // Load and display the saved image
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(selectedFileName);
                        bitmapImage.EndInit();

                        // Create a new Image control to display the selected image
                        Image imageControl = new Image();
                        imageControl.Source = bitmapImage;
                        imageControl.Width = 80; 
                        imageControl.Height = 80;
                        imageControl.Tag = selectedFileName;
                        imageControl.MouseLeftButtonDown += RemoveImage_MouseLeftButtonDown; 

                        // Set some margin to separate images
                        imageControl.Margin = new Thickness(5);
                        imagesContainer.Children.Add(imageControl);
                        files.Add(selectedFileName);
                    }
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving image: " + ex.Message);
                }
            }
        }

        private void RemoveImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image imageControl = sender as Image;
            string file = imageControl.Tag.ToString();
            files.Remove(file);
            imagesContainer.Children.Remove(imageControl);
        }
    }
}
