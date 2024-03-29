﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLabLibrary.Repository;

namespace TestLabManagerAppWPF
{
    public static class MyService
    {
        public static ServiceProvider serviceProvider { get; set; } = null!;

        public static void Initialize()
        {
            // lock instane of MyService
            if (serviceProvider == null)
            {
                ServiceCollection services = new ServiceCollection();
                ConfigureServices(services);
                serviceProvider = services.BuildServiceProvider();
            }
            else
            {
                return;
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<WindowLogin>();
            services.AddSingleton(typeof(IAdminRepository), typeof(AdminRepository));
        }
    }
}
