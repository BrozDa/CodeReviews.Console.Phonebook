using System.ComponentModel.DataAnnotations.Schema;

namespace Phonebook
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = null!;
        public List<Contact> Contacts { get; set; } = new();
    }
}