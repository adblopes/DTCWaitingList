using DTCWaitingList.Interface;
using DTCWaitingList.Services;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace DTCWaitingList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IEmailService, EmailService>();
                services.AddSingleton<IDataAccessService, DataAccessService>();
            })
            .Build();

            Run(host.Services);
        }
        static void Run(IServiceProvider hostProvider)
        {
            using IServiceScope serviceScope = hostProvider.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var emailService = provider.GetService<IEmailService>();

            //run the inbox process method every hour while the program is active
            Timer timer = new Timer(x => emailService!.ProcessInboxUnread(), null, TimeSpan.Zero, TimeSpan.FromMinutes(60));  
        }
    }
}