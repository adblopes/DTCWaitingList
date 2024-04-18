using DTCWaitingList.Interface;
using DTCWaitingList.Services;
using Google.Apis.Gmail.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace DTCWaitingList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeAppAsync();
        }

        public async void InitializeAppAsync()
        {
            using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IEmailService, EmailService>();
                services.AddSingleton<GmailService>();
                services.AddAutoMapper(typeof(App));
                services.AddScoped<IDataAccessService, DataAccessService>();
                services.AddDbContext<WaitingListDb>(options =>
                    options.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=DTCWaitingList;Trusted_Connection=True;TrustServerCertificate=True"),
                    ServiceLifetime.Scoped);
            })
            .Build();

            await RunAsync(host.Services);
        }

        static async Task RunAsync(IServiceProvider host)
        {
            using (IServiceScope serviceScope = host.CreateScope())
            {
                IServiceProvider provider = serviceScope.ServiceProvider;

                var dataAccessService = provider.GetService<IDataAccessService>();
                var emailService = provider.GetService<IEmailService>();

                try
                {
                    await emailService!.ProcessInboxUnreadAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing inbox: {ex.Message}");
                }

                try
                {
                    Current.MainWindow = new MainWindow(dataAccessService!);
                    Current.MainWindow.Show();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating/showing main window: {ex.Message}");
                }
            }
        }
    }
}
