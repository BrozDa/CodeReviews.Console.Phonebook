using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace Phonebook
{
    internal class UserInteraction
    {
        private Regex validStringFormat = new Regex("^[A-Za-z0-9]+(?: [A-Za-z0-9]+)*$");
        public void PrintAppHeader()
        {
            Console.WriteLine("Welcome to your phone book");
            Console.WriteLine("Application allows you to manage your contacts");
        }
        public void PressAnyKeyToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        public MainMenuOption GetMainMenuInput()
        {
            var options = Enum.GetValues<MainMenuOption>();

            var input = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOption>()
                .AddChoices(options)
                .UseConverter(x => x switch
                {
                    MainMenuOption.ViewContacts => "View contacts",
                    MainMenuOption.AddContact => "Add new contact",
                    MainMenuOption.UpdateContact => "Update contact",
                    MainMenuOption.DeleteContact => "Delete contact",
                    MainMenuOption.Exit => "Exit",
                    _ => throw new NotImplementedException("Invalid enum value passed in the main menu selection")
                }));
            return input;
        }
        public ViewContactMenuOption PrintViewContactMenu()
        {
            var options = Enum.GetValues<ViewContactMenuOption>();

            var input = AnsiConsole.Prompt(
                new SelectionPrompt<ViewContactMenuOption>()
                .AddChoices(options)
                .UseConverter(x => x switch
                {
                    ViewContactMenuOption.ViewAll => "View all contacts",
                    ViewContactMenuOption.ViewContactByName=> "Filter by name",
                    ViewContactMenuOption.ViewContactByCategory => "Filter by category",
                    ViewContactMenuOption.ReturnToMainMenu => "Return to main menu",
                    _ => throw new NotImplementedException("Invalid enum value passed in the main menu selection")
                }));
            return input;
        }
        public CategoryMenuOption PrintCategoryMenu()
        {
            var options = Enum.GetValues<CategoryMenuOption>();

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
            return input;
        }
        public Category GetCategoryFromUser(List<Category> categories)
        {
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<Category>()
                .Title("Please select category")
                .AddChoices(categories)
                .UseConverter( x => x.Name));

            return input;

        }
        public string GetStringFromUser(string prompt)
        {
            var input = AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                .Validate(x => validStringFormat.IsMatch(x))
                .ValidationErrorMessage("Can contain only alphanumeric characters with spaces in between")
                );
            return input.ToLower();
        }
        public void PrintContacts(List<Contact> contacts, List<Category> categories)
        {
            var table = new Table();
            if (contacts.IsNullOrEmpty())
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
                    table.AddRow(GenerateRow(contact, categories));
                }
            }
            AnsiConsole.Write(table);
            PressAnyKeyToContinue();
            Console.Clear();
        }
        private string[] GenerateRow(Contact contact, List<Category> categories)
        {
            var category = categories.Where(x => x.CategoryId == contact.CategoryId).FirstOrDefault();
            return new string[]
            {
                contact.FirstName,
                contact.LastName,
                contact.PhoneNumber,
                contact.Email?? "n/a",
                category != null ? category.Name : "n/a" 
            };
        }

    }
}
