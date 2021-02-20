using Hiwell.AddressBook.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Hiwell.AddressBook.Core.Interfaces
{
    public interface IAddressBookDbContext
    {
        //bool HasChanges();
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
        DbSet<Contact> Contacts { get; set; }
    }
}
