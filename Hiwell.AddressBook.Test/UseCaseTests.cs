using AutoMapper;
using Hiwell.AddressBook.Core.Entities;
using Hiwell.AddressBook.Core.UseCases;
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

                Assert.AreEqual(2, response.Count);
            }
        }

        [Test]
        public async Task AddNewContact()
        {
            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                var request = new AddressBook.Core.UseCases.AddNewContactCommandRequest()
                {
                    Name = "Contact 1",
                    Address = "Address 1",
                    Email = "contact1@email.com",
                    Phone = "568498752",
                    MobilePhone = "5551112356"
                };

                var handler = new AddressBook.Core.UseCases.AddNewContactCommandHandler(dbContext, mapper);
                _ = await handler.Handle(request, CancellationToken.None);
            }

            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                var contact = await dbContext.Contacts.FirstOrDefaultAsync();
                Assert.NotNull(contact);
                Assert.AreEqual("Contact 1", contact.Name);
            }
        }

        [Test]
        public async Task UpdateContact()
        {
            var uniqueId = string.Empty;
            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                var request = new AddressBook.Core.UseCases.AddNewContactCommandRequest()
                {
                    Name = "Contact 1",
                    Address = "Address 1",
                    Email = "contact1@email.com",
                    Phone = "568498752",
                    MobilePhone = "5551112356"
                };

                var handler = new AddressBook.Core.UseCases.AddNewContactCommandHandler(dbContext, mapper);
                var response = await handler.Handle(request, CancellationToken.None);

                uniqueId = response.UniqueId;
            }

            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                var request = new AddressBook.Core.UseCases.UpdateContactCommandRequest()
                {
                    UniqueId = uniqueId,
                    Name = "Contact 2",
                    Address = "Address 1",
                    Email = "contact1@email.com",
                    Phone = "568498752",
                    MobilePhone = "5551112356"
                };

                var handler = new AddressBook.Core.UseCases.UpdateContactCommandHandler(dbContext, mapper);
                _ = await handler.Handle(request, CancellationToken.None);
            }

            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                var contact = await dbContext.Contacts.FirstOrDefaultAsync();
                Assert.NotNull(contact);
                Assert.AreEqual("Contact 2", contact.Name);
            }
        }

        [Test]
        public async Task SearchContactByName()
        {
            var uniqueId = string.Empty;
            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                var request = new AddNewContactCommandRequest()
                {
                    Name = "Contact 1",
                    Address = "Address 1",
                    Email = "contact1@email.com",
                    Phone = "568498752",
                    MobilePhone = "5551112356"
                };

                var handler = new AddNewContactCommandHandler(dbContext, mapper);
                var response = await handler.Handle(request, CancellationToken.None);
                uniqueId = response.UniqueId;
            }

            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                var handler = new SearchContactsQueryHandler(dbContext, mapper);
                var response = await handler.Handle(new SearchContactsQueryRequest() { Name = "Contact 1" }, CancellationToken.None);
                Assert.AreEqual(uniqueId, response[0].UniqueId);
            }
        }

        [Test]
        public async Task DeleteContact()
        {
            var uniqueId = string.Empty;
            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                var request = new AddressBook.Core.UseCases.AddNewContactCommandRequest()
                {
                    Name = "Contact 1",
                    Address = "Address 1",
                    Email = "contact1@email.com",
                    Phone = "568498752",
                    MobilePhone = "5551112356"
                };

                var handler = new AddressBook.Core.UseCases.AddNewContactCommandHandler(dbContext, mapper);
                var response = await handler.Handle(request, CancellationToken.None);
                uniqueId = response.UniqueId;
            }

            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                var request = new DeleteContactCommandRequest() { UniqueId = uniqueId };
                var handler = new DeleteContactCommandHandler(dbContext, this.mapper);
                var response = await handler.Handle(request, CancellationToken.None);
                Assert.IsTrue(response.Success);
            }

            using (var dbContext = new AddressBookSqliteDbContext(opts))
            {
                var thereAreContacts = await dbContext.Contacts.AnyAsync();
                Assert.IsFalse(thereAreContacts);
            }
        }
    }
}