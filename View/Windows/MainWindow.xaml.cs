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

namespace CRMTelmate.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            FrameMain.Navigate(new Pages.ClientPage());
            UserName.Text = App.CurrentUser.SurnameUser.ToString() +"\n"+ App.CurrentUser.NameUser.ToString() +"\n"+ App.CurrentUser.PatronumicUser.ToString();
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.ClientPage());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrameMain.CanGoBack)
            {
                FrameMain.GoBack();
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            App.ResetCurrentUser();
            Authorization Authorization = new Authorization();
            this.Close();
            Authorization.Show();
        }

        private void BtnServices_Click(object sender, RoutedEventArgs e)
        {
            FrameMain.Navigate(new Pages.ServicePage());
        }
    }
}
