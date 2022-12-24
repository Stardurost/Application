using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CRMTelmate
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.TelmateEntities Context { get; } = new Entities.TelmateEntities();
        public static Entities.User CurrentUser = null;
    }
}
