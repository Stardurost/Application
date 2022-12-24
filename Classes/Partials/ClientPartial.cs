using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRMTelmate.Entities
{
    partial class Client
    {
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

        public decimal CostSum
        {
            get
            {
                return ClientServices.Sum(cs => cs.Service.Cost);
            }
        }
    }
}
