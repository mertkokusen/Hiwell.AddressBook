using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.Core.Interfaces;

namespace Hiwell.AddressBook.EF.Sqlite
{
    /// <summary>
    /// This DbContext is only intended to be used for prototyping
    /// </summary>
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

        public static string GetDummyDatabasePath() => Path.Combine(Path.GetDirectoryName(typeof(AddressBookSqliteDbContext).Assembly.Location), "HiWell.AddressBook.Test.db");
    }
}
