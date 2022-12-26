using CRMTelmate.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using File = System.IO.File;

namespace CRMTelmate.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientInfoPage.xaml
    /// </summary>
    public partial class ClientInfoPage : Page
    {
        public static CultureInfo cultureInfoRu = CultureInfo.GetCultureInfo("ru-RU");
        private Entities.Client _client;
        List<Entities.Service> clientsUnFiltered = new List<Entities.Service>();
        private byte[] _mainImageData = null;
        public ClientInfoPage(Entities.Client client)
        {
            InitializeComponent();

            _client = client;

            InitializeServicesList();
            InitializeClientInfo();
            InitializeCostStats();
            UpdateServicesList();
            initalizeImage();
        }
        private void initalizeImage()
        {
            _mainImageData = _client.Image;
            ImageService.Source = new ImageSourceConverter()
                .ConvertFrom(_client.Image) as ImageSource;
        }
        private void InitializeServicesList()
        {
            UpdateServicesList();
        }

        private void UpdateServicesList()
        {
            var services = App.Context.Services.ToList();
            services = Filter(services);

            LViewServices.ItemsSource = null;
            LViewServices.ItemsSource = services;
            var itemsSource = clientsUnFiltered.ToList()
                .Select(s => s.NameService);
            CBAddClientService.ItemsSource = itemsSource;
            CBAddClientService.SelectedIndex = 0;
        }
        private List<Entities.Service> Filter(List<Entities.Service> services)
        {
            List<Entities.Service> clientsFiltered = new List<Entities.Service>();
            List<Entities.ClientService> clientService;
            clientService = App.Context.ClientServices.ToList();
            clientService = clientService.Where(p => p.IDClient.ToString() == _client.IDClient.ToString())
                        .ToList();
            int[] IDServices = new int[clientService.Count];
            int index = 0;
            foreach (var client in clientService)
            {
                IDServices[index] = client.IDService;
                index++;
            }
            foreach (var service in services)
            {
                if (InArray(service.IDService, IDServices)) clientsFiltered.Add(service);
            }
            clientsUnFiltered.Clear();
            foreach (var service in services)
            {
                if (!InArray(service.IDService, IDServices)) clientsUnFiltered.Add(service);
            }

            return clientsFiltered;
        }
        private bool InArray(int Id, int[] Arr)
        {
            bool result = false;
            foreach (var item in Arr)
            {
                if (item == Id)
                {
                    return true;
                }
            }
            return result;
        }

        private void InitializeClientInfo()
        {
            TBFirstName.Text = _client.NameClient;
            TBSurnameName.Text = _client.SurnameClient;
            TBPatronumicName.Text = _client.PatronumicClient;
            TBEmail.Text = _client.EmailClient;
            DPRegistrationDate.SelectedDate = _client.RegistrationDate;
            TBPhone.Text = _client.ProneClient;
        }

        private void InitializeCostStats()
        {
            UpdateCostStats();
        }

        private void UpdateCostStats()
        {
            var sixMonthAgo = DateTime.Today;
            if (sixMonthAgo.Month <= 5)
            {
                sixMonthAgo = sixMonthAgo
                    .AddYears(-1)
                    .AddMonths(5);
            } else
            {
                sixMonthAgo = sixMonthAgo.AddMonths(-5);
            }
            var startTimeMin = new DateTime(sixMonthAgo.Year, sixMonthAgo.Month, 1);

            var monthToCostSum = new Dictionary<string, decimal>();
            for (var monthsToAdd = 0; monthsToAdd < 6; monthsToAdd++)
            {
                var month = new DateTime(startTimeMin.Ticks);
                var monthToBe = month.Month + monthsToAdd;
                if (monthToBe > 12)
                {
                    var monthShouldBe = monthToBe - 12;
                    month = month
                        .AddYears(1)
                        .AddMonths(-month.Month + monthShouldBe);
                } else
                {
                    month = month.AddMonths(monthsToAdd);
                }

                var monthIdentifier = month.ToString("Y", cultureInfoRu);
                monthToCostSum[monthIdentifier] = 0;
            }

            var clientServicesOrdered = _client.ClientServices.OrderBy(cs => cs.StartTime);
            foreach (var cs in clientServicesOrdered)
            {
                if (cs.StartTime < startTimeMin) continue;

                var month = cs.StartTime.ToString("Y", cultureInfoRu);
                monthToCostSum[month] += cs.Service.Cost;
            }

            var itemsSource = new List<KeyValuePair<string, decimal>> { };
            foreach (var pair in monthToCostSum)
            {
                itemsSource.Add(
                    new KeyValuePair<string, decimal>(
                        pair.Key,
                        pair.Value
                    )
                );
            }

            var costByMonthSeries = (AreaSeries) CostStats.Series[0];
            costByMonthSeries.ItemsSource = itemsSource;
        }

        private void BtnDeleteSrvc_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var currentIDService = ((CRMTelmate.Entities.Service)button.DataContext).IDService;

            var currentClientService = App.Context.ClientServices.Where(p => p.IDService == currentIDService && p.IDClient == _client.IDClient).FirstOrDefault();

            App.Context.ClientServices.Remove(currentClientService);
            App.Context.SaveChanges();

            UpdateServicesList();
            UpdateCostStats();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var date = DPRegistrationDate.SelectedDate;
            if (date == null)
            {
                MessageBox.Show(
                    "Дата регистрации является обязательным параметром. Пожалуйста, заполните это поле",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            _client.NameClient = TBFirstName.Text;
            _client.SurnameClient = TBSurnameName.Text;
            _client.PatronumicClient = TBPatronumicName.Text;
            _client.EmailClient = TBEmail.Text;
            _client.RegistrationDate = (DateTime) date;
            _client.ProneClient = TBPhone.Text;
            _client.Image = _mainImageData;
            App.Context.SaveChanges();

            MessageBox.Show(
                "Данные успешно сохранены",
                "Данные сохранены",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void BtnAddClientService_Click(object sender, RoutedEventArgs e)
        {
            if (DPClientServiceStartTime.SelectedDate == null)
            {
                MessageBox.Show(
                    "Дата оформления услуги является обязательным параметром. Пожалуйста, заполните это поле",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            var serviceNameSelected = CBAddClientService.SelectedItem as string;
            var serviceSelected = App.Context.Services.ToList()
                .Where(s => s.NameService == serviceNameSelected)
                .FirstOrDefault();
            var clientServiceCreated = App.Context.ClientServices.Create();

            clientServiceCreated.IDcs = App.Context.ClientServices.ToList()
                .Last().IDcs + 1;
            clientServiceCreated.IDClient = _client.IDClient;
            clientServiceCreated.IDService = serviceSelected.IDService;
            clientServiceCreated.StartTime = (DateTime) DPClientServiceStartTime.SelectedDate;

            App.Context.ClientServices.Add(clientServiceCreated);
            App.Context.SaveChanges();

            UpdateServicesList();
            UpdateCostStats();
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image | *.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageService.Source = new ImageSourceConverter()
                    .ConvertFrom(_mainImageData) as ImageSource;
            }
        }
    }
}
