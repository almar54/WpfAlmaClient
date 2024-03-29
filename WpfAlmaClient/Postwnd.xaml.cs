﻿using System;
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
            this.user = user;
            this.posts = myService.GetPostsByUserId(user.ID);
            InitializeComponent();
        }

        private void homeDr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Homewnd homewnd = new Homewnd(user);
            this.Close();
            homewnd.ShowDialog();
        }

        private void categoryDr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void eventDr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
