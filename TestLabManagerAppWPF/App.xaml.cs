using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
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
                List<string> definedPrograms = new List<string> { "CheatEngine", "Cheat Engine", "ArtMoney", 
                    "Art Money", "Cheat-O-Matic", "Cheat-O-Matic", "dnSpy", "dnSpy-x86", "dnSpy-x64", "dnSpy-netcore",
                    "de4dot", "de4dot-x86", "de4dot-x64", "de4dot-netcore", "de4dot-x86-netcore", "de4dot-x64-netcore",
                    "de4dot-netcore-x86", "de4dot-netcore-x64", "de4dot-netcore-x86-x64", "de4dot-netcore-x64-x86", 
                    "de4dot-netcore-x86-x64-x86", "de4dot-netcore-x64-x86-x64", "de4dot-netcore-x86-x64-x86-x64", 
                    "de4dot-netcore-x64-x86-x64-x86" };
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
                //check version
                //string version = "";
                //string currentVersion = GetCurrentVersion();
                //using (WebClient client = new WebClient())
                //{
                //    version = client.DownloadString("https://raw.githubusercontent.com/hoangphuc1998/TestLabManagerAppWPF/master/version.txt");
                //}
                //if (version != Assembly.GetExecutingAssembly().GetName().Version.ToString())
                //{
                //    MessageBox.Show("There is a new version of the application. Please update it.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    // close application
                //    Application.Current.Shutdown();
                //}
                
                var loginWindow = MyService.serviceProvider.GetService<WindowLogin>();
                loginWindow.Show();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetCurrentVersion()
        {
            //Get AppData folder
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //Get TestLab folder
            appData = Path.Combine(appData, "TestLab");
            //Get version file
            string versionFile = Path.Combine(appData, "version.txt");
            //Check if version file exists
            if (File.Exists(versionFile))
            {
                //Read version from file
                return File.ReadAllText(versionFile);
            }
            else
            {
                //Return default version
                return "1.0.0";
            }
        }
    }
}