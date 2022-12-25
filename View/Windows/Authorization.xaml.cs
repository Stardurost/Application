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

namespace CRMTelmate.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            this.Close();
            MainWindow.Show();
        }

        private void Window_TouchEnter(object sender, TouchEventArgs e)
        {
            var currentUser = App.Context.Users
                .FirstOrDefault(p => p.UserLogin == TextBoxLogin.Text
                && p.UserPassword == PswdBoxLogin.Password);
            if (currentUser != null)
            {
                App.CurrentUser = currentUser;
                MainWindow MainWindow = new MainWindow();
                this.Close();
                MainWindow.Show();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!!!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
