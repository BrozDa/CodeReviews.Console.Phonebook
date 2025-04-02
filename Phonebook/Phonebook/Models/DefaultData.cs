namespace Phonebook.Models
{
    internal class DefaultData
    {
        public List<DefaultCategory> Categories { get; set; } = new();
        public List<DefaultContact> Contacts { get; set; } = new();
    }

    public class DefaultContact
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Category { get; set; }
        public int? CategoryId { get; set; } 
    }
    public class DefaultCategory
    {
        public string? Name { get; set; }
    }
}
