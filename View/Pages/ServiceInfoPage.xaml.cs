using CRMTelmate.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
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
using File = System.IO.File;

namespace CRMTelmate.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiceInfoPage.xaml
    /// </summary>
    public partial class ServiceInfoPage : Page
    {
        public CRMTelmate.Entities.Service currentService = null;
        List<Entities.Client> clientsUnFiltered = new List<Entities.Client>();
        private byte[] _mainImageData = null;
        public ServiceInfoPage()
        {
            InitializeComponent();
        }
        public ServiceInfoPage(Entities.Service service)
        {
            currentService = service;
            InitializeComponent();
            InsertTBFromDB();
            InitilizeClients();
            InitilizeCB();
            InitilizeLWClients();
            _mainImageData = currentService.Image;
        }
        private void InitilizeClients()
        {
            var clients = App.Context.Clients.ToList();
            clients = Filter(clients);
            LViewServices.ItemsSource = null;
            LViewServices.ItemsSource = clients;
        }
        private void InitilizeLWClients()
        {
            var clients = App.Context.Clients.ToList();
            clients = Filter(clients);
            ClientLW.ItemsSource = null;
            ClientLW.ItemsSource = clientsUnFiltered;
        }
        private void InitilizeCB()
        {
            Type.ItemsSource = App.Context.TypeServices.ToList();
            Type.DisplayMemberPath = "TypeName";
            Type.SelectedIndex = 0;
        }
        private void InsertTBFromDB()
        {
            NameServiceTB.Text = currentService.NameService.ToString();
            CostTB.Text = currentService.Cost.ToString();
            if (currentService.Description != null)
            {
                DescriptionTB.Text = currentService.Description.ToString();
            }
            try  {
                ImageService.Source = new ImageSourceConverter()
                .ConvertFrom(currentService.Image) as ImageSource;
            }
            catch { }
        }
       private List<Entities.Client> Filter(List<Entities.Client> clients)
        {
            List<Entities.Client> clientsFiltered = new List<Entities.Client>();
            List<Entities.ClientService> clientService;
            clientService = App.Context.ClientServices.ToList();
            clientService = clientService.Where(p => p.IDService.ToString() == currentService.IDService.ToString())
                        .ToList();
            int[] IDClients = new int[clientService.Count];
            int index = 0;
            foreach (var client in clientService)
            {
                IDClients[index] = client.IDClient;
                index++;
            }
            foreach (var client in clients)
            {
                if (InArray(client.IDClient, IDClients)) clientsFiltered.Add(client);
            }
            clientsUnFiltered.Clear();
            foreach (var client in clients)
            {
                if (!InArray(client.IDClient, IDClients)) clientsUnFiltered.Add(client);
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
  

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (NameServiceTB.Text == null || CostTB.Text == null)
            {
                MessageBox.Show("Заполните все необходимые поля",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            currentService.NameService = NameServiceTB.Text;
            currentService.Cost = Convert.ToDecimal(CostTB.Text);
            currentService.Description = DescriptionTB.Text;
         //   currentService.TypeService = App.Context.TypeServices.Where(p => p.TypeName == Type.SelectedItem.ToString()).Select(p => p.IDTypeService).FirstOrDefault();
            currentService.Image = _mainImageData;

            App.Context.SaveChanges();

            MessageBox.Show(
                "Данные успешно сохранены",
                "Данные сохранены",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            NavigationService.GoBack();
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


        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var clients = clientsUnFiltered;
            clients = clients.Where(p => p.SurnameClient.ToLower()
                     .Contains(TBSearch.Text.ToLower())).ToList();
            ClientLW.ItemsSource = clients;
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

            var clientSelected = ((CRMTelmate.Entities.Client)ClientLW.SelectedItem).IDClient;
            var clSelected = App.Context.Clients.ToList()
                .Where(s => s.IDClient == clientSelected)
                .FirstOrDefault();
            var clientServiceCreated = App.Context.ClientServices.Create();

            clientServiceCreated.IDcs = App.Context.ClientServices.ToList()
                .Last().IDcs + 1;
            clientServiceCreated.IDClient = clSelected.IDClient;
            clientServiceCreated.IDService = currentService.IDService;
            clientServiceCreated.StartTime = (DateTime)DPClientServiceStartTime.SelectedDate;

            App.Context.ClientServices.Add(clientServiceCreated);
            App.Context.SaveChanges();

            InitilizeClients();
            InitilizeLWClients();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var currentIDClient = ((CRMTelmate.Entities.Client)button.DataContext).IDClient;

            var currentClientService = App.Context.ClientServices.Where(p => p.IDClient == currentIDClient && p.IDService == currentService.IDService).FirstOrDefault();

            App.Context.ClientServices.Remove(currentClientService);
            App.Context.SaveChanges();

            InitilizeClients();
            InitilizeLWClients();
        }

        private void CostTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            {
                if (IsNumber(e.Text) == false && e.Text != "," & e.Text != "-")      // длина не может быть отрицательной
                {
                    e.Handled = true;
                }

            }
        }  

        private bool IsNumber(string Text)
        {
               int output;
               return int.TryParse(Text, out output);

        }
        
    }
}