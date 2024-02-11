using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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
            try
            {
                //check anti cheat application
                List<Process> processes = Process.GetProcesses().ToList();
                List<string> definedPrograms = new List<string> { "CheatEngine", "Cheat Engine", "ArtMoney", "Art Money", "Cheat-O-Matic", "Cheat-O-Matic", "dnSpy", "dnSpy-x86", "dnSpy-x64", "dnSpy-netcore" };
                foreach (var process in processes)
                {
                    if (definedPrograms.Contains(process.ProcessName))
                    {
                        MessageBox.Show("You are running an anti-cheat application. Please close it and try again.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        // close application
                        Application.Current.Shutdown();
                    }
                }
                SeedData.Seed();
                var loginWindow = MyService.serviceProvider.GetService<WindowLogin>();
                loginWindow.Show();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}