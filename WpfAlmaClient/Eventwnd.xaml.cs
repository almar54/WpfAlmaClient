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
    /// Interaction logic for Eventwnd.xaml
    /// </summary>
    public partial class Eventwnd : Window
    {
        private User user;
        private UnityClient myService;
        private EventList events;
        private EventsUc eventsUc;
        public Eventwnd(User user)
        {
            InitializeComponent();
            this.user = user;
            this.myService = new UnityClient();
            this.events = myService.GetAllEvents();
            this.eventsUc = new EventsUc();
            spEvents.Children.Add(eventsUc);
            if (!user.IsManager)
            {
                spEvents.Children.Clear();
                TextBlock tbBye = new TextBlock();
                tbBye.Text = "You are not Manager, Go Back";
                tbBye.FontSize = 50;
                spEvents.Children.Add(tbBye);
            }
        }
        private void homeDr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Homewnd homewnd = new Homewnd(user);
            this.Close();
            homewnd.ShowDialog();
        }
        private void deleteEvent_Click(object sender, RoutedEventArgs e)
        {
            foreach (Event ev in events)
            {
                if (ev.Name == EventDel.Text)
                {
                    myService.DeleteEvent(ev);
                    events = myService.GetAllEvents();
                    EventDel.Text = "";
                    eventsUc.eventsLv.ItemsSource = events;
                    return;
                }
            }
        }

        private void eventbtn_Click(object sender, RoutedEventArgs e)
        {
            CreateEventwnd create = new CreateEventwnd(user);
            create.ShowDialog();
        }
    }
}
