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
        public static Entities.TelmateEntities1 Context { get; } = new Entities.TelmateEntities1();
        public static Entities.User CurrentUser = null;
        public static void ResetCurrentUser()
        {
            CurrentUser = null;
        }
    }
}
