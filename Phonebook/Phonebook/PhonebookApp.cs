namespace Phonebook
{
    internal class PhonebookApp
    {
        public PhoneBookService Service { get; set; }
        public UserInteraction UserInteraction { get; set; }


        public PhonebookApp(PhoneBookService service, UserInteraction userInteraction)
        {
            Service = service;
            UserInteraction = userInteraction;
        }

        public void Run()
        {
            MainMenuOption userInput = UserInteraction.GetMainMenuInput();

            while(userInput != MainMenuOption.Exit)
            {
                ProcessMainMenu(userInput);
                userInput = UserInteraction.GetMainMenuInput();
            }
        }
        private void ProcessMainMenu(MainMenuOption option)
        {
            switch (option) 
            {
                case MainMenuOption.ViewContacts:
                    HandleViewContacts();
                    break;
                case MainMenuOption.AddContact:
                    throw new NotImplementedException();
                    break;
                case MainMenuOption.UpdateContact:
                    throw new NotImplementedException();
                    break;
                case MainMenuOption.DeleteContact:
                    throw new NotImplementedException();
                    break;
 
            }
        }
        private void HandleViewContacts() 
        {
            List<Category> categories = Service.GetAllCategories();

            ViewContactMenuOption option = UserInteraction.PrintViewContactMenu();

            while (option != ViewContactMenuOption.ReturnToMainMenu)
            {
                ProcessViewContacts(option, categories);
                option = UserInteraction.PrintViewContactMenu();
            }

        }
        private void ProcessViewContacts(ViewContactMenuOption option, List<Category> categories)
        {
            List<Contact> contacts = new();

            switch (option)
            {
                case ViewContactMenuOption.ViewAll:
                    contacts = Service.GetAllContacts();
                    break;
                case ViewContactMenuOption.ViewContactByName:
                    string userInput = UserInteraction.GetStringFromUser("Please enter full name or part of a contact name (case insensitive): ");
                    contacts = Service.GetContactsByName(userInput);
                    break;
                case ViewContactMenuOption.ViewContactByCategory:
                    Category category = UserInteraction.GetCategoryFromUser(categories);
                    contacts = Service.GetContactsByCategory(category);
                    break;
                case ViewContactMenuOption.ReturnToMainMenu:
                    return;

            }
            UserInteraction.PrintContacts(contacts, categories);
        }

        public void HandleAddContact() { }
        public void HandleUpdateContact() { }
        public void HandleDeleteContact() { }



    }
}
