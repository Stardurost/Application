using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

namespace CRMTelmate.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        List<Entities.Service> services = new List<Entities.Service>();
        public ServicePage()
        {
            InitializeComponent();
            LViewService.ItemsSource = App.Context.Services.ToList();
            InitializeSort();
            InitializeFilter();
            InitializeClients();
            services = App.Context.Services.ToList();
        }

        static List<string> SortOptions = new List<string>()
        {
            "По умолчанию",
            "По возрастанию цены",
            "По убыванию цены"
        };
     

        private void InitializeSort()
        {
            SortCB.ItemsSource = SortOptions;
            SortCB.SelectedIndex = 0;
        }

        private void InitializeFilter()
        { 
            var ListTypes = App.Context.TypeServices.ToArray();
            var count = ListTypes.Count()+1;
            string[] arrItems = new string[count];
            arrItems[0] = "Все категории";
            int index = 1;
            foreach (var Type in ListTypes)
            {
                arrItems[index] = Type.TypeName;
                index++;
            }
            FilterCB.ItemsSource = arrItems.ToList();
            FilterCB.SelectedIndex = 0;
        }

        private void InitializeClients()
        {
            ComputeService();
        }
       

        private void UpdateServices()
        {
            var services = App.Context.Services.ToList();
            services = services.Where(p => p.NameService.ToLower()
                     .Contains(TBSearch.Text.ToLower())).ToList();

            LViewService.ItemsSource = services;
        }

        private void ComputeService()
        {
            var services = App.Context.Services.ToList();
            var servocesSort = Sort(services);
            var servicesFiltr = Filter(servocesSort);
            var servicesSearch = Search(servicesFiltr);

            LViewService.ItemsSource = servicesSearch;
        }

        private List<Entities.Service> Sort(List<Entities.Service> clients)
        {
            var sortOptionIndex = SortCB.SelectedIndex;
            List<Entities.Service> servicesSort;

            switch (sortOptionIndex)
            {
                case 1:
                    servicesSort = clients.OrderBy(c => c.Cost)
                        .ToList();
                    break;
                case 2:
                    servicesSort = clients.OrderByDescending(c => c.Cost)
                        .ToList();
                    break;
                case 0:
                default:
                    servicesSort = clients;
                    break;
            }

            return servicesSort;
        }

        private List<Entities.Service> Filter(List<Entities.Service> clients)
        {
            var filterOptionIndex = FilterCB.SelectedIndex;
            List<Entities.Service> servicesFiltr;

            switch (filterOptionIndex)
            {
                case 1:
                    servicesFiltr = clients.Where(c => c.TypeService == 1)
                        .ToList();
                    break;
                case 2:
                    servicesFiltr = clients.Where(c => c.TypeService == 2)
                        .ToList();
                    break;
                case 3:
                    servicesFiltr = clients.Where(c => c.TypeService == 3)
                        .ToList();
                    break;
                case 4:
                    servicesFiltr = clients.Where(c => c.TypeService == 4)
                        .ToList();
                    break;
                case 0:
                default:
                    servicesFiltr = clients;
                    break;
            }

            return servicesFiltr;
        }

        private List<Entities.Service> Search(List<Entities.Service> services)
        {
            var searchText = TBSearch.Text.ToLower();

            return services
                .Where(
                    c => c.NameService.ToLower()
                    .Contains(searchText)
                )
                .ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComputeService();
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComputeService();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var currentService = button.DataContext as Entities.Service;

            NavigationService.Navigate(new ServiceInfoPage(currentService));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentService = (sender as Button).DataContext as Entities.Service;
            if (MessageBox.Show($"Вы уверены, что хотите удалить товар: {currentService.NameService}?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DeleteItem(currentService);
                UpdateServices();
            }
        }
       private void DeleteItem(Entities.Service changingProduct)
        {
            var itemToClear = App.Context.Services.Where(p => p.IDService == changingProduct.IDService)
                    .FirstOrDefault();
            App.Context.Services.Remove(itemToClear);
            try
            {
                App.Context.SaveChanges();
            }
            catch
            { 
                MessageBox.Show("Невозможно удалить. Данная услуга используется клиентом."); 
            }
        }
    }
}
