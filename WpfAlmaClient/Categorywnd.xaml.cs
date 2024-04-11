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
        public Categorywnd(User user )
        {
            InitializeComponent();
            this.user = user;
            this.myService = new UnityClient();
            tb1.Text += user.UserName;
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
    }
}
