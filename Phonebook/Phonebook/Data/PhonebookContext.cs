using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Phonebook
{
    /// <summary>
    /// Represents the Entity Framework Core database context for the Phonebook application.
    /// Provides access to the Contacts and Categories tables and configures database options and relationships.
    /// </summary>
    /// 
    internal class PhonebookContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        /// <summary>
        /// Configures the database connection for the context.
        /// Loads the connection string from user secrets and sets SQL Server as the provider.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<PhonebookContext>()
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            optionsBuilder.UseSqlServer(connectionString);
        }
        /// <summary>
        /// Configures the entity relationships and constraints using the Fluent API.
        /// Sets up a one-to-many relationship between Category and Contact.
        /// Ensures that the Category.Name property is unique.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.Category)
                .WithMany(c => c.Contacts)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}