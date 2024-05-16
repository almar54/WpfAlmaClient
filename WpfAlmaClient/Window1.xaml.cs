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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            ImageManager.ImageDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Posts");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UnityClient x = new UnityClient();
            User user = x.Login(new User { UserName = "alma22", Password = "Alma22#", PhoneNum = "0584942153" });
            UsersWnd usersWnd = new UsersWnd(user);
            this.Close();
            usersWnd.ShowDialog();
        }
    }
}
