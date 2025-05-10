using Microsoft.Extensions.DependencyInjection;
using Phonebook.Services;


namespace Phonebook
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            SetServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var app = serviceProvider.GetRequiredService<PhoneBookApp>();
            app.Run();


        }
        public static void SetServices(IServiceCollection services)
        {
            services.AddSingleton<PhonebookContext>(new PhonebookContext());
            services.AddSingleton<UserInteractionService>(new UserInteractionService());
            
            services.AddSingleton(sp => new PhoneBookService(
                sp.GetRequiredService<PhonebookContext>()
                ));
            services.AddSingleton(sp => new EmailService(
                sp.GetRequiredService<UserInteractionService>()
                ));

            services.AddSingleton(sp => new PhoneBookApp(
                sp.GetRequiredService<PhoneBookService>(),
                sp.GetRequiredService<UserInteractionService>(),
                sp.GetRequiredService<EmailService>()
                ));
                
        }
    }
}