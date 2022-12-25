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

namespace CRMTelmate.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientInfoPage.xaml
    /// </summary>
    public partial class ClientInfoPage : Page
    {
        public static CultureInfo cultureInfoRu = CultureInfo.GetCultureInfo("ru-RU");
        private Entities.Client _client;

        public ClientInfoPage(Entities.Client client)
        {
            InitializeComponent();

            _client = client;

            InitializeCostStats();

            LViewClient.ItemsSource = new List<Entities.Client>() { client };
        }

        private void InitializeCostStats()
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

        private void BtnEditSrvc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteSrvc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
