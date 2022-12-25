using CRMTelmate.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private byte[] _mainImageData = null; 
        public ServiceInfoPage()
        {
            InitializeComponent();
        }
        public ServiceInfoPage(CRMTelmate.Entities.Service service)
        {
            currentService = service;
            InitializeComponent();
            InitilizeCB();
            InsertTBFromDB();
        }
        static List<string> SortOptions = new List<string>()
        {
            "По умолчанию",
            "По возрастанию даты регистрации",
            "По убыванию даты регистрации"
        };
        private void InitilizeCB()
        {
            Type.ItemsSource = App.Context.TypeServices.ToList();
            Type.DisplayMemberPath = "TypeName";
            Type.SelectedIndex = 0;

            SortCB.ItemsSource = SortOptions;
            SortCB.SelectedIndex = 0;

        }
        private void InsertTBFromDB()
        {
            NameServiceTB.Text = currentService.NameService.ToString();
            CostTB.Text = currentService.Cost.ToString();
            if (currentService.Description != null)
            {
                DescriptionTB.Text = currentService.Description.ToString();
            }
            ImageService.Source = new ImageSourceConverter()
                    .ConvertFrom(currentService.Image) as ImageSource;
        }
        private List<Entities.Client> Sort(List<Entities.Client> clients)
        {
            var sortOptionIndex = SortCB.SelectedIndex;
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
        private void BtnEditSrvc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteSrvc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            /*NameServiceTB.Text = currentService.NameService.ToString();
            CostTB.Text = currentService.Cost.ToString();
            if (currentService.Description != null)
            {
                DescriptionTB.Text = currentService.Description.ToString();
            }
            ImageService.Source = new ImageSourceConverter()
                    .ConvertFrom(currentService.Image) as ImageSource;

            curProduct.ProductCategory = App.Context.Category.Where(p => p.NameCategory == CBCategory.SelectedItem.ToString())
                    .Select(p => p.IdCategory).FirstOrDefault();

                curProduct.ProductDescription = TBoxDescription.Text;

                curProduct.ProductManufacturer = App.Context.Manufacturer.Where(p => p.NameManufacture == CBManufactor.SelectedItem.ToString())
                    .Select(p => p.IdManufacture).FirstOrDefault();

                curProduct.ProductProvider = App.Context.Provider.Where(p => p.NameProvider == CBProvider.SelectedItem.ToString())
                    .Select(p => p.IdProvider).FirstOrDefault();

                curProduct.ProductCost = Convert.ToDecimal(TBoxCost.Text);

                curProduct.ProductDiscountAmount = (byte?)Convert.ToDecimal(TBoxDiscountAm.Text);

                curProduct.ProductMaxDiscount = (int)Convert.ToDecimal(TBoxDiscountMax.Text);

                curProduct.ProductQuantityInStock = Convert.ToInt32(TBoxInStock.Text);
                if (_mainImageData != null)
                {
                    curProduct.ProductPhoto = _mainImageData;
                }
                App.Context.SaveChanges();
            
            NavigationService.GoBack();
            */
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

        private void BtnSaveImg_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clients = App.Context.Clients.ToList();
            clients = Filter(clients);
            LViewClient.ItemsSource = clients;
            LViewClient.ItemsSource = Sort(clients); 
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
