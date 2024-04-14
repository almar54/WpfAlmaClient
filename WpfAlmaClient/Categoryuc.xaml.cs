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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAlmaClient.CrisisUnityService;

namespace WpfAlmaClient
{
    /// <summary>
    /// Interaction logic for Categoryuc.xaml
    /// </summary>
    public partial class Categoryuc : UserControl
    {
        private Category category;
        private UnityClient myService;
        private PostList posts;
        public Categoryuc(Category category)
        {
            InitializeComponent();
            this.category = category;
            this.myService = new UnityClient();
            posts = myService.GetPostsByDate(DateTime.Today.AddDays(-240));
            LoadPostCard(posts);
            ctgName.Text = category.Name;
        }
        private void LoadPostCard(PostList list)
        {
            foreach (Post p in list)
            {
                if (p.Category.ID.Equals(category.ID))
                {
                    PostFlipUc postFlipUc = new PostFlipUc(p);
                    postFlipUc.Margin = new Thickness(5);
                    wpPosts.Children.Add(postFlipUc);
                }
            }
        }
    }
}
