namespace Phonebook
{
    internal class PhoneBookService
    {
        public PhonebookContext Context { get; set; }

        public PhoneBookService(PhonebookContext context)
        {
            Context = context;            
        }

        
    }
}
