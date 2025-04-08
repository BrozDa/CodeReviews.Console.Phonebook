using Spectre.Console;
using System.Text.RegularExpressions;

namespace Phonebook
{
    /// <summary>
    /// Handles all user interactions through the console interface.
    /// Provides prompts, validation, input parsing, and formatted output using Spectre.Console.
    /// </summary>
    internal class UserInteraction
    {
        private Regex validStringFormat = new Regex("^[A-Za-z0-9]+(?: [A-Za-z0-9]+)*$");
        private Regex validPhoneNumberFormat = new Regex(@"\+\d+");
        private Regex validEmailAddressFormat = new Regex(@"^[\w\.-]+@[\w\.-]+\.\w{2,}$");

        /// <summary>
        /// Prints application header to the console
        /// </summary>
        public void PrintAppHeader()
        {
            Console.WriteLine(AppStrings.APP_HEADER);
            Console.WriteLine();
        }
        /// <summary>
        /// Prints "Press any key to continue" and awaits for user input
        /// </summary>
        public void PressAnyKeyToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Gets user confirmation
        /// </summary>
        /// <returns>true if user confirmed the operation, false otherwise</returns>
        public bool ConfirmOperation()
        {
            Console.WriteLine();
            AnsiConsole.MarkupLine(AppStrings.CONFIRM_OPERATION);

            bool confirmation = AnsiConsole.Prompt(
                new SelectionPrompt<bool>()
                .AddChoices(true, false)
                .UseConverter(x => x == true ? "Confirm" : "Cancel")
                );

            return confirmation;
        }
        /// <summary>
        /// Print text in the argument to the console
        /// </summary>
        /// <param name="text"></param>
        public void PrintText(string text)
        {
            Console.WriteLine(text);
        }
        /// <summary>
        /// Retrieves string value from user
        /// Input is validated
        /// </summary>
        /// <param name="prompt">Text to be presented as the prompt</param>
        /// <returns>Validated user input</returns>
        public string GetStringFromUser(string prompt)
        {
            var input = AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                .Validate(x => validStringFormat.IsMatch(x))
                .ValidationErrorMessage(AppStrings.INVALID_STRINGFORMAT)
                );
            return input;
        }
        /// <summary>
        /// Retrieves string value representing phone number from user
        /// </summary>
        /// <param name="prompt">Text to be presented as the prompt</param>
        /// <param name="defaultValue">Optional parameter representing default value in the prompt</param>
        /// <returns>Validated user input</returns>
        public string GetPhoneNumber(string prompt, string? defaultValue = null)
        {
            var inputPrompt = new TextPrompt<string>(prompt)
                .Validate(x => validPhoneNumberFormat.IsMatch(x))
                .ValidationErrorMessage(AppStrings.PHONENUMBER_FORMAT);

            if (defaultValue != null)
            {
                inputPrompt.DefaultValue(defaultValue);
            }

            var input = AnsiConsole.Prompt(inputPrompt);

            return input;
        }
        /// <summary>
        /// Prints out the main menu and gets user input
        /// </summary>
        /// <returns><see cref="MainMenuOption"/> value represeting user input in menu</returns>
        /// <exception cref="NotImplementedException">Thrown when invalid enum value is passed</exception>
        public MainMenuOption GetMainMenuInput()
        {
            var options = Enum.GetValues<MainMenuOption>();

            AnsiConsole.MarkupLine(AppStrings.MENU_CHOOSE_OPTION);
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOption>()
                .AddChoices(options)
                .UseConverter(x => x switch
                {
                    MainMenuOption.ViewContacts => AppStrings.MAINMENU_VIEWCONTACTS,
                    MainMenuOption.AddContact => AppStrings.MAINMENU_ADDCONTACT,
                    MainMenuOption.UpdateContact => AppStrings.MAINMENU_UPDATECONTACT,
                    MainMenuOption.DeleteContact => AppStrings.MAINMENU_DELETECONTACT,
                    MainMenuOption.ManageCategories => AppStrings.MAINMENU_DELETECONTACT,
                    MainMenuOption.Exit => AppStrings.MAINMENU_EXIT,
                    _ => throw new NotImplementedException(AppStrings.MAINMENU_INVALIDOPTION)
                }));
            Console.Clear();
            return input;
        }
        /// <summary>
        /// Prints out the view contact menu and gets user input
        /// </summary>
        /// <returns><see cref="ViewContactMenuOption"/> value represeting user input in menu</returns>
        /// <exception cref="NotImplementedException">Thrown when invalid enum value is passed</exception>
        public ViewContactMenuOption PrintViewContactMenu()
        {
            var options = Enum.GetValues<ViewContactMenuOption>();

            AnsiConsole.MarkupLine(AppStrings.MENU_CHOOSE_OPTION);

            var input = AnsiConsole.Prompt(
                new SelectionPrompt<ViewContactMenuOption>()
                .AddChoices(options)
                .UseConverter(x => x switch
                {
                    ViewContactMenuOption.ViewAll => AppStrings.VIEWCONTACTMENU_VIEWALL,
                    ViewContactMenuOption.ViewContactByName => AppStrings.VIEWCONTACTMENU_FILTERBYNAME,
                    ViewContactMenuOption.ViewContactByCategory => AppStrings.VIEWCONTACTMENU_FILTERBYCATEGORY,
                    ViewContactMenuOption.ReturnToMainMenu => AppStrings.VIEWCONTACTMENU_EXIT,
                    _ => throw new NotImplementedException(AppStrings.VIEWCONTACTMENU_INVALIDOPTION)
                }));
            Console.Clear();
            return input;
        }
        /// <summary>
        /// Prints out the manage categories menu and gets user input
        /// </summary>
        /// <returns><see cref="CategoryMenuOption"/> value represeting user input in menu</returns>
        /// <exception cref="NotImplementedException">Thrown when invalid enum value is passed</exception>
        public CategoryMenuOption PrintCategoryMenu()
        {
            var options = Enum.GetValues<CategoryMenuOption>();

            AnsiConsole.MarkupLine(AppStrings.MENU_CHOOSE_OPTION);
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<CategoryMenuOption>()
                .AddChoices(options)
                .UseConverter(x => x switch
                {
                    CategoryMenuOption.ViewCategories => AppStrings.CATEGORYMENU_VIEW,
                    CategoryMenuOption.AddCategory => AppStrings.CATEGORYMENU_ADD,
                    CategoryMenuOption.UpdateCategory => AppStrings.CATEGORYMENU_UPDATE,
                    CategoryMenuOption.DeleteCategory => AppStrings.CATEGORYMENU_DELETE,
                    CategoryMenuOption.ReturnToMainMenu => AppStrings.CATEGORYMENU_EXIT,
                    _ => throw new NotImplementedException(AppStrings.CATEGORYMENU_INVALIDOPTION)
                }));
            Console.Clear();
            return input;
        }
        /// <summary>
        /// Prompts user to select category from presented options
        /// </summary>
        /// <param name="categories">List of categories which to be presented to the user</param>
        /// <param name="prompt">Text to be presented as the prompt</param>
        /// <param name="includeUncategorizedOpt">optional parameter which allows to add uncategorized to selection </param>
        /// <returns>Selected Category, or null in case user chose uncategorized option</returns>
        public Category? SelectCategory(List<Category> categories, string prompt, bool includeUncategorizedOpt = true)
        {
            List<string> categoryNames = categories.Select(x => x.Name).ToList();

            if (includeUncategorizedOpt)
            {
                categoryNames.Add(AppStrings.NOCATEGORY);
            }

            AnsiConsole.MarkupLine($"{prompt}:");

            var input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices(categoryNames)
                );

            Console.Clear();

            if (input != AppStrings.NOCATEGORY)
                return categories.Find(x => x.Name == input);
            else
                return null;
        }
        /// <summary>
        /// Prints out single contact to the console in form of table
        /// </summary>
        /// <param name="contact"><see cref="Contact"/> to be printed</param>
        /// <param name="prompt">Text to be presented as the prompt</param>
        public void PrintContact(Contact contact, string prompt)
        {
            Console.WriteLine(prompt);

            string categoryStr = contact.Category == null ?
                "-" : 
                contact.Category.Name;

            var table = new Table();
            table.AddColumns(AppStrings.CONTACT_PROPERTIES);
            table.AddRow(
                contact.FirstName,
                contact.LastName,
                contact.PhoneNumber,
                contact.Email ?? AppStrings.NOVALUE,
                contact.Category != null ? contact.Category.Name : AppStrings.NOVALUE);

            AnsiConsole.Write(table);
        }
        /// <summary>
        /// Prints out all contacts to the console in form of table
        /// </summary>
        /// <param name="contacts">List of contacts to be printed</param>
        public void PrintContacts(List<Contact> contacts)
        {
            var table = new Table();
            if (contacts == null || contacts.Count == 0)
            {
                table.AddColumn("");
                table.HideHeaders();
                table.AddRow(AppStrings.NOCONTACT);
            }
            else
            {
                table.AddColumns(AppStrings.CONTACT_PROPERTIES);

                foreach (var contact in contacts)
                {
                    table.AddRow(GenerateRow(contact));
                }
            }
            AnsiConsole.Write(table);
            PressAnyKeyToContinue();
            Console.Clear();
        }
        /// <summary>
        /// Helper method for PrintContacts method, which generates string array from Contact properties
        /// </summary>
        /// <param name="contact">Contact value to be printed</param>
        /// <returns>String array from Contact properties</returns>
        private string[] GenerateRow(Contact contact)
        {
            return new string[]
            {
                contact.FirstName,
                contact.LastName,
                contact.PhoneNumber,
                contact.Email?? "-",
                contact.Category != null ? contact.Category.Name : "-"
            };
        }
        /// <summary>
        /// Retrieves new Contact values from user input
        /// </summary>
        /// <param name="categories">List of categories to which can be contact assigned</param>
        /// <returns>Initialized Contact object based on values from user input</returns>
        public Contact GetNewContact(List<Category> categories)
        {
            string firstName = GetName(AppStrings.CONTACT_ENTERFIRSTNAME);
            string lastName = GetName(AppStrings.CONTACT_ENTERLASTNAME);
            string phoneNumber = GetPhoneNumber(AppStrings.CONTACT_ENTERPHONENUMBER);
            string? email = GetEmail(AppStrings.CONTACT_ENTEREMAIL);
            Category? category = SelectCategory(categories, AppStrings.SELECT_CATEGORY);

            Contact newContact = new Contact()
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Category = category
            };
            PrintContact(newContact, AppStrings.CONTACT_SUMMARY);

            return newContact;
        }
        /// <summary>
        /// Prints out all contacts in the database in form of selection prompt
        /// </summary>
        /// <param name="contacts">List of contacts to be printed</param>
        /// <param name="prompt">Text to be presented as the prompt</param>
        /// <returns>Contact object selected by the user</returns>
        public Contact SelectContact(List<Contact> contacts, string prompt)
        {
            var contact = AnsiConsole.Prompt(
                new SelectionPrompt<Contact>()
                .Title(prompt)
                .PageSize(10)
                .EnableSearch()
                .AddChoices(contacts)
                .UseConverter(x => new string($"{x.FirstName} {x.LastName}"))
                );

            return contact;
        }
        /// <summary>
        /// Updates existing contact with new values
        /// Each value is presented to the user who have choice to adjust it or keep it as it is
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="categories">List of categories to which can be contact assigned</param>
        /// <returns>Contact object updated with new values</returns>
        public Contact GetUpdatedContact(Contact contact, List<Category> categories)
        {
            contact.FirstName = GetName(AppStrings.CONTACT_ENTERFIRSTNAME, contact.FirstName);
            contact.LastName = GetName(AppStrings.CONTACT_ENTERLASTNAME, contact.LastName);

            contact.PhoneNumber = GetPhoneNumber(AppStrings.CONTACT_ENTERPHONENUMBER, contact.PhoneNumber);
            contact.Email = GetEmail(AppStrings.CONTACT_ENTEREMAIL, contact.Email);
            contact.Category = SelectCategory(categories, AppStrings.SELECT_CATEGORY);

            return contact;
        }
        /// <summary>
        /// Retrieves a validated string representing Contact name
        /// </summary>
        /// <param name="prompt">Text to be presented as the prompt/param>
        /// <param name="defaultValue">optional parameter which can be presented as default value</param>
        /// <returns>String representing contact name</returns>
        private string GetName(string prompt, string? defaultValue = null)
        {
            var inputPrompt = new TextPrompt<string>(prompt)
                .Validate(x => validStringFormat.IsMatch(x));

            if (defaultValue != null)
            {
                inputPrompt.DefaultValue(defaultValue);
            }

            string input = AnsiConsole.Prompt(inputPrompt);

            return input;
        }
        /// <summary>
        /// Retrieves a validated string representing Contact email
        /// </summary>
        /// <param name="prompt">Text to be presented as the prompt</param>
        /// <param name="defaultValue">optional parameter which can be presented as default value</param>
        /// <returns>String representing contact email</returns>
        public string? GetEmail(string prompt, string? defaultValue = null)
        {
            var inputPrompt = new TextPrompt<string>(prompt)
                .Validate(x => validEmailAddressFormat.IsMatch(x) || x.ToLower() == AppStrings.NOVALUE)
                .ValidationErrorMessage(AppStrings.INVALID_EMAILFORMAT);

            if (defaultValue != null)
            {
                inputPrompt.DefaultValue(defaultValue);
            }

            var input = AnsiConsole.Prompt(inputPrompt);

            return input;
        }
        /// <summary>
        /// Prints out categories to the console in form of table
        /// </summary>
        /// <param name="categories">A List of categories to be printed</param>
        public void PrintCategories(List<Category> categories)
        {
            Table table = new Table();
            table.AddColumn("Categories");

            if (categories == null || categories.Count == 0)
            {
                table.HideHeaders();

                table.AddRow("No categories");
            }
            else
            {
                foreach (Category category in categories)
                {
                    table.AddRow(category.Name);
                }
            }

            AnsiConsole.Write(table);
            PressAnyKeyToContinue();
            Console.Clear();
        }
        /// <summary>
        /// Prints out single category to the console in form of table
        /// </summary>
        /// <param name="category">A Category to be printed</param>
        /// <param name="prompt">Text to be presented as the prompt</param>
        public void PrintCategory(Category category, string prompt)
        {
            Console.WriteLine(prompt);

            Table table = new Table();
            table.AddColumn("Category");
            table.AddRow(category.Name);
            AnsiConsole.Write(table);
        }
        /// <summary>
        /// Updates category with new values
        /// </summary>
        /// <param name="categories">A List of categories from which user can select</param>
        /// <returns>Category object with updated values</returns>
        public Category UpdateCategory(List<Category> categories)
        {
            Category category = SelectCategory(categories, AppStrings.SELECT_CATEGORY, false)!;

            var newName = GetStringFromUser(AppStrings.CATEGORY_NEWNAME);
            category.Name = newName;

            return category;
        }
        /// <summary>
        /// Retrieves new Category values from user input
        /// </summary>
        /// <param name="prompt">Text to be presented as the prompt</param>
        /// <param name="categories">A List of categories, used to check if entered value is unique</param>
        /// <returns>Initialized Category object based on values from user input</returns>
        public Category GetNewCategory(string prompt, List<Category> categories)
        {
            var input = AnsiConsole.Prompt(
                new TextPrompt<string>(AppStrings.CATEGORY_NEWNAME)
                .Validate(newCategory => !categories.Any(x => x.Name.ToLower() == newCategory.ToLower())
                    && validStringFormat.IsMatch(newCategory))
                .ValidationErrorMessage(AppStrings.INVALID_CATEGORYNAME)
                );

            return new Category() { Name = input };
        }
    }
}