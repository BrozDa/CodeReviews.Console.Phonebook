namespace Phonebook
{
    internal static class AppStrings
    {
        //general
        public const string APP_HEADER = "Welcome to your phone book\nApplication allows you to manage your contacts\n";
        
        public const string CONFIRM_OPERATION = "Please confirm your operation";

        //input format

        public const string PHONENUMBER_FORMAT = "Phone number needs to start with '+' followed by digits, no spaces are allowed";
        public const string INVALID_STRINGFORMAT = "Can contain only alphanumeric characters with spaces in between";
        public const string INVALID_EMAILFORMAT = "Enter a email address in correct format";
        public const string INVALID_CATEGORYNAME = "Invalid input format or category already exists";

        //model related
        public static string[] CONTACT_PROPERTIES = ["First Name", "Last Name", "Phone Number", "Email", "Category"];
        public const string NOCONTACT = "No contact match your criteria";
        public const string NOCATEGORY = "Uncategorized";
        public const string NOVALUE = "-";

        public const string CONTACT_ENTERFIRSTNAME = "Enter first name: ";
        public const string CONTACT_ENTERLASTNAME = "Enter last name: ";
        public const string CONTACT_ENTERPHONENUMBER = "Enter phone number in format +xxxxxxx: ";
        public const string CONTACT_ENTEREMAIL = $"Enter email address (or enter '{NOVALUE}' to leave it blank): ";
        public const string SELECT_CATEGORY = "Please select category for the contact";

        public const string CONTACT_SUMMARY = "Following contact will be added to your phonebook";


        public const string CATEGORY_NEWNAME = "Please enter name of category (alphanumberic words with spaces in between, cannot alredy exist): ";

        //general menu
        public const string MENU_CHOOSE_OPTION = "[bold]Please select menu option:[/]";

        //main menu
        public const string MAINMENU_VIEWCONTACTS = "View contacts";
        public const string MAINMENU_ADDCONTACT = "Add new contact";
        public const string MAINMENU_UPDATECONTACT = "Update contact";
        public const string MAINMENU_DELETECONTACT = "Delete contact";
        public const string MAINMENU_MANAGECATEGORIES = "Manage categories";
        public const string MAINMENU_EXIT = "Exit";
        public const string MAINMENU_INVALIDOPTION = "Invalid enum value passed in the main menu selection";

        //view contacts menu
        public const string VIEWCONTACTMENU_VIEWALL = "View all contacts";
        public const string VIEWCONTACTMENU_FILTERBYNAME = "Filter by name";
        public const string VIEWCONTACTMENU_FILTERBYCATEGORY = "Filter by category";
        public const string VIEWCONTACTMENU_EXIT = "Return to main menu";
        public const string VIEWCONTACTMENU_INVALIDOPTION = "Invalid enum value passed in the view contacts menu selection";

        //category menu
        public const string CATEGORYMENU_VIEW = "View categories";
        public const string CATEGORYMENU_ADD = "Add new category";
        public const string CATEGORYMENU_UPDATE = "Update category";
        public const string CATEGORYMENU_DELETE = "Delete category";
        public const string CATEGORYMENU_EXIT = "Return to main menu";
        public const string CATEGORYMENU_INVALIDOPTION = "Invalid enum value passed in the category menu selection";
    }
}
