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
        private List<CheckBox> eventsCheckBox;
        public Homewnd(User user)
        {
            InitializeComponent();
            this.user = user;
            unityService = new UnityClient();
            posts = unityService.GetPostsByDate(DateTime.Today.AddDays(-180));
            categories = unityService.GetAllCategories();
            events = unityService.GetAllEvents();
            tbUserName.Text = "Welcome! " + user.UserName;
            eventsCheckBox = new List<CheckBox>();
            AddexCategory();
            AddexEvent();
            LoadPostCard(posts);
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
                checkBox.Checked += CheckBox_Checked;
                checkBox.Unchecked += CheckBox_Checked;
                items.Children.Add(checkBox);
                eventsCheckBox.Add(checkBox);
            }
            expander.Content = items;
            spExpanders.Children.Add(expander);
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
                checkBox.Checked += CheckBox_Checked;
                checkBox.Unchecked += CheckBox_Checked;
                spExpander.Children.Add(checkBox);
                eventsCheckBox.Add(checkBox);
            }
            exCategories.Content = spExpander;
            spExpanders.Children.Add(exCategories);
        }
        private void LoadPostCard(PostList list)
        {
            foreach (Post p in list)
            {
                PostFlipUc postFlipUc = new PostFlipUc(p);
                postFlipUc.Margin = new Thickness(5);
                wpPosts.Children.Add(postFlipUc);
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PostList viewposts = new PostList();
            List<int> listEvent= new List<int>();
            List<int> listCat= new List<int>();
            foreach(CheckBox checkBox in eventsCheckBox)
                if((bool)checkBox.IsChecked)
                {
                    if(checkBox.Tag is Event)
                        listEvent.Add((checkBox.Tag as Event).ID);
                    else
                        listCat.Add((checkBox.Tag as Category).ID);
                }    
            wpPosts.Children.Clear();
            if (listCat.Count == 0 && listEvent.Count == 0) {
                LoadPostCard(posts);
                return;
            }
            foreach (Post p in posts)
            {
                if (listEvent.Count == 0 && (listCat.Count != 0 && listCat.Find(i => i == p.Category.ID) != 0))
                    viewposts.Add(p);
                else
                 if (listCat.Count == 0 && (listEvent.Count != 0 && listEvent.Find(i => i == p.Event.ID) != 0))
                    viewposts.Add(p);
                else
                if (listEvent.Find(i => i == p.Event.ID) != 0 && listCat.Find(i => i == p.Category.ID) != 0)
                {
                    viewposts.Add(p);
                }
            }
            LoadPostCard(viewposts);
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PostView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Postwnd postwnd = new Postwnd(user);
            this.Close();
            postwnd.ShowDialog();
        }

        private void Category_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Categorywnd categorywnd = new Categorywnd(user);
            this.Close();
            categorywnd.ShowDialog();
        }

        private void Event_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Eventwnd eventwnd = new Eventwnd(user);
            this.Close();
            eventwnd.ShowDialog();
        }

        private void userIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Userwnd userwnd = new Userwnd(user);
            ManagerWnd managerWnd = new ManagerWnd(user);
            this.Close();
            if (user.IsManager)
            {
                managerWnd.ShowDialog();
            }
            userwnd.ShowDialog();
            
        }        
    }
}
