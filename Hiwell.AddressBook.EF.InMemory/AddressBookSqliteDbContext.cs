using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.Core.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Hiwell.AddressBook.EF.Sqlite
{
    public class AddressBookSqliteDbContext : DbContext, IAddressBookDbContext
    {
        public AddressBookSqliteDbContext(DbContextOptions<AddressBookSqliteDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = GetDummyDatabasePath();
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = dbPath };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }

        public DbSet<Contact> Contacts { get; set; }

        public bool HasChanges()
        {
            return this.HasChanges();
        }

        private string GetDummyDatabasePath() => Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "HiWell.AddressBook.Test.db");
    }
}
