using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiwell.AddressBook.Core.Entities
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int? ContactId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        [Column(TypeName = "int")]
        public bool Active { get; set; }
    }
}
