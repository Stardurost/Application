using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMTelmate.Entities
{
    partial class Service
    {
        public static CultureInfo cultureInfoRu = CultureInfo.GetCultureInfo("ru-RU");

        public string CostFormatted
        {
            get
            {
                return $"Цена: {Cost.ToString("C", cultureInfoRu)}";
            }
        }
    }
}
