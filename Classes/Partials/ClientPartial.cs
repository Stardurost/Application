using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRMTelmate.Entities
{
    partial class Client
    {
        public static CultureInfo cultureInfoRu = CultureInfo.GetCultureInfo("ru-RU");

        public byte[] Avatar
        {
            get
            {
                if (Image != null)
                {
                    return Image;
                }

                var resourceName = "CRMTelmate.Resources.user.png";

                Assembly assembly = Assembly.GetEntryAssembly();

                Stream stream = assembly.GetManifestResourceStream(resourceName);
                byte[] image = new byte[stream.Length];
                stream.Read(image, 0, (int)stream.Length);

                return image;
            }
        }

        public string FullName
        {
            get
            {
                return $"{SurnameClient} {NameClient} {PatronumicClient}";
            }
        }

        public string RegistrationDateFormatted
        {
            get
            {
                var dateFormatted = RegistrationDate.ToString("D", cultureInfoRu);

                return $"Дата регистрации: {dateFormatted}";
            }
        }

        public decimal CostSum
        {
            get
            {
                return ClientServices.Sum(cs => cs.Service.Cost);
            }
        }

        public string CostSumFormatted
        {
            get
            {
                return $"Сумма трат: {CostSum.ToString("C", cultureInfoRu)}";
            }
        }
    }
}
