using System.Text.Json;

namespace Phonebook
{
    /// <summary>
    /// Provides a service layer between the Phonebook application and the database.
    /// Handles data access, CRUD operations, and optional auto-seeding from a JSON file.
    /// </summary>
    internal class PhoneBookService
    {
        public PhonebookContext Context { get; set; }


        /// <summary>
        /// Initializes new instance PhoneBookService class
        /// </summary>
        /// <param name="context"><see cref="PhonebookContext"/> used for DB access</param>
        public PhoneBookService(PhonebookContext context)
        {
            Context = context;
        }
        /// <summary>
        /// Automatically seeds the database with default data if both Contacts and Categories are empty.
        /// </summary>
        public void AutoSeed()
        {

            if (!Context.Contacts.Any() && !Context.Categories.Any())
            {
                var rawJson = File.ReadAllText("Resources/defaultData.json");
                var deserializedJson = JsonSerializer.Deserialize<DefaultData>(rawJson);

                if (deserializedJson == null) { return; }

                AutoSeedCategories(deserializedJson);
                AutoSeedContacts(deserializedJson);
            }
        }
        /// <summary>
        /// Seeds the database with default categories.
        /// </summary>
        /// <param name="data">The deserialized data containing default categories.</param>
        private void AutoSeedCategories(DefaultData data)
        {
            foreach (var defaultCategory in data.Categories)
            {
                Context.Add(new Category() { Name = defaultCategory.Name! });
            }

            Context.SaveChanges();
        }
        /// <summary>
        /// Seeds the database with default contacts.
        /// </summary>
        /// <param name="data">The deserialized data containing default contacts.</param>
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

        /// <summary>
        /// Inserts single Contact to database
        /// </summary>
        /// <param name="contact"><see cref="Contact"/> to be inserted</param>
        /// <returns>true if the contact was sucessfully inserted, false otherwise</returns>
        public bool InsertContact(Contact contact)
        {
            Context.Contacts.Add(contact);
            return Context.SaveChanges() > 0;
        }
        /// <summary>
        /// Updates single Contact to database
        /// </summary>
        /// <param name="contact"><see cref="Contact"/> updated with new values</param>
        /// <returns>true if the contact was sucessfully updated, false otherwise</returns>
        public bool UpdateContact(Contact updatedContact)
        {
            Context.Contacts.Update(updatedContact);

            return Context.SaveChanges() > 0;
        }
        /// <summary>
        /// Deletes single Contact to database
        /// </summary>
        /// <param name="contact"><see cref="Contact"/> to be deleted</param>
        /// <returns>true if the contact was sucessfully deleted, false otherwise</returns>
        public bool DeleteContact(Contact contact)
        {
            Context.Contacts.Remove(contact);
            return Context.SaveChanges() > 0;
        }
        /// <summary>
        /// Retrieves a list of all contacts from the database
        /// </summary>
        /// <returns>A list containing all contact records</returns>
        public List<Contact> GetAllContacts()
        {
            var contacts = Context.Contacts.OrderBy(x => x.FirstName).ToList();
            return contacts;
        }
        /// <summary>
        /// Retrieves all contacts from the database whose first name starts with given name
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>A list of matching contacts.</returns>
        public List<Contact> GetContactsStartingWith(string name)
        {
            var contacts = Context.Contacts.Where(x => x.FirstName.ToLower()
                                    .StartsWith(name))
                                    .OrderBy(x => x.FirstName);

            return contacts.ToList();
        }
        /// <summary>
        /// Retrieves all contacts from the database whose first name starts with given name
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>A list of matching contacts.</returns>
        public List<Contact> GetContactsByCategory(Category? category)
        {
            if (category == null)
            {
                return Context.Contacts.Where(x => x.Category == null).ToList();
            }

            return Context.Contacts.Where(x => x.CategoryId == category.CategoryId)
                                    .OrderBy(x => x.FirstName)
                                    .ToList();
        }
        public List<Contact> GetAllContactsWithEmail()
        {
            var contacts = Context.Contacts.Where(x => x.Email !=null).ToList();
            return contacts;
        }
        /// <summary>
        /// Inserts single Category to database
        /// </summary>
        /// <param name="category"><see cref="Category"/> to be inserted</param>
        /// <returns>true if the category was sucessfully inserted, false otherwise</returns>
        public bool InsertCategory(Category category)
        {
            Context.Categories.Add(category);
            return Context.SaveChanges() > 0;
        }
        /// <summary>
        /// Updates single existing category in the database
        /// </summary>
        /// <param name="category"><see cref="Category"/> updated with new values</param>
        /// <returns>true if the category was sucessfully updated, false otherwise</returns>
        public bool UpdateCategory(Category updatedCategory)
        {
            Context.Categories.Update(updatedCategory);
            return Context.SaveChanges() > 0;
        }
        /// <summary>
        /// Deleted single Category from database
        /// </summary>
        /// <param name="category"><see cref="Category"/> to be deleted</param>
        /// <returns>true if the category was sucessfully deleted, false otherwise</returns>
        public bool DeleteCategory(Category category)
        {
            Context.Categories.Remove(category);
            return Context.SaveChanges() > 0;
        }
        /// <summary>
        /// Retrieves list of all Categories from the database
        /// </summary>
        /// <returns>A List of all Categories in the database</returns>
        public List<Category> GetAllCategories()
        {
            var contacts = Context.Categories.ToList();
            return contacts;
        }
    }
}