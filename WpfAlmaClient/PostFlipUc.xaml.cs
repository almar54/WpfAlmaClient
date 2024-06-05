using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAlmaClient.CrisisUnityService;

namespace WpfAlmaClient
{
    /// <summary>
    /// Interaction logic for PostFlipUc.xaml
    /// </summary>
    public partial class PostFlipUc : UserControl
    {
        private Post post;
        private string[] images;
        public PostFlipUc(Post post)
        {
            InitializeComponent();
            this.post = post;
            postTitle.Text = $"Title: {post.Title} | Event: {post.Event.Name} | Category: {post.Category.Name} | City: {post.City.Name}";
            postDes.Text = post.Description;
            images = ImageManager.GetAllPostImages(post);
            if (post.User.PhoneNum == "0000000000")
            {
                contactTxt.Text = "No contact details found";
            }
            else
            {
                contactTxt.Text += post.User.PhoneNum;
            }
            LoadImages();
        }

        private void LoadImages()
        {
            if (images == null)
            {
                imgtb.Text = "No images";
                return;
            }
            foreach (string file in images)
            {
                try
                {
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(file));
                    image.Margin = new Thickness(4);
                    image.Width = 30;
                    image.Height = 30;
                    image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
                    imageGrid.Children.Add(image);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                if (image.Width == 30)
                {
                    image.Width = 120;
                    image.Height = 120;
                }
                else
                {
                    image.Width = 30;
                    image.Height = 30;
                }
            }
        }
    }
}
