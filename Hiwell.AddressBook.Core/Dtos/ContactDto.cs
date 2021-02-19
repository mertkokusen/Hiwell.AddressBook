using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.Core.Mappings;

namespace Hiwell.AddressBook.Core.Dtos
{
    public class ContactDto: IMapFrom<Contact>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
    }
}
