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
    /// Interaction logic for Homewnd.xaml
    /// </summary>
    public partial class Homewnd : Window
    {
        private User user;
        private UnityClient unityService;
        private EventList events;
        private CategoryList categories;
        private PostList posts;
        public Homewnd(User user)
        {
            InitializeComponent();
            this.user = user;
            unityService = new UnityClient();
            posts = unityService.GetPostsByDate(DateTime.Today.AddDays(-180));
            categories = unityService.GetAllCategories();
            events = unityService.GetAllEvents();
            tbUserName.Text = "Welcome! " + user.UserName;
            AddexCategory();
            AddexEvent();
            AddPostCard();
        }
        private void AddexEvent()
        {
            Expander expander = new Expander();
            expander.Header = "Events";
            StackPanel items = new StackPanel();
            foreach (Event e in events)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = e.Name;
                checkBox.Tag = e;
                checkBox.Checked += CheckBoxEvent_Checked;
                items.Children.Add(checkBox);
            }
            expander.Content = items;
            spExpanders.Children.Add(expander);
        }
        private void AddPostCard()
        {
            foreach (Post p in posts)
            {
                PostFlipUc postFlipUc = new PostFlipUc(p);
                wpPosts.Children.Add(postFlipUc);
            }
        }

        private void CheckBoxEvent_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Event mye = checkBox.Tag as Event;

        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddexCategory()
        {
            Expander exCategories = new Expander();
            exCategories.Header = "Categories";
            StackPanel spExpander = new StackPanel();
            foreach (Category cat in categories)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = cat.Name;
                checkBox.Tag = cat;
                checkBox.Checked += CheckBoxCategory_Checked;
                spExpander.Children.Add(checkBox);
            }
            exCategories.Content = spExpander;
            spExpanders.Children.Add(exCategories);
        }

        private void CheckBoxCategory_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Category category = checkBox.Tag as Category;
        }
    }
}
