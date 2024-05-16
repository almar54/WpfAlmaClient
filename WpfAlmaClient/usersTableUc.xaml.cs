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
    /// Interaction logic for usersTableUc.xaml
    /// </summary>
    public partial class usersTableUc : UserControl
    {
        private UnityClient myService;
        private UserList users;
        public usersTableUc()
        {
            InitializeComponent();
            this.myService = new UnityClient();
            this.users = myService.GetAllUsers();
            usersLV.ItemsSource = users;
            
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            User user = (sender as CheckBox).Tag as User;
            myService.UpdateUser(user);
        }

    }
}
