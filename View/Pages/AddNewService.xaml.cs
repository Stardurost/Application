using CRMTelmate.Entities;
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
    /// Логика взаимодействия для AddNewService.xaml
    /// </summary>
    public partial class AddNewService : Page
    {
        private byte[] _mainImageData = null;
        int IDNew = App.Context.Services.Max(p => p.IDService) + 1;
        public AddNewService()
        {
            InitializeComponent();
            InitilizeCB();
        }
        private void InitilizeCB()
        {
            Type.ItemsSource = App.Context.TypeServices.ToList();
            Type.DisplayMemberPath = "TypeName";
            Type.SelectedIndex = 0;
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
            if (NameServiceTB.Text == null || CostTB.Text == null)
            {
                MessageBox.Show("Заполните все необходимые поля",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            try {
                var serviceNew = new Entities.Service
                {
                    IDService = IDNew,
                    NameService = NameServiceTB.Text,
                    Cost = Convert.ToDecimal(CostTB.Text),
                    Description = DescriptionTB.Text,
                    TypeService = App.Context.TypeServices.Where(p => p.TypeName == Type.Text).FirstOrDefault().IDTypeService,
                    Image = _mainImageData
                };

                App.Context.Services.Add(serviceNew);
                MessageBox.Show(
               "Данные успешно сохранены",
               "Данные сохранены",
               MessageBoxButton.OK,
               MessageBoxImage.Information
           );
                App.Context.SaveChanges();

                NavigationService.GoBack();
            }
            catch { MessageBox.Show("Не удалось сохранить услугу. Неверный ввод данных."); }
            
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
