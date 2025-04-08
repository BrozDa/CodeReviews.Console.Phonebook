namespace Phonebook
{
    /// <summary>
    /// Main application class that manages the phonebook's core flow and user interaction.
    /// </summary>
    internal class PhoneBookApp
    {
        public PhoneBookService Service { get; set; }
        public UserInteraction UiService { get; set; }

        /// <summary>
        /// Initializes PhoneBookApp object
        /// </summary>
        /// <param name="service">Service used to manage database access</param>
        /// <param name="userInteraction">Service used to present or get data from user</param>
        public PhoneBookApp(PhoneBookService service, UserInteraction userInteraction)
        {
            Service = service;
            UiService = userInteraction;
        }
        /// <summary>
        /// Starts the phonebook application and controls the main loop.
        /// </summary>
        public void Run()
        {
            Service.AutoSeed();

            UiService.PrintAppHeader();

            HandleMainMenu();
            
        }
        /// <summary>
        /// Processes the user selected main menu option.
        /// </summary>
        /// <param name="option">Selected <see cref="MainMenuOption"/>.</param>
        private void ProcessMainMenu(MainMenuOption option)
        {
            Console.Clear();

            List<Category> categories = Service.GetAllCategories();
            switch (option)
            {
                case MainMenuOption.ViewContacts:
                    HandleViewContactsMenu(categories);
                    break;

                case MainMenuOption.AddContact:
                    HandleAddContact(categories);
                    break;

                case MainMenuOption.UpdateContact:
                    HandleUpdateContact(categories);
                    break;

                case MainMenuOption.DeleteContact:
                    HandleDeleteContact();
                    break;

                case MainMenuOption.ManageCategories:
                    HandleManageCategoriesMenu();
                    break;
            }
            Console.Clear();
        }
        /// <summary>
        /// Processes the user selected view contacts menu option.
        /// </summary>
        /// <param name="option">Selected <see cref="ViewContactMenuOption"/>.</param>
        private void ProcessViewContacts(ViewContactMenuOption option, List<Category> categories)
        {
            List<Contact> contacts = new();

            switch (option)
            {
                case ViewContactMenuOption.ViewAll:
                    contacts = Service.GetAllContacts();
                    break;

                case ViewContactMenuOption.ViewContactByName:
                    string userInput = UiService
                            .GetStringFromUser("Please enter full or part of a contact name (case insensitive): ")
                            .ToLower();
                    contacts = Service.GetContactsStartingWith(userInput);
                    break;

                case ViewContactMenuOption.ViewContactByCategory:
                    Category? category = UiService.SelectCategory(categories, "Please select category");
                    contacts = Service.GetContactsByCategory(category);
                    break;
            }

            UiService.PrintContacts(contacts);
        }
        /// <summary>
        /// Processes the user selected category menu option.
        /// </summary>
        /// <param name="option">Selected <see cref="CategoryMenuOption"/>.</param>
        private void ProcessCategoryMenu(CategoryMenuOption option)
        {
            var categories = Service.GetAllCategories();
            switch (option)
            {
                case CategoryMenuOption.ViewCategories:
                    UiService.PrintCategories(categories);
                    break;

                case CategoryMenuOption.AddCategory:
                    HandleAddCategory(categories);
                    break;

                case CategoryMenuOption.UpdateCategory:
                    HandleUpdateCategory(categories);
                    break;

                case CategoryMenuOption.DeleteCategory:
                    HandleDeleteCategory(categories);
                    break;
            }
            Console.Clear();
        }
        /// <summary>
        /// Handles the main menu until user decides to exit the application
        /// </summary>
        private void HandleMainMenu()
        {
            MainMenuOption userInput = UiService.GetMainMenuInput();

            while (userInput != MainMenuOption.Exit)
            {
                ProcessMainMenu(userInput);

                UiService.PrintAppHeader();
                userInput = UiService.GetMainMenuInput();
            }
        }
        /// <summary>
        /// Handles manage categories menu until user decides to exit to main menu
        /// </summary>
        private void HandleManageCategoriesMenu()
        {
            CategoryMenuOption option = UiService.PrintCategoryMenu();

            while (option != CategoryMenuOption.ReturnToMainMenu)
            {
                ProcessCategoryMenu(option);
                option = UiService.PrintCategoryMenu();
            }
        }
        /// <summary>
        /// Handles view contacts menu until user decides to exit to main menu
        /// </summary>
        private void HandleViewContactsMenu(List<Category> categories)
        {
            ViewContactMenuOption option = UiService.PrintViewContactMenu();

            while (option != ViewContactMenuOption.ReturnToMainMenu)
            {
                ProcessViewContacts(option, categories);
                option = UiService.PrintViewContactMenu();
            }
        }

        /// <summary>
        /// Handles <see cref="MainMenuOption.AddContact"/> main menu option
        /// </summary>
        /// <param name="categories">List of all available categories</param>
        public void HandleAddContact(List<Category> categories)
        {
            Contact newContact = UiService.GetNewContact(categories);

            ConfirmAndExecute(() => Service.InsertContact(newContact),
               "Contact added successfully",
               "Contact not added, please contact admin");
        }
        /// <summary>
        /// Handles <see cref="MainMenuOption.UpdateContact"/> main menu option
        /// </summary>
        /// <param name="categories">List of all available categories</param>
        public void HandleUpdateContact(List<Category> categories)
        {
            List<Contact> contacts = Service.GetAllContacts();
            Contact contact = UiService.SelectContact(contacts, "Please select contact to be updated");
            contact = UiService.GetUpdatedContact(contact, categories);

            UiService.PrintContact(contact, "Updated contact information");

            ConfirmAndExecute(() => Service.UpdateContact(contact),
               "Contact updated successfully",
               "Contact not updated, please contact admin");
        }
        /// <summary>
        /// Handles <see cref="MainMenuOption.DeleteContact"/> main menu option
        /// </summary>
        public void HandleDeleteContact()
        {
            List<Contact> contacts = Service.GetAllContacts();

            Contact contact = UiService.SelectContact(contacts,"Please select contact to be deleted");

            UiService.PrintContact(contact, "Following contact will be deleted from your phonebook");

            ConfirmAndExecute(() => Service.DeleteContact(contact),
               "Contact deleted successfully",
               "Contact not deleted, please contact admin");
        }

        /// <summary>
        /// Handles <see cref="CategoryMenuOption.AddCategory"/> category menu option
        /// </summary>
        /// <param name="categories">List of all available categories</param>
        public void HandleAddCategory(List<Category> categories)
        {
            Category newCategory = UiService.GetNewCategory("Please enter new category name: ", categories);

            ConfirmAndExecute(() => Service.InsertCategory(newCategory),
               "Category added successfully",
               "Category not added, please contact admin");
        }
        /// <summary>
        /// Handles <see cref="CategoryMenuOption.UpdateCategory"/> category menu option
        /// </summary>
        /// <param name="categories">List of all available categories</param>
        public void HandleUpdateCategory(List<Category> categories)
        {
            Category updatedCategory = UiService.UpdateCategory(categories);

            UiService.PrintCategory(updatedCategory, "Following category will be updated: ");

            ConfirmAndExecute(() => Service.UpdateCategory(updatedCategory),
               "Category updated successfully",
               "Category not updated, please contact admin");
        }
        /// <summary>
        /// Handles <see cref="CategoryMenuOption.DeleteCategory"/> category menu option
        /// </summary>
        /// <param name="categories">List of all available categories</param>
        public void HandleDeleteCategory(List<Category> categories)
        {
            Category toBeDeleted = UiService.SelectCategory(categories,
                "Please select category to be deleted", false)!;

            UiService.PrintCategory(toBeDeleted, "Following category will be deleted: ");

            ConfirmAndExecute(() => Service.DeleteCategory(toBeDeleted),
                "Category deleted successfully",
                "Category not deleted, please contact admin");
        }

        /// <summary>
        /// Confirmation presented to user for any action against the database
        /// </summary>
        /// <param name="action">Func representing method to be executed</param>
        /// <param name="successMessage">string representing the output in case of success</param>
        /// <param name="failureMessage">string representing the output in case of failure</param>
        private void ConfirmAndExecute(Func<bool> action, string successMessage, string failureMessage)
        {
            if (!UiService.ConfirmOperation()) return;

            if (action())
            {
                UiService.PrintText(successMessage);
            }
            else
            {
                UiService.PrintText(failureMessage);
            }
            
            UiService.PressAnyKeyToContinue();
        }
    }
}