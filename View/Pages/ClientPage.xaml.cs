using System;
using System.Collections.Generic;
using System.Globalization;
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
        public static CultureInfo cultureInfoRu = CultureInfo.GetCultureInfo("ru-RU");

        static List<string> SortOptions = new List<string>()
        {
            "По умолчанию",
            "Сначала старые клиенты",
            "Сначала новые клиенты"
        };
        static List<string> FilterOptions = new List<string>()
        {
            "По умолчанию",
            $"Сумма трат от {(1000).ToString("C", cultureInfoRu)}",
            $"Сумма трат от {(5000).ToString("C", cultureInfoRu)}",
            $"Сумма трат от {(10000).ToString("C", cultureInfoRu)}"
        };

        public ClientPage()
        {
            InitializeComponent();

            InitializeSort();
            InitializeFilter();
            InitializeClients();
        }

        private void InitializeSort()
        {
            CBSort.ItemsSource = SortOptions;
            CBSort.SelectedIndex = 0;
        }

        private void InitializeFilter()
        {
            CBFilter.ItemsSource = FilterOptions;
            CBFilter.SelectedIndex = 0;
        }

        private void InitializeClients()
        {
            ComputeClients();
        }

        private void ComputeClients()
        {
            var clients = App.Context.Clients.ToList();
            var clientsSorted = Sort(clients);
            var clientsFiltered = Filter(clientsSorted);
            var clientsSearched = Search(clientsFiltered);

            LViewClients.ItemsSource = clientsSearched;
        }

        private List<Entities.Client> Sort(List<Entities.Client> clients)
        {
            var sortOptionIndex = CBSort.SelectedIndex;
            List<Entities.Client> clientsSorted;

            switch (sortOptionIndex)
            {
                case 1:
                    clientsSorted = clients.OrderBy(c => c.RegistrationDate)
                        .ToList();
                    break;
                case 2:
                    clientsSorted = clients.OrderByDescending(c => c.RegistrationDate)
                        .ToList();
                    break;
                case 0:
                default:
                    clientsSorted = clients;
                    break;
            }

            return clientsSorted;
        }

        private List<Entities.Client> Filter(List<Entities.Client> clients)
        {
            var filterOptionIndex = CBFilter.SelectedIndex;
            List<Entities.Client> clientsFiltered;

            switch (filterOptionIndex)
            {
                case 1:
                    clientsFiltered = clients.Where(c => c.CostSum >= 1000)
                        .ToList();
                    break;
                case 2:
                    clientsFiltered = clients.Where(c => c.CostSum >= 5000)
                        .ToList();
                    break;
                case 3:
                    clientsFiltered = clients.Where(c => c.CostSum >= 10000)
                        .ToList();
                    break;
                case 0:
                default:
                    clientsFiltered = clients;
                    break;
            }

            return clientsFiltered;
        }

        private List<Entities.Client> Search(List<Entities.Client> clients)
        {
            var searchText = TBSearch.Text.ToLower();

            return clients
                .Where(
                    c => c.FullName.ToLower()
                    .Contains(searchText)
                )
                .ToList();
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
            var button = sender as Button;
            var currentClient = button.DataContext as Entities.Client;

            NavigationService.Navigate(new ClientInfoPage(currentClient));
        }

        private void CBSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComputeClients();
        }

        private void CBFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComputeClients();
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComputeClients();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ComputeClients();
        }
    }
}
