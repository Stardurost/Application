using Microsoft.Win32;
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
using File = System.IO.File;

namespace CRMTelmate.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddNewClient.xaml
    /// </summary>
    public partial class AddNewClient : Page
    {
        private byte[] _mainImageData = null;
        int IDNew = App.Context.Clients.Max(p => p.IDClient) + 1;
        public AddNewClient()
        {
            InitializeComponent();
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (TBFirstName.Text == null || TBSurnameName.Text == null || TBPhone.Text == null || TBEmail.Text == null)
            {
                MessageBox.Show("Заполните все необходимые поля",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            var clientNew = new Entities.Client
            {
                IDClient = IDNew,
                SurnameClient = TBSurnameName.Text,
                PatronumicClient = TBPatronumicName.Text,
                NameClient = TBFirstName.Text,
                ProneClient = TBPhone.Text,
                EmailClient = TBEmail.Text,
                RegistrationDate = (DateTime)DPRegistrationDate.SelectedDate,
                Image = _mainImageData
            };

            App.Context.Clients.Add(clientNew);

            MessageBox.Show(
                "Данные успешно сохранены",
                "Данные сохранены",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            App.Context.SaveChanges();

            NavigationService.GoBack();
        }
    }
}
