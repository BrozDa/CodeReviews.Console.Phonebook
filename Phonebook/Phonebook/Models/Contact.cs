using System.ComponentModel.DataAnnotations.Schema;

namespace Phonebook
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; } = null!;
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; } = null!;
        [Column(TypeName = "nvarchar(50)")]
        public string PhoneNumber { get; set; } = null!;
        [Column(TypeName = "nvarchar(50)")]
        public string? Email { get; set; }
        public int? ContactCategoryId { get; set; } 
        public Category? Category { get; set; }

    }
}
