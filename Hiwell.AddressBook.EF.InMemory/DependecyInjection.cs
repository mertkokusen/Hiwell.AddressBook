using Hiwell.AddressBook.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;

namespace Hiwell.AddressBook.EF.Sqlite
{
    public static class DependecyInjection
    {
        public static void AddSqlite(this IServiceCollection services)
        {
            services.AddDbContext<AddressBookSqliteDbContext>();

            services.AddScoped<IAddressBookDbContext>(provider => provider.GetService<AddressBookSqliteDbContext>());
        }

        public static void EnsureSqliteDbCreated(this IApplicationBuilder app, bool deleteExistingDatabase = false)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AddressBookSqliteDbContext>();

                CreateAndSeed(context, deleteExistingDatabase,true);
            }
        }

        public static void CreateAndSeed(AddressBookSqliteDbContext context, bool deleteExistingDatabase, bool seed)
        {
            if (deleteExistingDatabase)
            {
                var dbPath = AddressBookSqliteDbContext.GetDummyDatabasePath();
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }
            }

            context.Database.EnsureCreated();

            if (seed)
            {
                var hasData = context.Contacts.Count() > 0;

                //TODO: Generate random data with BOGUS 
                //https://github.com/bchavez/Bogus
                if (!hasData)
                {
                    context.Contacts.Add(new Core.Entities.Contact()
                    {
                        Name = "Contact 1",
                        Address = "Address 1",
                        Email = "contact1@email.com",
                        Phone = "568498752",
                        MobilePhone = "5551112356",
                        Active = true,
                        UniqueId = Guid.NewGuid().ToString("N")
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
