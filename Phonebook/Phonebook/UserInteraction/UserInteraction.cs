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
            Console.WriteLine("Welcome to your phone book");
            Console.WriteLine("Application allows you to manage your contacts");
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
            AnsiConsole.MarkupLine("Please confirm your operation");

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
                .ValidationErrorMessage("Can contain only alphanumeric characters with spaces in between")
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
                .ValidationErrorMessage("Phone number needs to start with '+' followed by digits, no spaces are allowed");

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

            AnsiConsole.MarkupLine("[bold]Please select menu option:[/]");
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOption>()
                .AddChoices(options)
                .UseConverter(x => x switch
                {
                    MainMenuOption.ViewContacts => "View contacts",
                    MainMenuOption.AddContact => "Add new contact",
                    MainMenuOption.UpdateContact => "Update contact",
                    MainMenuOption.DeleteContact => "Delete contact",
                    MainMenuOption.ManageCategories => "Manage categories",
                    MainMenuOption.Exit => "Exit",
                    _ => throw new NotImplementedException("Invalid enum value passed in the main menu selection")
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

            AnsiConsole.MarkupLine("[bold]Please select menu option:[/]");

            var input = AnsiConsole.Prompt(
                new SelectionPrompt<ViewContactMenuOption>()
                .AddChoices(options)
                .UseConverter(x => x switch
                {
                    ViewContactMenuOption.ViewAll => "View all contacts",
                    ViewContactMenuOption.ViewContactByName => "Filter by name",
                    ViewContactMenuOption.ViewContactByCategory => "Filter by category",
                    ViewContactMenuOption.ReturnToMainMenu => "Return to main menu",
                    _ => throw new NotImplementedException("Invalid enum value passed in the main menu selection")
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

            AnsiConsole.MarkupLine("[bold]Please select menu option:[/]");
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<CategoryMenuOption>()
                .AddChoices(options)
                .UseConverter(x => x switch
                {
                    CategoryMenuOption.ViewCategories => "View categories",
                    CategoryMenuOption.AddCategory => "Add new category",
                    CategoryMenuOption.UpdateCategory => "Update category",
                    CategoryMenuOption.DeleteCategory => "Delete category",
                    CategoryMenuOption.ReturnToMainMenu => "Return to main menu",
                    _ => throw new NotImplementedException("Invalid enum value passed in the main menu selection")
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
                categoryNames.Add("Uncategorized");
            }

            AnsiConsole.MarkupLine($"{prompt}:");

            var input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices(categoryNames)
                );

            Console.Clear();

            if (input != "Uncategorized")
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

            string categoryStr = contact.Category == null ? "-" : contact.Category.Name;

            var table = new Table();
            table.AddColumns("First Name", "Last Name", "Phone Number", "Email", "Category");
            table.AddRow(
                contact.FirstName,
                contact.LastName,
                contact.PhoneNumber,
                contact.Email ?? "-",
                contact.Category != null ? contact.Category.Name : "-");

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
                table.AddRow("No contact match your criteria");
            }
            else
            {
                table.AddColumns("First Name", "Last Name", "Phone Number", "Email", "Category");

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
            string firstName = GetName("Enter first name: ");
            string lastName = GetName("Enter last name: ");
            string phoneNumber = GetPhoneNumber("Enter phone number in format +xxxxxxx: ");
            string? email = GetEmail("Enter email address (or enter 'NO' to leave it blank): ");
            Category? category = SelectCategory(categories, "Please enter category for the contact");

            Contact newContact = new Contact()
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email,
                Category = category
            };
            PrintContact(newContact, "Following contact will be added to your phonebook");

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
            contact.FirstName = GetName("Please enter new first name:", contact.FirstName);
            contact.LastName = GetName("Please enter new last name:", contact.LastName);

            contact.PhoneNumber = GetPhoneNumber("Enter phone number in format +xxxxxxx: ", contact.PhoneNumber);
            contact.Email = GetEmail("Please enter new email value (or enter 'NO' to leave it blank): ", contact.Email);
            contact.Category = SelectCategory(categories, "Please enter category for the contact");

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
                .Validate(x => validEmailAddressFormat.IsMatch(x) || x.ToLower() == "no")
                .ValidationErrorMessage("Enter a email address in correct format");

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
            Category category = SelectCategory(categories, "Please select category to be updated", false)!;

            var newName = GetStringFromUser("Please select new category name: ");
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
                new TextPrompt<string>("Please enter name of category (alphanumberic words with spaces in between, cannot alredy exist): ")
                .Validate(newCategory => !categories.Any(x => x.Name.ToLower() == newCategory.ToLower())
                    && validStringFormat.IsMatch(newCategory))
                .ValidationErrorMessage("Invalid input format or category already exists")
                );

            return new Category() { Name = input };
        }
    }
}