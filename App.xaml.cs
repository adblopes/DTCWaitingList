using DTCWaitingList.Database;
using DTCWaitingList.Interfaces;
using DTCWaitingList.Services;
using Google.Apis.Gmail.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DTCWaitingList
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Services { get; set; }

        public new static App Current => (App)Application.Current;

        public App()
        {
            Services = ConfigureServices();
            IEmailService emailService = App.Current.Services.GetService<IEmailService>()!;
            emailService.ProcessInboxUnreadAsync();

            //run the inbox process method every half hour while the program is active
            Timer timer = new(async x => await emailService!.ProcessInboxUnreadAsync(), null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<GmailService>();
            services.AddAutoMapper(typeof(App));
            services.AddSingleton<IDataAccessService, DataAccessService>();
            services.AddDbContext<WaitingListDb>(options =>
                options.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=DTCWaitingList;Trusted_Connection=True;TrustServerCertificate=True"),
                ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Singleton);

            return services.BuildServiceProvider();
        }
    }
}
