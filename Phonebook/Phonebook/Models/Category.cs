using System.ComponentModel.DataAnnotations.Schema;

namespace Phonebook
{
    /// <summary>
    /// Represents Category entry in the database
    /// </summary>
    public class Category
    {
        public int CategoryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = null!;

        public List<Contact> Contacts { get; set; } = new();
    }
}