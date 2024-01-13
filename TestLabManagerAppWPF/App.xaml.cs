using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using TestLabEntity;
using TestLabEntity.AutoDB;

namespace TestLabManagerAppWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // initialize the database
            MyService.Initialize();
            MyMapper.Initialize();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            SeedData.Seed();
            var loginWindow = MyService.serviceProvider.GetService<WindowLogin>();
            loginWindow.Show();
        }
    }
}