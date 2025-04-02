using Phonebook.Models;
using System.Text.Json;

namespace Phonebook
{
    internal class PhoneBookService
    {
        public PhonebookContext Context { get; set; }
        public bool _autoSeed;
        public PhoneBookService(PhonebookContext context, bool autoSeed = true)
        {
            Context = context;            
            _autoSeed = autoSeed;
        }

        public void InsertContact(Contact contact)
        {
            Context.Add(contact);
            Context.SaveChanges();
        } 

        public void AutoSeed()
        {
            if(!_autoSeed) { return; }

            if (!Context.Contacts.Any() && !Context.Categories.Any()) 
            {
                var rawJson = File.ReadAllText("Resources/defaultData.json");
                var deserializedJson = JsonSerializer.Deserialize<DefaultData>(rawJson);

                if (deserializedJson == null){ return; }

                AutoSeedCategories(deserializedJson);
                AutoSeedContacts(deserializedJson);
            }
        }
        private void AutoSeedCategories(DefaultData data)
        {
            foreach (var defaultCategory in data.Categories)
            {
                Context.Add(new Category() { Name = defaultCategory.Name! });
            }

            Context.SaveChanges();
        }
        private void AutoSeedContacts(DefaultData data)
        {
            foreach (var defaultContact in data.Contacts)
            {
                int? categoryId = null;

                if (!string.IsNullOrEmpty(defaultContact.Category))
                {
                    var category = Context.Categories.FirstOrDefault(x => x.Name == defaultContact.Category);

                    if (category != null) { categoryId = category.CategoryId; }
                }

                Context.Contacts.Add(new Contact()
                {
                    FirstName = defaultContact.FirstName,
                    LastName = defaultContact.LastName,
                    PhoneNumber = defaultContact.PhoneNumber,
                    Email = defaultContact.Email,
                    CategoryId = categoryId
                });
            }

            Context.SaveChanges();
        }




        
    }
}
