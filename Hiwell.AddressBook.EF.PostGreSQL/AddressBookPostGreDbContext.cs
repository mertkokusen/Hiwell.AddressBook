using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Hiwell.AddressBook.EF.PostGreSQL
{
    public class AddressBookPostGreDbContext : DbContext, IAddressBookDbContext
    {
        private string connectionString;
        public AddressBookPostGreDbContext(IConfiguration configuration, IConnectionStringProvider connectionStringProvider)
        {
            this.connectionString = connectionStringProvider.Hiwelladdressbookdb;
        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseNpgsql(this.connectionString);
    }
}
