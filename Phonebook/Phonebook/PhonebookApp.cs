using Phonebook.Services;

namespace Phonebook
{
    /// <summary>
    /// Main application class that manages the phonebook's core flow and user interaction.
    /// </summary>
    internal class PhoneBookApp
    {
        public PhoneBookService Service { get; set; }
        public UserInteractionService UiService { get; set; }
        
        public EmailService EmailService { get; set; }
        /// <summary>
        /// Initializes PhoneBookApp object
        /// </summary>
        /// <param name="service">Service used to manage database access</param>
        /// <param name="userInteraction">Service used to present or get data from user</param>
        public PhoneBookApp(PhoneBookService service, UserInteractionService userInteraction, EmailService emailService)
        {
            Service = service;
            UiService = userInteraction;
            EmailService = emailService;
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

                case MainMenuOption.SendEmail:
                    HandleSendEmail();
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
                            .GetStringFromUser(AppStrings.CONTACT_PARTNAME)
                            .ToLower();
                    contacts = Service.GetContactsStartingWith(userInput);
                    break;

                case ViewContactMenuOption.ViewContactByCategory:
                    Category? category = UiService.SelectCategory(categories, AppStrings.SELECT_CATEGORY);
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
        /// Handles sending of the email
        /// </summary>
        private void HandleSendEmail()
        {
            var contactsWithEmail = Service.GetAllContactsWithEmail();
            var destinationEmail = UiService.SelectContact(contactsWithEmail, AppStrings.EMAIL_DESTINATION_PROMPT)
                                    .Email!;


            if (EmailService.SendEmail(destinationEmail))
            {
                Console.WriteLine(AppStrings.SENDEMAIL_SUCCESS);
            }
            else
            {
                Console.WriteLine(AppStrings.SENDEMAIL_FAIL);
            }
            UiService.PressAnyKeyToContinue();



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
               AppStrings.CONTACT_ADD_SUCCESS,
               AppStrings.CONTACT_ADD_FAIL);
        }
        /// <summary>
        /// Handles <see cref="MainMenuOption.UpdateContact"/> main menu option
        /// </summary>
        /// <param name="categories">List of all available categories</param>
        public void HandleUpdateContact(List<Category> categories)
        {
            List<Contact> contacts = Service.GetAllContacts();
            Contact contact = UiService.SelectContact(contacts, AppStrings.CONTACT_SELECT);
            contact = UiService.GetUpdatedContact(contact, categories);

            UiService.PrintContact(contact, AppStrings.CONTACT_UPDATE_SUMMARY);

            ConfirmAndExecute(() => Service.UpdateContact(contact),
               AppStrings.CONTACT_UPDATE_SUCCESS,
               AppStrings.CONTACT_UPDATE_FAIL);
        }
        /// <summary>
        /// Handles <see cref="MainMenuOption.DeleteContact"/> main menu option
        /// </summary>
        public void HandleDeleteContact()
        {
            List<Contact> contacts = Service.GetAllContacts();

            Contact contact = UiService.SelectContact(contacts, AppStrings.CONTACT_SELECT);

            UiService.PrintContact(contact, AppStrings.CONTACT_DELETE_SUMMARY);

            ConfirmAndExecute(() => Service.DeleteContact(contact),
               AppStrings.CONTACT_DELETE_SUCCESS,
               AppStrings.CONTACT_DELETE_FAIL);
        }

        /// <summary>
        /// Handles <see cref="CategoryMenuOption.AddCategory"/> category menu option
        /// </summary>
        /// <param name="categories">List of all available categories</param>
        public void HandleAddCategory(List<Category> categories)
        {
            Category newCategory = UiService.GetNewCategory(AppStrings.CATEGORY_NEWNAME, categories);

            ConfirmAndExecute(() => Service.InsertCategory(newCategory),
               AppStrings.CATEGORY_ADD_SUCCESS,
               AppStrings.CATEGORY_ADD_FAIL);
        }
        /// <summary>
        /// Handles <see cref="CategoryMenuOption.UpdateCategory"/> category menu option
        /// </summary>
        /// <param name="categories">List of all available categories</param>
        public void HandleUpdateCategory(List<Category> categories)
        {
            Category updatedCategory = UiService.UpdateCategory(categories);

            UiService.PrintCategory(updatedCategory, AppStrings.CATEGORY_ADD_SUMMARY);

            ConfirmAndExecute(() => Service.UpdateCategory(updatedCategory),
               AppStrings.CATEGORY_UPDATE_SUCCESS,
               AppStrings.CATEGORY_UPDATE_FAIL);
        }
        /// <summary>
        /// Handles <see cref="CategoryMenuOption.DeleteCategory"/> category menu option
        /// </summary>
        /// <param name="categories">List of all available categories</param>
        public void HandleDeleteCategory(List<Category> categories)
        {
            Category toBeDeleted = UiService.SelectCategory(categories,
                AppStrings.CATEGORY_SELECT, false)!;

            UiService.PrintCategory(toBeDeleted, AppStrings.CATEGORY_DELETE_SUMMARY);

            ConfirmAndExecute(() => Service.DeleteCategory(toBeDeleted),
                AppStrings.CATEGORY_DELETE_SUCCESS,
               AppStrings.CATEGORY_DELETE_FAIL);
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