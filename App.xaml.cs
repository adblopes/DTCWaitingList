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
            using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IEmailService, EmailService>();
                services.AddSingleton<IDataAccessService, DataAccessService>();
                services.AddSingleton<GmailService>();
                services.AddDbContext<WaitingListDb>(options =>
                    options.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=DTCWaitingList;Trusted_Connection=True;TrustServerCertificate=True"));
            })
            .Build();

            Run(host.Services);
        }
        static void Run(IServiceProvider host)
        {
            using IServiceScope serviceScope = host.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            //var emailService = provider.GetService<IEmailService>();

            //run the inbox process method every 30min while the program is active
            //Timer timer = new Timer(x => emailService!.ProcessInboxUnread(), null, TimeSpan.Zero, TimeSpan.FromMinutes(30));

            try
            {
                //var dataAccessService = provider.GetService<IDataAccessService>();
                //Application.Current.MainWindow = new MainWindow(dataAccessService!);
            }
            catch
            {
                throw new Exception("Was not able to access database, please verify connection and/or credentials and try again.");
            }
        }
    }
}
