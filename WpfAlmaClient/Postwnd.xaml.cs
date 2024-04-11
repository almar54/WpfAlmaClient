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
    /// Interaction logic for Postwnd.xaml
    /// </summary>
    public partial class Postwnd : Window
    {
        private PostList posts;
        private User user;
        private UnityClient myService;
        public Postwnd(User user)
        {
            InitializeComponent();
            this.user = user;
            this.myService = new UnityClient();
            this.posts = myService.GetPostsByUserId(user.ID);
            AddPostCard();
            
        }
        private void AddPostCard()
        {
            if (posts.Count > 0)
            {
                tb1.Text = $"Post created by: {user.UserName}";
                foreach (Post p in posts)
                {
                    PostFlipUc postFlipUc = new PostFlipUc(p);
                    postFlipUc.Margin = new Thickness(5);
                    wpPosts.Children.Add(postFlipUc);
                }
            }
            else
            {
                tb1.Text = "You have no posts, let's create a new one!";
            }
        }

        private void homeDr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Homewnd homewnd = new Homewnd(user);
            this.Close();
            homewnd.ShowDialog();
        }

        private void categoryDr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Categorywnd categorywnd = new Categorywnd(user);
            this.Close();
            categorywnd.ShowDialog();
        }

        private void eventDr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Eventwnd eventwnd = new Eventwnd(user);
            this.Close();
            eventwnd.ShowDialog();
        }

        private void newPostbtn_Click(object sender, RoutedEventArgs e)
        {
            CreatePostwnd createPostwnd = new CreatePostwnd(user);
            createPostwnd.ShowDialog();
        }
    }
}
