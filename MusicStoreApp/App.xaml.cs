using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicStoreApp.Core.Data;
using MusicStoreApp.Core.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MusicStoreApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddDbContext<MusicStoreDbContext>(options =>
                options.UseSqlServer("Data Source=DESKTOP-67UPOEU;Initial Catalog=MusicStore;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"));

            services.AddTransient<UserService>();
            services.AddTransient<OrderService>();
            services.AddTransient<ProductService>();

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }


}
