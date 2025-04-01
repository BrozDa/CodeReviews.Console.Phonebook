using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Phonebook
{
    internal class PhonebookContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<ContactCategory> Categories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<PhonebookContext>()
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
