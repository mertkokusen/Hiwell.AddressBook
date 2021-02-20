using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hiwell.AddressBook.EF.PostGreSQL
{
    public class AddressBookPostGreDbContext : DbContext, IAddressBookDbContext
    {
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=hiwellapp;Password=hiwell2021");
    }
}
