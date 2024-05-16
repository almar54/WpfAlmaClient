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
    /// Interaction logic for UsersWnd.xaml
    /// </summary>
    public partial class UsersWnd : Window
    {
        private User user;
        private UnityClient myService;

        public UsersWnd(User user)
        {
            InitializeComponent();
            this.user = user;
            myService = new UnityClient();
            usersTableUc tableUc = new usersTableUc();
            spTable.Children.Add(tableUc);
            
        }

        private void homeDr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Homewnd homewnd = new Homewnd(user);
            this.Close();
            homewnd.ShowDialog();
        }

        private void updateCities_Click(object sender, RoutedEventArgs e)
        {
            myService.UpdateCitiesFromExternalData();
        }

        private void deleteUser_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
