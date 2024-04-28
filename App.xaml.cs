using DTCWaitingList.Database;
using DTCWaitingList.Interfaces;
using DTCWaitingList.Services;
using Google.Apis.Gmail.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Windows;
using System.IO;
using System.Timers;

namespace DTCWaitingList
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; set; }

        public IConfiguration Configuration { get; set; }

        public new static App Current => (App)Application.Current;

        public App()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            Services = ConfigureServices(Configuration);

            //run the inbox process method every half hour while the program is active
            var timer = new System.Timers.Timer(30 * 60000);
            timer.Elapsed += RunEmailService;
            timer.Start();
        }

        private static IServiceProvider ConfigureServices(IConfiguration config)
        {
            var services = new ServiceCollection();

            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<GmailService>();
            services.AddAutoMapper(typeof(App));
            services.AddSingleton<IDataAccessService, DataAccessService>();
            services.AddDbContext<WaitingListDb>(options =>
                options.UseSqlServer(config.GetConnectionString("Database")),
                ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Singleton);

            return services.BuildServiceProvider();
        }

        private static void RunEmailService(object sender, ElapsedEventArgs e)
        {
            IEmailService emailService = App.Current.Services.GetService<IEmailService>()!;
            emailService.ProcessInboxUnreadAsync();
        }
    }
}
