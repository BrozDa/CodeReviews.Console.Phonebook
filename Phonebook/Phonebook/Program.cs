using Microsoft.EntityFrameworkCore;
using Phonebook.Models;
using System.Text.Json;

namespace Phonebook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PhonebookContext context = new PhonebookContext();
            PhoneBookService svc = new PhoneBookService(context, true);
            UserInteraction ui = new UserInteraction();
            PhonebookApp app = new PhonebookApp(svc, ui);
            app.Run();

            /*Console.WriteLine("aaaA");
            var json = File.ReadAllText("Resources/defaultData.json");
            var seedData = JsonSerializer.Deserialize<DefaultData>(json);

            using var context = new PhonebookContext();

            foreach (var category in seedData.Categories) 
            {
                context.Categories.Add(new ContactCategory() { Name = category.Name! });
            }
            context.Categories.AddRange(context.Categories);

            context.SaveChanges();

            List<Contact> contacts = new List<Contact>();

            foreach (var contact in seedData.Contacts)
            {
                if (!string.IsNullOrEmpty(contact.Category))
                {
                    Category category = context.Categories.FirstOrDefault(x => x.Name == contact.Category)!;
                    contact.CategoryId = category.CategoryId;
                }
                contacts.Add(new Contact()
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    PhoneNumber = contact.PhoneNumber,
                    Email = contact.Email,
                    ContactCategoryId = contact.CategoryId,

                });
            }
            context.AddRange(contacts);
            context.SaveChanges();*/
        }
    }
}
