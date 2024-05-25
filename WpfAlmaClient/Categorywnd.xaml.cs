using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for Categorywnd.xaml
    /// </summary>
    public partial class Categorywnd : Window
    {
        private User user;
        private UnityClient myService;
        private CategoryList categories;
        private TextBox catDel;
        private Button del;
        public Categorywnd(User user)
        {
            InitializeComponent();
            this.user = user;
            this.myService = new UnityClient();
            tb1.Text += user.UserName;
            this.categories = myService.GetAllCategories();
            this.catDel = new TextBox();
            this.del = new Button();
            LoadCategoryCard(categories);
            AdminControls();
        }
        private void LoadCategoryCard(CategoryList list)
        {
            foreach (Category c in list)
            {
                Categoryuc categoryuc = new Categoryuc(c);
                categoryuc.Margin = new Thickness(5);
                spCtg.Children.Add(categoryuc);

            }
        }
        private void PostView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Postwnd postwnd = new Postwnd(user);
            this.Close();
            postwnd.ShowDialog();
        }

        private void Home_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Homewnd homewnd = new Homewnd(user);
            this.Close();
            homewnd.ShowDialog();
        }

        private void Event_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Eventwnd eventwnd = new Eventwnd(user);
            this.Close();
            eventwnd.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateCategorywnd create = new CreateCategorywnd(user);
            create.ShowDialog();
        }
        private void AdminControls()
        {
            if (user.IsManager)
            {
                HintAssist.SetHelperText(catDel, "Enter category to delete");
                catDel.Margin = new Thickness(5, 20, 0, 10);
                catDel.Width = 250;
                catDel.HorizontalAlignment = HorizontalAlignment.Left;
                del.Content = "Delete";
                del.Click += Del_Click;
                del.FontFamily = new FontFamily("Aharoni");
                del.FontSize = 20;
                del.HorizontalAlignment = HorizontalAlignment.Left;
                del.Foreground = Brushes.Gray;
                del.Margin = new Thickness(10, 10, 10, 10);
                spMain.Children.Add(catDel);
                spMain.Children.Add(del);
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            foreach (Category c in categories)
            {
                if (c.Name == catDel.Text)
                {
                    myService.DeleteCategory(c);
                    categories = myService.GetAllCategories();
                    catDel.Text = "";
                    LoadCategoryCard(categories);
                    return;
                }
            }
            MessageBox.Show("Category doesnt exist.", "Error", MessageBoxButton.OK);
            return;
        }
    }
}
