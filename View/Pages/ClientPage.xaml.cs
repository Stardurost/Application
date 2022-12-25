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

namespace CRMTelmate.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var currentClient = button.DataContext as Entities.Client;

            var result = MessageBox.Show(
                $"После удаления клиент будет больше недоступен. Удалить клиента {currentClient.FullName}?",
                "Подтверждение удаления",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.OK)
            {
                App.Context.Clients.Remove(currentClient);

                App.Context.SaveChanges();

                ComputeClients();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            //это нерабочий примерный код навигации на страницу определенного клиента
            //var button = sender as Button;
            //var currentClient = button.DataContext as Entities.Clients;

            //NavigationService.Navigate(new ClientInfoPage(currentClient));
        }
    }
}
