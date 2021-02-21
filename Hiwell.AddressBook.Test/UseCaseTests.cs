using AutoMapper;
using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.EF.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hiwell.AddressBook.Test
{
    public class Tests
    {
        IMapper mapper;
        DbContextOptions<AddressBookSqliteDbContext> opts;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new Hiwell.AddressBook.Core.Mappings.MappingProfile());
            });

            mapper = config.CreateMapper();

            opts = new Microsoft.EntityFrameworkCore.DbContextOptions<AddressBookSqliteDbContext>();
            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                Hiwell.AddressBook.EF.Sqlite.DependecyInjection.CreateAndSeed(dbContext, deleteExistingDatabase: true, seed: false);
            }
        }

        [Test]
        public async Task GetAllContacts()
        {
            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                dbContext.Contacts.Add(new Core.Entities.Contact()
                {
                    Name = "Contact 1",
                    Address = "Address 1",
                    Email = "contact1@email.com",
                    Phone = "568498752",
                    MobilePhone = "5551112356",
                    Active = true,
                    UniqueId = Guid.NewGuid().ToString("N")
                });

                dbContext.Contacts.Add(new Core.Entities.Contact()
                {
                    Name = "Contact 2",
                    Address = "Address 2",
                    Email = "contact2@email.com",
                    Phone = "666548153",
                    MobilePhone = "5551112356",
                    Active = true,
                    UniqueId = Guid.NewGuid().ToString("N")
                });

                dbContext.SaveChanges();

                var request = new AddressBook.Core.UseCases.GetAllContactsQuery();
                var handler = new AddressBook.Core.UseCases.GetAllContactsQueryHandler(dbContext, mapper);
                var response = await handler.Handle(request, CancellationToken.None);

                Assert.AreEqual(response.Count, 2);
            }
        }
    }
}