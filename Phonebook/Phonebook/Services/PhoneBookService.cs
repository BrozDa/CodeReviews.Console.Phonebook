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

        public bool AddContact(Contact contact)
        {
            Context.Contacts.Add(contact);
            return Context.SaveChanges() > 0;
        }
        public bool UpdateContact(int originalContactId, Contact updatedContact)
        {
            Contact? original = Context.Contacts.Where(x => x.ContactId == originalContactId).FirstOrDefault();
            if (original is null) return false;

            original.FirstName = updatedContact.FirstName;
            original.LastName = updatedContact.LastName;
            original.PhoneNumber = updatedContact.PhoneNumber;
            original.Email = updatedContact.Email;

            return Context.SaveChanges() > 0;
        }
        public bool RemoveContact(Contact contact) 
        { 
            Context.Contacts.Remove(contact);
            return Context.SaveChanges() > 0;
        }
        public List<Contact> GetAllContacts()
        {
            var contacts = Context.Contacts.OrderBy(x => x.FirstName).ToList();
            return contacts;
        }
        public List<Contact> GetContactsByName(string name)
        {
            var contacts = Context.Contacts.Where(x => x.FirstName
                                    .ToLower()
                                    .StartsWith(name))
                                    .OrderBy(x => x.FirstName);

            return contacts.ToList();
        }
        public List<Contact> GetContactsByCategory(Category category)
        {
            var contacts = Context.Contacts.Where(x => x.CategoryId == category.CategoryId)
                                    .OrderBy(x => x.FirstName)
                                    .ToList();

            return contacts.ToList();
        }
        public Contact? GetContactById(int id)
        {
            return Context.Contacts.Where(x => x.ContactId == id).FirstOrDefault();
        }

        public bool AddCategory(Category category)
        {
            Context.Categories.Add(category);
            return Context.SaveChanges() > 0;
        }
        public bool UpdateCategoty(int originalCategoryId, Category updatedCategory)
        {
            Category? original = Context.Categories.Where(x => x.CategoryId == originalCategoryId).FirstOrDefault(); 
            if (original is null) return false;

            original.Name = updatedCategory.Name;

            return Context.SaveChanges() > 0;
        }
        public bool RemoveCategory(Category category)
        {
            Context.Categories.Remove(category);
            return Context.SaveChanges() > 0;
        }
        public List<Category> GetAllCategories()
        {
            var contacts = Context.Categories.ToList();
            return contacts;
        }
        public Category? GetCategoryById(int id)
        {
            return Context.Categories.Where(x => x.CategoryId == id).FirstOrDefault();
        }




    }
}
