using System.ComponentModel.DataAnnotations.Schema;

namespace Phonebook
{
    public class ContactCategory
    {
        public int ContactCategoryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = null!;
        public List<Contact> Contacts { get; set; } = new();
    }
}